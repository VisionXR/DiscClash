using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class MouseInputManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public InputDataSO inputData;
    public MyPlayerSettings myPlayerSettings;
    public BoardPropertiesSO boardProperties;

    [Header("Raycast Settings")]
    [Tooltip("Camera used to convert screen -> world. If null will use Camera.main.")]
    public Camera targetCamera;
    [Tooltip("Which layers should be considered interactable (Striker / Board etc).")]
    public LayerMask interactableLayers = ~0;
    [Tooltip("Max distance for the raycast.")]
    public float maxRayDistance = 10f;

    [Header("Drag / Swipe Settings")]
    public float dragSensitivity = 3f;
    [Tooltip("Minimum screen distance (pixels) required to consider gesture a swipe.")]
    public float minSwipeDistancePixels = 50f;

    // Drag state (striker)
    private bool isTouchingStriker;
    private Vector2 initialScreenPoint;
    private Vector3 initialWorldPoint;
    private Transform touchedStrikerTransform;

    // Swipe state (separate from striker dragging)
    private bool swipeActive;
    private Vector2 swipeStartScreen;
    private float swipeStartTime;

    // Computed values (exposed for debug / consumers)
    public Vector3 LastDragDirection { get; private set; }
    public float LastDragDistance { get; private set; }


    public void StrikerPositionChanged(float val)
    {
        if(!inputData.isInputActivated) return;

        inputData.MoveStriker(val);
    }

    private void Update()
    {
        
        // handle touch
        if (Input.touchCount > 0)
        {
            var touch = Input.touches[0];
            HandlePointer(touch.position, touch.phase);
            return;
        }

        // handle mouse
        if (Input.GetMouseButtonDown(0))
        {
            HandlePointer(Input.mousePosition, TouchPhase.Began);
        }
        else if (Input.GetMouseButton(0))
        {
            HandlePointer(Input.mousePosition, TouchPhase.Moved);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandlePointer(Input.mousePosition, TouchPhase.Ended);
        }
    }

    // New unified pointer handler.
    private void HandlePointer(Vector2 screenPos, TouchPhase phase)
    {
        // 1) ignore UI touches early
        if (IsPointerOverUIObject(screenPos) && phase == TouchPhase.Began)
            return;

        // Determine horizontal zone: left 25%, middle 50%, right 25%
        float xNorm = screenPos.x / Screen.width;
        bool inLeftZone = xNorm < 0.25f;
        bool inRightZone = xNorm > 0.75f;
        bool inMiddleZone = !inLeftZone && !inRightZone;

        // For Began: decide whether to start striker/ swipe / direct swipe zones
        if (phase == TouchPhase.Began)
        {
            // Left/right zones -> candidate quick swipe (only if not hitting board/striker)
            if (inLeftZone || inRightZone)
            {
              
                    var dir = inLeftZone ? SwipeDirection.LEFT : SwipeDirection.RIGHT;

                //  inputData.Swiped(dir);
                // do not start striker/swipe state in this case
                    BeginSwipe(screenPos);
                    isTouchingStriker = false;
                    return;
                
            }

            // Middle zone: if input is activated try striker detection and start striker drag; otherwise treat as potential swipe start
            if (inMiddleZone)
            {
                if (inputData.isInputActivated)
                {
                    if (TryRaycastForStriker(screenPos, out RaycastHit hit, out Transform striker))
                    {
                        BeginTouchOnStriker(screenPos, hit.point, striker);
                        return;
                    }
                    // not a striker/board hit -> check for coin hit and fire coin event
                    if (Physics.Raycast(targetCamera.ScreenPointToRay(screenPos), out RaycastHit coinHit, maxRayDistance))
                    {
                        var t = coinHit.transform;
                        if (t != null)
                        {
                            string tag = t.gameObject.tag;
                            if (tag == "Black" || tag == "White" || tag == "Red")
                            {
                               // inputData.CoinSwiped(t.gameObject);
                                return;
                            }
                        }
                    }
                  
                    return;
                }
                else
                {
                 
                    return;
                }
            }

          
            return;
        }

        // Moved / Stationary: update striker drag or cancel swipe if hits board/striker
        if (phase == TouchPhase.Moved || phase == TouchPhase.Stationary)
        {
            if (isTouchingStriker)
            {
                ContinueTouch(screenPos);
                return;
            }

            if (swipeActive)
            {
                if (IsPointerOverUIObject(screenPos))
                {
                    CancelSwipe();
                }
            }
            return;
        }

        // Ended / Canceled: finalize striker or swipe
        if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
        {
            if (isTouchingStriker)
            {
                EndTouch();
                return;
            }

            if (swipeActive)
            {
                // Determine left/right/up/down swipe from start->end, but only fire if path did not hit board/striker or UI
                EndSwipe(screenPos);
            }
        }
    }

    private void BeginTouchOnStriker(Vector2 screenPoint, Vector3 worldPoint, Transform striker)
    {
        // cancel any swipe that might be active
        CancelSwipe();

        isTouchingStriker = true;
        initialScreenPoint = screenPoint;
        initialWorldPoint = worldPoint;
        touchedStrikerTransform = striker;
        LastDragDirection = Vector3.zero;
        LastDragDistance = 0f;
    }

    private void ContinueTouch(Vector2 currentScreenPoint)
    {
        // Project current screen point onto horizontal plane at initialWorldPoint.y
        Plane plane = new Plane(Vector3.up, initialWorldPoint);
        Ray ray = targetCamera.ScreenPointToRay(currentScreenPoint);
        Vector3 currentWorldPoint;
        if (plane.Raycast(ray, out float enter) && enter > 0f)
        {
            currentWorldPoint = ray.GetPoint(enter);
        }
        else
        {
            // fallback: use a point along the ray at max distance
            currentWorldPoint = ray.GetPoint(maxRayDistance);
        }

        Vector3 delta = currentWorldPoint - initialWorldPoint;
        Vector3 direction = (initialWorldPoint - currentWorldPoint).normalized;

        inputData.AimStriker(direction);

        // compute normalized force based on drag distance and sensitivity * strikerRadius
        float strikerRadius = 0.02f;
        if (boardProperties != null)
            strikerRadius = boardProperties.GetStrikerRadius();

        float maxDragDistance = Mathf.Max(0.0001f, dragSensitivity * strikerRadius); // avoid div by zero
        float dragDistance = delta.magnitude;
        float normalizedForce = Mathf.Clamp01(dragDistance / maxDragDistance);

        // publish normalized force (0..1)
        inputData.SetStrikerForce(normalizedForce);

        LastDragDirection = direction;
        LastDragDistance = dragDistance;
    }

    private void EndTouch()
    {
        isTouchingStriker = false;
        touchedStrikerTransform = null;
        inputData.FireStriker();
    }

    // --- Swipe helpers -------------------------------------------------

    private void BeginSwipe(Vector2 screenPoint)
    {
        swipeActive = true;
        swipeStartScreen = screenPoint;
        swipeStartTime = Time.time;
    }

    private void CancelSwipe()
    {
        swipeActive = false;
    }

    private void EndSwipe(Vector2 endScreenPoint)
    {
        swipeActive = false;

        float distance = Vector2.Distance(endScreenPoint, swipeStartScreen);
        if (distance < minSwipeDistancePixels)
            return; // too small to be a swipe

        Vector2 delta = endScreenPoint - swipeStartScreen;
        SwipeDirection dir;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            // horizontal
            dir = delta.x > 0 ? SwipeDirection.RIGHT : SwipeDirection.LEFT;
        }
        else
        {
            // vertical
            dir = delta.y > 0 ? SwipeDirection.UP : SwipeDirection.DOWN;
        }

        // Final safety: ensure midpoint did not hit UI/board/striker
        Vector2 midPoint = (swipeStartScreen + endScreenPoint) * 0.5f;
        if (IsPointerOverUIObject(midPoint))
            return;

        inputData.SwipeDetected(dir);
    }

    /// <summary>
    /// Returns true when the provided screen position is over any UI element.
    /// Uses the EventSystem raycast path (works for Graphics UI and World Space canvases).
    /// </summary>
    /// <param name="screenPosition">Screen position (Input.mousePosition or touch.position)</param>
    /// <returns>True if the pointer is over any UI object</returns>
    private bool IsPointerOverUIObject(Vector2 screenPosition)
    {
        if (EventSystem.current == null)
            return false;

        var eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

    /// <summary>
    /// Raycast from screen point and determine if a striker was hit directly OR a board was hit
    /// and a striker is overlapping the hit point (using OverlapSphere).
    /// Returns true if a striker interaction should begin. Out parameters contain the striker transform (if any).
    /// </summary>
    private bool TryRaycastForStriker(Vector2 screenPoint, out RaycastHit hitInfo, out Transform strikerTransform)
    {
        strikerTransform = null;
        hitInfo = default;

        Ray ray = targetCamera.ScreenPointToRay(screenPoint);
        if (Physics.Raycast(ray, out hitInfo, maxRayDistance, interactableLayers))
        {
            GameObject hitObject = hitInfo.transform.gameObject;
            if (hitObject != null)
            {
                // direct striker hit
                if (hitObject.CompareTag("Striker"))
                {
                    strikerTransform = hitInfo.transform;
                    return true;
                }

                // board hit -> check overlap sphere for any striker colliders near the hit point
                if (hitObject.CompareTag("Board"))
                {
                    float strikerRadius = 0.5f;
                    if (boardProperties != null)
                        strikerRadius = boardProperties.GetStrikerRadius();
                    else
                        Debug.LogWarning("MouseInputManager: boardProperties is not assigned. Using fallback striker radius.");

                    float checkRadius = strikerRadius * 2.5f;
                    Collider[] cols = Physics.OverlapSphere(hitInfo.point, checkRadius);
                    foreach (var c in cols)
                    {
                        if (c == null) continue;
                        if (c.gameObject.CompareTag("Board")) continue;

                        var striker = c.GetComponentInParent<com.VisionXR.GameElements.StrikerMovement>();
                        if (striker != null)
                        {
                            strikerTransform = striker.transform;
                            return true;
                        }

                        if (c.gameObject.CompareTag("Striker"))
                        {
                            strikerTransform = c.transform;
                            return true;
                        }
                    }

                    return false;
                }

                // other object: maybe parent is striker
                var parentStriker = hitInfo.transform.GetComponentInParent<com.VisionXR.GameElements.StrikerMovement>();
                if (parentStriker != null)
                {
                    strikerTransform = parentStriker.transform;
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Perform a physics raycast from the screen point and return true if it hits board or striker.
    /// Used to cancel/ignore swipe gestures if the touch collides with game objects.
    /// </summary>
    private bool RaycastHitsBoardOrStriker(Vector2 screenPoint)
    {
        Ray ray = targetCamera.ScreenPointToRay(screenPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, interactableLayers))
        {
            var go = hit.transform.gameObject;
            if (go.CompareTag("Board") || go.CompareTag("Striker"))
                return true;

            if (hit.transform.GetComponentInParent<com.VisionXR.GameElements.StrikerMovement>() != null)
                return true;
        }
        return false;
    }
}

[Serializable]
public enum SwipeDirection { LEFT,RIGHT,UP,DOWN}

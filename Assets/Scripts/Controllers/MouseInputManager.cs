using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using com.VisionXR.HelperClasses;

public class MouseInputManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public InputDataSO inputData;
    public MyPlayerSettings myPlayerSettings;
    public BoardDataSO boardData;



    // Computed values (exposed for debug / consumers)
    public Vector3 LastDragDirection { get; private set; }
    public float LastDragDistance { get; private set; }


    public void StrikerPositionChanged(float val)
    {
        if (!inputData.isInputActivated) return;

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

        // handle mouse (map mouse to touch phases)
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

    // New simplified pointer handler: only forwards touch events to InputDataSO (TouchStarted/Continued/Ended)
    private void HandlePointer(Vector2 screenPos, TouchPhase phase)
    {
        // 1) ignore UI touches early (only for Began)
        if (IsPointerOverUIObject(screenPos) && phase == TouchPhase.Began)
            return;

        // Determine horizontal zone: left 25%, middle 50%, right 25%
        float xNorm = screenPos.x / Screen.width;
        bool inLeftZone = xNorm < 0.25f;
        bool inRightZone = xNorm > 0.75f;
        TouchZone zone = inLeftZone ? TouchZone.LEFT : (inRightZone ? TouchZone.RIGHT : TouchZone.MIDDLE);

        switch (phase)
        {
            case TouchPhase.Began:
                inputData.TouchStarted(zone, screenPos);
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                inputData.TouchContinued(zone, screenPos);
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                inputData.TouchEnded(zone, screenPos);
                break;
        }
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

}



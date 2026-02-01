using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;


namespace com.VisionXR.GameElements
{

    public class PlayerInput : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public InputDataSO inputData;
        public CoinDataSO coinData;
        public BoardDataSO boardData;

        [Header("Local Objects")]
        public Player player;
        public bool canIRotate = false;

        [Tooltip("Max distance for the raycast.")]
        public float maxRayDistance = 10f;

        [Header("Drag / Swipe Settings")]
        [Tooltip("Multiplier for how far dragging translates to normalized force (higher = less force for same drag).")]
        public float dragSensitivity = 5f;

        [Tooltip("Sensitivity mapping from horizontal swipe pixels -> rotation degrees for coins.")]
        public float coinRotateSensitivity = 0.2f;


        // Drag state (striker)
        private bool isTouchingStriker;
        private Vector2 initialScreenPoint;
        private Vector3 initialWorldPoint;
        private Transform touchedStrikerTransform;

        // Coin drag state
        private bool isTouchingCoins;
        private Vector2 initialCoinScreenPoint;
        private Vector3 initialCoinWorldPoint;
        private Transform touchedCoinTransform;
        private float _lastCoinScreenX;


        private void OnEnable()
        {
            
            inputData.MoveStrikerEvent += MoveStriker;

            inputData.TouchStartedEvent += TouchStarted;
            inputData.TouchContinuedEvent += TouchContinued;
            inputData.TouchEndedEvent += TouchEnded;
        }

        private void OnDisable()
        {
           
            inputData.MoveStrikerEvent -= MoveStriker;

            inputData.TouchStartedEvent -= TouchStarted;
            inputData.TouchContinuedEvent -= TouchContinued;
            inputData.TouchEndedEvent -= TouchEnded;
        }


        private void TouchStarted(TouchZone zone, Vector2 pos)
        {
            // mirror MouseInputManager: only handle striker detection in MIDDLE zone
            if (zone != TouchZone.MIDDLE) return;

            if (!inputData.isInputActivated) return;

            // 1) Try striker first
            if (TryRaycastForStriker(pos, out RaycastHit hit, out Transform striker))
            {
                // Begin touch on striker
                CancelCurrentTouch(); // reset safety
                isTouchingStriker = true;
                initialScreenPoint = pos;
                initialWorldPoint = hit.point;
                touchedStrikerTransform = striker;

                AppProperties.instance.PlayVibration();
                return;
            }

            // 2) Not a striker: try coin detection (tags White/Black/Red)
            if (TryRaycastForCoin(pos, out RaycastHit coinHit, out Transform coin))
            {
                CancelCurrentTouch();
                isTouchingCoins = true;
                initialCoinScreenPoint = pos;
                initialCoinWorldPoint = coinHit.point;
                touchedCoinTransform = coin;
                _lastCoinScreenX = pos.x;

                // Optionally show rotation canvas
                //   coinData.ShowRotationCanvasEvent?.Invoke();
                AppProperties.instance.PlayVibration();
                return;
            }

            // otherwise nothing to do on start
        }

        private void TouchContinued(TouchZone zone, Vector2 pos)
        {
            // Striker handling
            if (isTouchingStriker)
            {
                // Project current screen point onto horizontal plane at initialWorldPoint.y
                Camera cam = Camera.main;
                if (cam == null) return;

                Plane plane = new Plane(Vector3.up, initialWorldPoint);
                Ray ray = cam.ScreenPointToRay(pos);
                Vector3 currentWorldPoint;
                if (plane.Raycast(ray, out float enter) && enter > 0f)
                {
                    currentWorldPoint = ray.GetPoint(enter);
                }
                else
                {
                    currentWorldPoint = ray.GetPoint(maxRayDistance);
                }

                Vector3 delta = currentWorldPoint - initialWorldPoint;
                Vector3 direction = (initialWorldPoint - currentWorldPoint).normalized;

                // Publish aiming
                player.strikerMovement.AimStriker(direction);

                // compute normalized force based on drag distance and sensitivity * strikerRadius
                float strikerRadius = 0.02f;
                if (boardData != null)
                    strikerRadius = boardData.GetStrikerRadius();

                float maxDragDistance = Mathf.Max(0.001f, dragSensitivity * strikerRadius); // avoid div by zero
                float dragDistance = delta.magnitude;
                float normalizedForce = Mathf.Clamp01(dragDistance / maxDragDistance);

                // publish normalized force (0..1)
                player.strikerShoot.SetStrikerForce(normalizedForce);
                return;
            }

            // Coin handling: compute horizontal swipe delta (pixels) and rotate coins accordingly
            if (isTouchingCoins)
            {
                float deltaX = pos.x - _lastCoinScreenX;
                float rotationDegrees = deltaX * coinRotateSensitivity;

                // call RotateCoins (player wrapper applies canIRotate check)
                RotateCoins(rotationDegrees);

                _lastCoinScreenX = pos.x;
                return;
            }
        }

        private void TouchEnded(TouchZone zone, Vector2 pos)
        {
            if (isTouchingStriker)
            {
                Camera cam = Camera.main;
                if (cam == null)
                {
                    ResetTouchState();
                    return;
                }

                Plane plane = new Plane(Vector3.up, initialWorldPoint);
                Ray ray = cam.ScreenPointToRay(pos);
                Vector3 currentWorldPoint;
                if (plane.Raycast(ray, out float enter) && enter > 0f)
                {
                    currentWorldPoint = ray.GetPoint(enter);
                }
                else
                {
                    currentWorldPoint = ray.GetPoint(maxRayDistance);
                }

                Vector3 delta = currentWorldPoint - initialWorldPoint;

                // compute normalized force based on drag distance and sensitivity * strikerRadius
                float strikerRadius = 0.02f;
                if (boardData != null)
                    strikerRadius = boardData.GetStrikerRadius();

                float maxDragDistance = Mathf.Max(0.001f, dragSensitivity * strikerRadius); // avoid div by zero
                float dragDistance = delta.magnitude;
                float normalizedForce = Mathf.Clamp01(dragDistance / maxDragDistance);

                // Fire with computed force
                AppProperties.instance.PlayStrikerVibration();
                player.strikerShoot.FireStriker(normalizedForce);

                ResetTouchState();
                return;
            }

            if (isTouchingCoins)
            {
               
                ResetTouchState();
                return;
            }
        }

        private void ResetTouchState()
        {
            isTouchingStriker = false;
            touchedStrikerTransform = null;
            initialScreenPoint = Vector2.zero;
            initialWorldPoint = Vector3.zero;

            isTouchingCoins = false;
            touchedCoinTransform = null;
            initialCoinScreenPoint = Vector2.zero;
            initialCoinWorldPoint = Vector3.zero;
            _lastCoinScreenX = 0f;
        }

        private void CancelCurrentTouch()
        {
            ResetTouchState();
        }


        private void MoveStriker(float val)
        {
            player.strikerMovement.MoveStriker(val);
        }




        private void RotateCoins(float val)
        {
            if (canIRotate)
            {
                coinData.RotateCoins(val);
                player.AllCoinsRotatedEvent?.Invoke(coinData.AllCoinsYRotationValue);
            }
        }


        public void StartRotation()
        {
            canIRotate = true;
        }

        public void StopRotation()
        {
            canIRotate = false;
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

            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out hitInfo, maxRayDistance))
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
                        float strikerRadius = boardData.StrikerRadius;

                        float checkRadius = strikerRadius * 2.5f;
                        Collider[] cols = Physics.OverlapSphere(hitInfo.point, checkRadius);
                        foreach (var c in cols)
                        {
                            if (c == null) continue;
                            if (c.gameObject.CompareTag("Board")) continue;

                            var striker = c.GetComponentInParent<StrikerMovement>();
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
                    var parentStriker = hitInfo.transform.GetComponentInParent<StrikerMovement>();
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
        /// Raycast from screen point and determine if a coin was hit directly OR a board was hit
        /// and a coin is overlapping the hit point (using OverlapSphere).
        /// Returns true if a coin interaction should begin. Out parameters contain the coin transform (if any).
        /// </summary>
        private bool TryRaycastForCoin(Vector2 screenPoint, out RaycastHit hitInfo, out Transform coinTransform)
        {
            coinTransform = null;
            hitInfo = default;

            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out hitInfo, maxRayDistance))
            {
                GameObject hitObject = hitInfo.transform.gameObject;
                if (hitObject != null)
                {
                    // direct coin hit by tag
                    string tag = hitObject.tag;
                    if (tag == "White" || tag == "Black" || tag == "Red")
                    {
                        coinTransform = hitInfo.transform;
                        return true;
                    }

                    // board hit -> check overlap sphere for any coin colliders near the hit point
                    if (hitObject.CompareTag("Board"))
                    {
                        float coinRadius = 0.02f;
                        if (boardData != null)
                            coinRadius = boardData.GetCoinRadius();

                        float checkRadius = coinRadius * 2.5f;
                        Collider[] cols = Physics.OverlapSphere(hitInfo.point, checkRadius);
                        foreach (var c in cols)
                        {
                            if (c == null) continue;
                            if (c.gameObject.CompareTag("Board")) continue;

                            string t = c.gameObject.tag;
                            if (t == "White" || t == "Black" || t == "Red")
                            {
                                coinTransform = c.transform;
                                return true;
                            }
                        }

                        return false;
                    }

                    // other object: maybe parent is coin
                    var parentCoin = hitInfo.transform.GetComponentInParent<Rigidbody>();
                    if (parentCoin != null && (parentCoin.gameObject.tag == "White" || parentCoin.gameObject.tag == "Black" || parentCoin.gameObject.tag == "Red"))
                    {
                        coinTransform = parentCoin.transform;
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
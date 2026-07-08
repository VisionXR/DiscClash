using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;



namespace com.VisionXR.Controllers
{
    public class MobileInputManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public InputDataSO inputData;
        public MyPlayerSettings userData;
        public StrikerDataSO strikerData;
        public UIDataSO uiData;

        public GameDataSO gameData;


        [Header("Swipe Settings")]
        public float swipeminDistanceThreshold = 100f; // Minimum pixels to register a swipe
        public float swipemaxDistanceThreshold = 400f; // Minimum pixels to register a swipe
        public float swipeminTimeThreshold = 0.1f; // Minimum time for a swipe (seconds)
        public float swipemaxTimeThreshold = 1; // Maximum time for a swipe (seconds)


        //local variables
        public LayerMask boardLayerMask;
        private Vector2 swipeStartPosition;
        private float swipeStartTime;
        public float cutoffValue = 0.1f;
        public float movementswipeSensitivity = 1;
        public float aimswipeSensitivity = 1;
        public bool isSwipeStarted = false;
        public bool isAimStarted = false;



        private void OnEnable()
        {
            // 3. You MUST enable EnhancedTouch once
             EnhancedTouchSupport.Enable();
        }
        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }
        private void Start()
        {
            if (!Input.touchSupported && !Application.isEditor)
            {
                this.enabled = false;
            }
        }

        private void LateUpdate()
        {

            // In the New Input System, the Android back button natively maps to the Escape Key
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                AudioManager.instance.PlayButtonClickSound();
                uiData.uiManager.GoToState(HelperClasses.StateName.QuitState);
            }



            if (!inputData.isInputEnabled) return;

            HandleTouchInput();
        }



        private void HandleTouchInput()
        {


            var activeTouches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;

            if (activeTouches.Count == 1)
            {
                var touch = activeTouches[0];
                switch (touch.phase)
                {
                    case UnityEngine.InputSystem.TouchPhase.Began:
                        HandleTouchBegan(touch.screenPosition);
                        break;

                    case UnityEngine.InputSystem.TouchPhase.Moved:
                    case UnityEngine.InputSystem.TouchPhase.Stationary:
                        HandleTouchUpdate(touch.screenPosition);
                        break;

                    case UnityEngine.InputSystem.TouchPhase.Ended:
                    case UnityEngine.InputSystem.TouchPhase.Canceled:
                        HandleTouchEnded(touch.screenPosition);
                        break;
                }
            }

            // Fallback for Editor testing with mouse
            if (!Application.isEditor)
                return;

            var pointer = Pointer.current;
            if (pointer != null)
            {
                Vector2 pointerPos = pointer.position.ReadValue();

                if (pointer.press.wasPressedThisFrame)
                {
                    HandleTouchBegan(pointerPos);
                }
                else if (pointer.press.isPressed)
                {
                    HandleTouchUpdate(pointerPos);
                }
                else if (pointer.press.wasReleasedThisFrame)
                {
                    HandleTouchEnded(pointerPos);
                }
            }
        }

        private void HandleTouchBegan(Vector2 touch)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //  Debug.Log("Pointer on ui");
                return;
            }

            swipeStartPosition = touch;
            swipeStartTime = Time.time;


            Ray ray = Camera.main.ScreenPointToRay(touch);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if ((boardLayerMask.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    isSwipeStarted = true;
                    return;
                }
            }


            isAimStarted = true;

        }

        private void HandleTouchUpdate(Vector2 touch)
        {
            if (isSwipeStarted) // Board Raycast Hit -> Move Striker Left/Right
            {
                // 1. Calculate horizontal pixel delta from previous tracking point
                float deltaX = touch.x - swipeStartPosition.x;

                // 2. Normalize it against screen width
                float normalizedDeltaX = deltaX / Screen.width;

                // 3. Scale by your sensitivity (e.g., if a small swipe should map to full movement range)
                float movementDelta = normalizedDeltaX * movementswipeSensitivity;

                // 4. Clamp the output strictly between -1 and +1
                movementDelta = Mathf.Clamp(movementDelta, -1f, 1f);

              

                //if (Mathf.Abs(movementDelta) > cutoffValue)
                //{
                // Fire the Move Striker event instead of the old SwipePinch Continued
                inputData.MoveStriker(movementDelta);

                    // Reset tracking markers dynamically for frame-by-frame delta relative updates
                    swipeStartPosition = touch;
                    swipeStartTime = Time.time;
               // }
            }
            else if (isAimStarted) // No Board Hit -> Rotate Striker Delta Angle
            {
                float deltaX = touch.x - swipeStartPosition.x;
                float normalizedDeltaX = deltaX / Screen.width;
                float angleDelta = normalizedDeltaX * aimswipeSensitivity;

                //if (Mathf.Abs(angleDelta) > cutoffValue)
                //{
                    inputData.RotateStrikerAbsolute(angleDelta);
                    swipeStartPosition = touch;
                    swipeStartTime = Time.time;
               // }
            }
        }

        private void HandleTouchEnded(Vector2 touch)
        {
            if (isSwipeStarted)
            {
                // Final micro-movement evaluation on release
                float deltaX = touch.x - swipeStartPosition.x;
                float normalizedDeltaX = deltaX / Screen.width;
                float movementDelta = Mathf.Clamp(normalizedDeltaX * movementswipeSensitivity, -1f, 1f);


                inputData.MoveStriker(movementDelta);
                

                isSwipeStarted = false;
            }
            else if (isAimStarted)
            {
                float deltaX = touch.x - swipeStartPosition.x;
                float normalizedDeltaX = deltaX / Screen.width;
                float angleDelta = normalizedDeltaX * aimswipeSensitivity;

                //if (Mathf.Abs(angleDelta) > cutoffValue)
                //{
                    inputData.RotateStrikerAbsolute(angleDelta);
              //  }

                isAimStarted = false;
            }
        }

    }
}

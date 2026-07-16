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
        public float swipeminDistanceThreshold = 100f;
        public float swipemaxDistanceThreshold = 400f;
        public float swipeminTimeThreshold = 0.1f;
        public float swipemaxTimeThreshold = 1;

        // Local variables
        private Vector2 swipeStartPosition;
        private float swipeStartTime;
        public float cutoffValue = 0.1f;
        public float movementswipeSensitivity = 1;
        public float aimswipeSensitivity = 1;
        public bool isSwipeStarted = false;
        public bool isAimStarted = false;

        private void OnEnable()
        {
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
            // Android back button maps to Escape Key
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

        private void HandleTouchBegan(Vector2 touchPosition)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            swipeStartPosition = touchPosition;
            swipeStartTime = Time.time;

            // Calculate the screen height threshold for the bottom 25%
            float bottomRegionThreshold = Screen.height * 0.25f;

            // Check if the touch started in the bottom 25% area
            if (touchPosition.y <= bottomRegionThreshold)
            {
                isSwipeStarted = true;
            }
            else
            {
                isAimStarted = true;
            }
        }

        private void HandleTouchUpdate(Vector2 touch)
        {
            if (isSwipeStarted) // Bottom 25% -> Move Striker Left/Right
            {
                float deltaX = touch.x - swipeStartPosition.x;
                float normalizedDeltaX = deltaX / Screen.width;
                float movementDelta = normalizedDeltaX * movementswipeSensitivity;
                movementDelta = Mathf.Clamp(movementDelta, -1f, 1f);

                inputData.MoveStriker(movementDelta);

                swipeStartPosition = touch;
                swipeStartTime = Time.time;
            }
            else if (isAimStarted) // Top 75% -> Rotate Striker Delta Angle
            {
                float deltaX = touch.x - swipeStartPosition.x;
                float normalizedDeltaX = deltaX / Screen.width;
                float angleDelta = normalizedDeltaX * aimswipeSensitivity;

                inputData.RotateStrikerAbsolute(angleDelta);

                swipeStartPosition = touch;
                swipeStartTime = Time.time;
            }
        }

        private void HandleTouchEnded(Vector2 touch)
        {
            if (isSwipeStarted)
            {
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

                inputData.RotateStrikerAbsolute(angleDelta);
                isAimStarted = false;
            }
        }
    }
}
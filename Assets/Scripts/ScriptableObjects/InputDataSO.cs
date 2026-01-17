
using com.VisionXR.HelperClasses;
using System;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "InputDataSO", menuName = "ScriptableObjects/InputDataSO", order = 1)]
    public class InputDataSO : ScriptableObject
    {
        // variables
        public MyPlayerSettings myPlayerSettings;
        public bool isInputActivated;
        public bool isGamePaused;
        public bool isHandTrackingActive = false;

        // Events
        public Action<float> MovePlayerXEvent;
        public Action<float> MovePlayerYEvent;


        public Action<float> FireStrikerWithForceEvent;
        public Action TriggerButtonClickedEvent;
        public Action GrabButtonClickedEvent;
        public Action<float> ControllerRotatedEvent;
        public Action<Vector3, Transform> SwipedPositionEvent;
        public Action<SwipeDirection> KeyboardRotatedEvent;
        public Action<SwipeDirection> SwipedEvent;


        public Action PauseGameEvent;
        public Action ResumeGameEvent;


        public Action<float> ThumbStickLeftRightSwipedEvent;
        public Action<float> ThumbStickUpDownSwipedEvent;

        // Striker Events
        public Action<float> RotateStrikerAbsoluteEvent;
        public Action<float> RotateStrikerRelativeEvent;
        public Action<Vector3> RotateStrikerTowardsEvent;


        // Methods

        private void OnEnable()
        {
            isInputActivated = false;
        }
        public void MovePlayerX(float value)
        {
            MovePlayerXEvent?.Invoke(value);
        }

        public void MovePlayerY(float value)
        {
            MovePlayerYEvent?.Invoke(value);
        }
        public void FireStrikerWithForce(float force)
        {
            FireStrikerWithForceEvent?.Invoke(force);
        }
        public void TriggerButtonClicked()
        {
            TriggerButtonClickedEvent?.Invoke();
        }
        public void GrabButtonClicked()
        {
            GrabButtonClickedEvent?.Invoke();
        }
        public void ActivateInput()
        {
            isInputActivated = true;
            myPlayerSettings.ChangeHand(myPlayerSettings.myDominantHand);
        }

        public void DeactivateInput()
        {
            isInputActivated = false;
            myPlayerSettings.ChangeHand(DominantHand.BOTH);
        }

        public void ControllerRotated(float angle)
        {
            ControllerRotatedEvent?.Invoke(angle);
        }

        public void RotateStrikerTowards(Vector3 initialPosition)
        {
            RotateStrikerTowardsEvent?.Invoke(initialPosition);
        }

     

        public void SwipedPosition(Vector3 position, Transform transform)
        {
            SwipedPositionEvent?.Invoke(position, transform);
        }

        public void KeyboardRotated(SwipeDirection direction)
        {
            KeyboardRotatedEvent?.Invoke(direction);
        }

        public void Swiped(SwipeDirection direction)
        {
            SwipedEvent?.Invoke(direction);
        }

        public void PauseButtonClicked()
        {
            if(!isGamePaused)
            {
                isGamePaused = true;
                PauseGameEvent?.Invoke();
               
            }
            else
            {
                isGamePaused = false;
                ResumeGameEvent?.Invoke();
               
            }
        }

        public void ThumbStickLeftRightSwiped(float value)
        {
            ThumbStickLeftRightSwipedEvent?.Invoke(value);
        }
        public void ThumbStickUpDownSwiped(float value)
        {
            ThumbStickUpDownSwipedEvent?.Invoke(value);
        }
    }
}

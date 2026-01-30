
using com.VisionXR.HelperClasses;
using System;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "InputDataSO", menuName = "ScriptableObjects/InputDataSO", order = 1)]
    public class InputDataSO : ScriptableObject
    {
        // variables
        
        public bool isInputActivated;
        public bool isGamePaused;
        
       
      // Striker Events
        public Action<float> MoveStrikerEvent;
        public Action<Vector3> AimStrikerEvent;
        public Action<float> SetStrikerForceEvent;
        public Action<float> FireStrikerEvent;

        // camEvents
        public Action<SwipeDirection> SwipeDetectedEvent;


        // Touch Events
        public Action<TouchZone,Vector2> TouchStartedEvent;
        public Action<TouchZone,Vector2> TouchContinuedEvent;
        public Action<TouchZone,Vector2> TouchEndedEvent;

        // Methods

        private void OnEnable()
        {
            isInputActivated = false;
        }

        public void ActivateInput()
        {
            isInputActivated = true;
          
        }

        public void DeactivateInput()
        {
            isInputActivated = false;

        }

        public void MoveStriker(float val)
        {
            MoveStrikerEvent?.Invoke(val);
        }

        public void AimStriker(Vector3 direction)
        {
            AimStrikerEvent?.Invoke(direction);
        }

        public void SetStrikerForce(float normalisedValue)
        {
            SetStrikerForceEvent?.Invoke(normalisedValue);
        }

        public void FireStriker(float val)
        {
            FireStrikerEvent?.Invoke(val);
        }

        public void SwipeDetected(SwipeDirection swipeDirection)
        {
            
            SwipeDetectedEvent?.Invoke(swipeDirection);
        }


        public void TouchStarted(TouchZone zone,Vector2 screenPos)
        {
            TouchStartedEvent?.Invoke(zone,screenPos);
        }

        public void TouchContinued(TouchZone zone,Vector2 screenPos)
        {
            TouchContinuedEvent?.Invoke(zone,screenPos);
        }

        public void TouchEnded(TouchZone zone,Vector2 screenPos)
        {
            TouchEndedEvent?.Invoke(zone,screenPos);
        }

    }
}

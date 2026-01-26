
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



       
      // Striker Events
        public Action<float> MoveStrikerEvent;
        public Action<Vector3> AimStrikerEvent;
        public Action<float> SetStrikerForceEvent;
        public Action FireStrikerEvent;

        // camEvents
        public Action<SwipeDirection> SwipeDetectedEvent;





        // Methods

        private void OnEnable()
        {
            isInputActivated = false;
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

        public void FireStriker()
        {
            FireStrikerEvent?.Invoke();
        }

        public void SwipeDetected(SwipeDirection swipeDirection)
        {
            Debug.Log("Swipe Detedted "+Enum.GetName(typeof(SwipeDirection), swipeDirection));  
            SwipeDetectedEvent?.Invoke(swipeDirection);
        }


    }
}

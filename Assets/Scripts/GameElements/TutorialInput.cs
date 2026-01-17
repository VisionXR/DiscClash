using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;


namespace com.VisionXR.GameElements
{

    public class TutorialInput : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public InputDataSO inputData;
        public TutorialDataSO tutorialData;

        [Header("Game Objects")]
        public GameObject striker;
        private IStrikerMovement strikerMovement;

        public void RegisterEvents()
        {
            strikerMovement = striker.GetComponent<IStrikerMovement>();
            inputData.FireStrikerWithForceEvent += FireStrikerWithForce;
            inputData.KeyboardRotatedEvent += KeyboardRotated;
            inputData.SwipedEvent += Swiped;
            inputData.SwipedPositionEvent += OnControllerMoved;
            inputData.ControllerRotatedEvent += OnControllerRotated;
            inputData.RotateStrikerTowardsEvent += OnHandRotated;
            inputData.GrabButtonClickedEvent += OnGrabButtonClicked;
            inputData.TriggerButtonClickedEvent += OnTriggerButtonClicked;
        }

        public void DeRegisterEvents()
        {
            inputData.FireStrikerWithForceEvent -= FireStrikerWithForce;
            inputData.KeyboardRotatedEvent -= KeyboardRotated;
            inputData.SwipedEvent -= Swiped;
            inputData.SwipedPositionEvent -= OnControllerMoved;
            inputData.ControllerRotatedEvent -= OnControllerRotated;
            inputData.RotateStrikerTowardsEvent -= OnHandRotated;
            inputData.GrabButtonClickedEvent -= OnGrabButtonClicked;
            inputData.TriggerButtonClickedEvent -= OnTriggerButtonClicked;
        }



        private void OnTriggerButtonClicked()
        {
            if (tutorialData.canIAim && ! tutorialData.canIFire)
            {
               
                tutorialData.CheckAim();
            }
        }

        private void OnGrabButtonClicked()
        {
            if (tutorialData.canIPosition && !tutorialData.canIAim)
            {
               
                tutorialData.CheckPosition();
            }
        }

        private void OnControllerMoved(Vector3 vector, Transform transform)
        {
            if (tutorialData.canIPosition)
            {
                strikerMovement.MoveStriker(vector, transform);
            }
        }


        private void OnControllerRotated(float value)
        {
            if (tutorialData.canIAim)
            {
                strikerMovement.AimStriker(value);
            }
           
        }

        private void OnHandRotated(Vector3 pos)
        {
            if (tutorialData.canIAim)
            {
                strikerMovement.RotateTo(pos);  
            }
        }

        private void Swiped(SwipeDirection direction)
        {
            if (tutorialData.canIPosition)
            {
            
                strikerMovement.MoveStriker(direction);
            }
        }

        private void KeyboardRotated(SwipeDirection direction)
        {
            if (tutorialData.canIAim)
            {
                strikerMovement.AimStriker(direction);
            }
        }

        private void FireStrikerWithForce(float value)
        {
            if (tutorialData.canIFire)
            {
                striker.GetComponent<IStrikerShoot>().FireStriker(value);
            }
        }
    }
}

using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;


namespace com.VisionXR.GameElements
{

    public class TrickShotsInput: MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public InputDataSO inputData;
      

        [Header("Game Objects")]
       public TrickShotManager trickShotManager;

        public void OnEnable()
        {
            

            inputData.FireStrikerWithForceEvent += FireStrikerWithForce;
            inputData.KeyboardRotatedEvent += KeyboardRotated;
            inputData.SwipedEvent += Swiped;
            inputData.SwipedPositionEvent += OnControllerMoved;
            inputData.ControllerRotatedEvent += OnControllerRotated;
            inputData.RotateStrikerTowardsEvent += OnHandRotated;
          
        }

        public void OnDisable()
        {
            inputData.FireStrikerWithForceEvent -= FireStrikerWithForce;
            inputData.KeyboardRotatedEvent -= KeyboardRotated;
            inputData.SwipedEvent -= Swiped;
            inputData.SwipedPositionEvent -= OnControllerMoved;
            inputData.ControllerRotatedEvent -= OnControllerRotated;
            inputData.RotateStrikerTowardsEvent -= OnHandRotated;

        }



        private void OnControllerMoved(Vector3 vector, Transform transform)
        {

            trickShotManager.strikerMovement.MoveStriker(vector, transform);
            
        }


        private void OnControllerRotated(float value)
        {

            trickShotManager.strikerMovement.AimStriker(value);
                  
        }

        private void OnHandRotated(Vector3 pos)
        {
            trickShotManager.strikerMovement.RotateTo(pos);
        }

        private void Swiped(SwipeDirection direction)
        {

            trickShotManager.strikerMovement.MoveStriker(direction);
            
        }

        private void KeyboardRotated(SwipeDirection direction)
        {

            trickShotManager.strikerMovement.AimStriker(direction);
            
        }

        private void FireStrikerWithForce(float value)
        {

            trickShotManager.strikerShooting.FireStriker(value);
           
        }
    }
}

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

        [Header("Local Objects")]
        public Player player;
        public bool canIRotate = false;
      

        private void OnEnable()
        {
            inputData.FireStrikerWithForceEvent += FireStrikerWithForce;
            inputData.KeyboardRotatedEvent += KeyboardRotated;
            inputData.SwipedEvent += Swiped;
            inputData.SwipedPositionEvent += OnControllerMoved;
            inputData.ControllerRotatedEvent += OnControllerRotated;
            inputData.RotateStrikerTowardsEvent += OnHandRotated;
          
            inputData.ThumbStickUpDownSwipedEvent += RotateCoins;
        }

        private void OnDisable()
        {
            inputData.FireStrikerWithForceEvent -= FireStrikerWithForce;
            inputData.KeyboardRotatedEvent -= KeyboardRotated;
            inputData.SwipedEvent -= Swiped;
            inputData.SwipedPositionEvent -= OnControllerMoved;
            inputData.ControllerRotatedEvent -= OnControllerRotated;
            inputData.RotateStrikerTowardsEvent -= OnHandRotated;

            inputData.ThumbStickUpDownSwipedEvent -= RotateCoins;
        }

        private void RotateCoins(float val)
        {
            if(canIRotate)
            {
                coinData.RotateCoins(val);
                player.AllCoinsRotatedEvent?.Invoke(coinData.AllCoinsYRotationValue);
            }
        }

        private void OnControllerMoved(Vector3 vector, Transform transform)
        {
             player.strikerMovement.MoveStriker(vector, transform);
        }
        private void OnControllerRotated(float value)
        {
            player.strikerMovement.AimStriker(value);
        }

        private void OnHandRotated(Vector3 position)
        {
            player.strikerMovement.RotateTo(position);
        }

        private void Swiped(SwipeDirection direction)
        {
            player.strikerMovement.MoveStriker(direction);
        }

        private void KeyboardRotated(SwipeDirection direction)
        {
            player.strikerMovement.AimStriker(direction);
        }

        private void FireStrikerWithForce(float value)  
        {
            player.myStriker.GetComponent<IStrikerShoot>().FireStriker(value);                  
        }

        public void StartRotattion()
        {
            canIRotate = true;
        }

        public void StopRotation()
        {
            canIRotate = false;
        }
    }
}
            
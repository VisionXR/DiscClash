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
        public StrikerDataSO strikerData;
        public AppDataSO appData;

        [Header("Local Objects")]
        public Player player;
        public bool canIRotate = false;



        private void OnEnable()
        {
            inputData.MoveStrikerEvent += MoveStriker;
            inputData.FireStrikeEvent += FireStriker;
            inputData.RotateStrikerAbsoluteEvent += RotateStriker;
            inputData.StrikerForceChangedEvent += StrikerForceChanged;

        }

        private void OnDisable()
        {
            inputData.MoveStrikerEvent -= MoveStriker;
            inputData.FireStrikeEvent -= FireStriker;
            inputData.RotateStrikerAbsoluteEvent -= RotateStriker;
            inputData.StrikerForceChangedEvent -= StrikerForceChanged;
        }


        private void StrikerForceChanged(float obj)
        {
            player.strikerShooting.SetStrikerForce(obj);
        }

        private void RotateStriker(float angle)
        {
            player.strikerMovement.AimStriker(angle);
        }

        private void FireStriker(float val)
        {

            player.strikerShooting.FireStriker(val);
            player.strikerArrow.TurnOffArrow();
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

     

    }
}
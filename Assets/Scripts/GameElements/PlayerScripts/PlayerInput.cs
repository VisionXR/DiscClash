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
            inputData.FireStrikerEvent += FireStrikerWithForce;
            inputData.MoveStrikerEvent += MoveStriker;
            inputData.AimStrikerEvent += AimStriker;
            inputData.SetStrikerForceEvent += SetStrikerForce;       
           
        }

        private void OnDisable()
        {
            inputData.FireStrikerEvent -= FireStrikerWithForce;
            inputData.MoveStrikerEvent += MoveStriker;
            inputData.AimStrikerEvent -= AimStriker;
            inputData.SetStrikerForceEvent -= SetStrikerForce;
            
        }

        private void MoveStriker(float val)
        {
            player.strikerMovement.MoveStriker(val);
        }

        private void SetStrikerForce(float normalisedValue)
        {
            player.strikerShoot.SetStrikerForce(normalisedValue);
        }

        private void AimStriker(Vector3 direction)
        {
            player.strikerMovement.AimStriker(direction);
        }

        private void RotateCoins(float val)
        {
            if(canIRotate)
            {
                coinData.RotateCoins(val);
                player.AllCoinsRotatedEvent?.Invoke(coinData.AllCoinsYRotationValue);
            }
        }

        private void FireStrikerWithForce()  
        {
            player.strikerShoot.FireStriker();
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
            
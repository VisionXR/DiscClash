using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

namespace com.VisionXR.GameElements
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public InputDataSO inputData;
        public CamPositionSO camPositionData;

        [Header(" local Objects")]
        public Player currentPlayer;
       

        void OnEnable()
        {
            inputData.SwipeDetectedEvent += ChangePlayerPosition;
        }

        private void OnDisable()
        {
            inputData.SwipeDetectedEvent -= ChangePlayerPosition;
        }

        private void ChangePlayerPosition(SwipeDirection direction)
        {
            camPositionData.RotateCam(currentPlayer.myId,direction);
        }

     
    }
}

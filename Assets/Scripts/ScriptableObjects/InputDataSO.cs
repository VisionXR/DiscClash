using System;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "InputDataSO", menuName = "ScriptableObjects/InputDataSO", order = 1)]
    public class InputDataSO : ScriptableObject
    {
        // User Data
        public bool isInputEnabled = true;
      

        // Events

        public Action<float> FireStrikeEvent;
        public Action<float> StrikerForceChangedEvent;


        public Action<bool> InputChangeEvent;

        public Action<float> RotateStrikerAbsoluteEvent;
        public Action<float> MoveStrikerEvent;
        public Action<float> RotateCoinsEvent;



        //Methods

        void OnEnable()
        {
            isInputEnabled = false;
        }

        public void EnableInput()
        {
            isInputEnabled = true;
            InputChangeEvent?.Invoke(true);
        }

        public void DisableInput()
        {
            isInputEnabled = false;
            InputChangeEvent?.Invoke(false);
        }


        public void MoveStriker(float val)
        {
            MoveStrikerEvent?.Invoke(val);
        }

        public void RotateStrikerAbsolute(float angle)
        {
            RotateStrikerAbsoluteEvent?.Invoke(angle);
        }

        public void RotateCoins(float angle)
        {
            RotateCoinsEvent?.Invoke(angle);
        }

        public void FireStrike(float power)
        {
            FireStrikeEvent?.Invoke(power);
        }
        public void StrikerForceChanged(float force)
        {
            StrikerForceChangedEvent?.Invoke(force);
        }

    }
}

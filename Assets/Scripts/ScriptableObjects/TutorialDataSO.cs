using System;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "TutorialDataSO", menuName = "ScriptableObjects/TutorialDataSO", order = 1)]
    public class TutorialDataSO : ScriptableObject
    {
        // variables
        public bool canIPosition;
        public bool canIAim;
        public bool canIFire;

        // Events
        public Action CheckPositionEvent;
        public Action CheckAimEvent;
        public Action CheckStrikeEvent;

        // Methods

        public void ResetVariables()
        {
            canIPosition = false;
            canIAim = false;
            canIFire = false;
        }

        public void SetCanIPosition(bool value)
        {
            canIPosition = value;
        }

        public void SetCanIAim(bool value)
        {
            canIAim = value;
        }

        public void SetCanIFire(bool value)
        {
            canIFire = value;
        }

        public void CheckPosition()
        {
            if (canIPosition)
            {
                CheckPositionEvent?.Invoke();
            }
        }

        public void CheckAim()
        {
            if (canIAim)
            {
                CheckAimEvent?.Invoke();
            }
        }

        public void CheckStrike()
        {
            if (canIFire)
            {
                CheckStrikeEvent?.Invoke();
            }
        }
    }
}

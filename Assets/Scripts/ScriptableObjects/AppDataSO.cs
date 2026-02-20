using System;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "AppDataSO", menuName = "ScriptableObjects/AppDataSO", order = 1)]
    public class AppDataSO : ScriptableObject
    {


        [Header(" Colors ")]
        public Color SelectedColor;
        public Color HoverColor;
        public Color IdleColor;
        

        [Header(" Local variables")]
        
        public bool isHapticsOn = true;
        public float vibrationDuration = 0.5f;
        [Range(0f, 1f)]
        public float vibrationAmplitude = 0.1f;
        [Range(0f, 1f)]
        public float vibrationAmplitudeForStriking = 1f;

        // Action
        public Action PlayButtonVibrationEvent;
        public Action PlayStrikerVibrationEvent;




        public void SetHaptics(bool val)
        {
            isHapticsOn |= val;
        }

        public void PlayButtonVibration()
        {
            if(isHapticsOn)
            {
                PlayButtonVibrationEvent?.Invoke();
            }
        }

        public void PlayStrikervibration()
        {
            if(isHapticsOn)
            {
                PlayStrikerVibrationEvent?.Invoke();
            }
        }
    }


}

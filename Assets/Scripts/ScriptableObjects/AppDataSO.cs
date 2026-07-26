using System;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "AppDataSO", menuName = "ScriptableObjects/AppDataSO", order = 1)]
    public class AppDataSO : ScriptableObject
    {


        [Header(" Local variables")]
        public float vibrationDuration = 0.2f;
        public float vibrationAmplitude = 0.2f;
        public float vibrationFrequency = 0.2f;
        public float strikingVibrationFrequency = 0.5f;
        public float strikingVibrationDuration = 0.5f;


        // Actions

        public Action StartVibrationEvent;
        public Action StartStrikingVibrationEvent;

        //Methods

        public void StartVibration()
        {
            StartVibrationEvent?.Invoke();
        }

        public void StartStrikingVibration()
        {
            StartStrikingVibrationEvent?.Invoke();
        }

    }

}

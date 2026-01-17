using System;
using UnityEngine;
namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "CamPositionSO", menuName = "ScriptableObjects/CamPositionSO", order = 1)]
    public class CamPositionSO : ScriptableObject
    {
        
        //actions
        public Action<int> SetCamPositionEvent;
        public Action<int, float> MoveCamUpDownEvent;
        public Action<int,float> MoveCamLeftRightEvent;
        public Action<int, float> RotateCamEvent;

        // methods

        public void SetCamPosition(int id)
        {
            SetCamPositionEvent?.Invoke(id);
        }

        public void MoveCamUpDown(int id,float yValue)
        {
            MoveCamUpDownEvent?.Invoke(id, yValue);
        }

        public void MoveCamLeftRight(int id,float value)
        {
            MoveCamLeftRightEvent?.Invoke(id,value);
        }

        public void RotateCam(int id, float value)
        {
            RotateCamEvent?.Invoke(id, value);
        }
    }
}

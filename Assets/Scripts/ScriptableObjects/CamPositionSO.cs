using System;
using UnityEngine;
namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "CamPositionSO", menuName = "ScriptableObjects/CamPositionSO", order = 1)]
    public class CamPositionSO : ScriptableObject
    {
        
        //actions
        public Action<int> SetCamPositionEvent;
        public Action<int,SwipeDirection> RotateCamEvent;
        public Action<int> RecenterEvent;

        // methods

        public void SetCamPosition(int id)
        {
            SetCamPositionEvent?.Invoke(id);
        }

        public void RotateCam(int id,SwipeDirection direction)
        {
            RotateCamEvent?.Invoke(id,direction);
        }

        public void Recenter(int id)
        {
            RecenterEvent?.Invoke(id);
        }
    }
}

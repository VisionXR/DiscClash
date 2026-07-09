using com.VisionXR.HelperClasses;
using System;
using UnityEngine;
namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "CamPositionSO", menuName = "ScriptableObjects/CamPositionSO", order = 1)]
    public class CamPositionSO : ScriptableObject
    {
        
        //actions
        public Action<int> SetCamPositionFrontViewEvent;
        public Action<int> SetCamPositionTopViewEvent;
        public Action<int,SwipeDirection> RotateCamEvent;
        public Action<int> RecenterEvent;

        // methods

        public void SetCamPositionFrontView(int id)
        {
            SetCamPositionFrontViewEvent?.Invoke(id);
        }

        public void SetCamPositionTopView(int id)
        {
            SetCamPositionTopViewEvent?.Invoke(id);
        }

        public void Recenter(int id)
        {
            RecenterEvent?.Invoke(id);
        }
    }
}

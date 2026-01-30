using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.GameElements
{
    public class StrikerArrow : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public StrikerDataSO strikerData;
        public BoardDataSO boardData;

        [Header(" Local Scripts")]
        public StrikerShooting strikerShooting;

        [Header(" Color ")]
        public Color minColor;
        public Color maxColor;

        [Header(" Game Objects")]
        public float initScale;
        public float maxScale = 3;
        public GameObject AimCanvasObject;
        public Image ArrowHead;
        public Image ArrowLength;

        // cached scales
        private Vector3 _initialCanvasScale = Vector3.one;
        private Vector3 _baseCanvasScale = Vector3.one;

        private void Awake()
        {
          
            _baseCanvasScale = _initialCanvasScale * initScale;
        }

        private void OnEnable()
        {
          
            strikerData.TurnOnStrikerArrowEvent += TurnOnArrow;
            strikerData.TurnOffStrikerArrowEvent += TurnOffArrow;
     
        }

        private void OnDisable()
        {
           
            strikerData.TurnOnStrikerArrowEvent -= TurnOnArrow;
            strikerData.TurnOffStrikerArrowEvent -= TurnOffArrow;      
            TurnOffArrow();
        }

        public void ChangeColorOfArrow(float value)
        {
            if (!AimCanvasObject.activeInHierarchy)
            {
                AimCanvasObject.SetActive(true);
            }

            // clamp input
            value = Mathf.Clamp01(value);

            // set colors
            Color c = Color.Lerp(minColor, maxColor, value);
            if (ArrowHead != null) ArrowHead.color = c;
            if (ArrowLength != null) ArrowLength.color = c;


            float t = (value) / 1f; // normalize from 0.1..1 to 0..1
            float multiplier = 1f + maxScale * t; // 1 -> 4
            AimCanvasObject.transform.localScale = _baseCanvasScale * multiplier;

        }

        public void TurnOnArrow()
        {
            if (AimCanvasObject != null)
            {
                AimCanvasObject.transform.localScale = _baseCanvasScale;
                AimCanvasObject.SetActive(true);
            }
        }

        public void TurnOffArrow()
        {
            if (AimCanvasObject != null)
            {
                AimCanvasObject.transform.localScale = _baseCanvasScale;
                AimCanvasObject.SetActive(false);
            }
        
        }
    }
}

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
        public float maxScale = 4;
        public GameObject AimCanvasObject;
        public Image ArrowHead;
        public Image ArrowLength;

        [Header(" Game Objects")]
        public GameObject displayArrow;
        public Renderer arrowRenderer;


        // cached scales
        private Vector3 _initialCanvasScale = Vector3.one;
        private Vector3 _baseCanvasScale = Vector3.one;

        private void Awake()
        {
          
            _baseCanvasScale = _initialCanvasScale * initScale;
        }

        private void OnDisable()
        {
               
            TurnOffArrow();
        }

        public void ChangeColorOfArrow(float value)
        {
            if (!displayArrow.activeInHierarchy)
            {
                displayArrow.SetActive(true);
            }

            //// clamp input
            //value = Mathf.Clamp01(value);

            //// set colors
            //Color c = Color.Lerp(minColor, maxColor, value);
            //if (ArrowHead != null) ArrowHead.color = c;
            //if (ArrowLength != null) ArrowLength.color = c;


            //float t = (value) / 1f; // normalize from 0.1..1 to 0..1
            //float multiplier = 1f + maxScale * t; // 1 -> 4
            //AimCanvasObject.transform.localScale = _baseCanvasScale * multiplier;

            Debug.Log("Changing arrow threshold to: " + value);

            if (arrowRenderer.material.HasProperty("_Threshold"))
            {
                Debug.Log("threshold : " + value);
                // Set the new threshold value
                arrowRenderer.material.SetFloat("_Threshold", value);
            }
        }

        public void TurnOnArrow()
        {
            //if (AimCanvasObject != null)
            //{
            //    AimCanvasObject.transform.localScale = _baseCanvasScale;
            //    AimCanvasObject.SetActive(true);
            //}

           displayArrow.SetActive(true);
            arrowRenderer.material.SetFloat("_Threshold", 0);
        }

        public void TurnOffArrow()
        {
            //if (AimCanvasObject != null)
            //{
            //    AimCanvasObject.transform.localScale = _baseCanvasScale;
            //    AimCanvasObject.SetActive(false);
            //}

            displayArrow.SetActive(false);
            arrowRenderer.material.SetFloat("_Threshold", 0);
        }
    }
}

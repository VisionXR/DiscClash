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

        [Header(" Game Objects")]
        public GameObject displayArrow;
        public Renderer arrowRenderer;


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

            if (arrowRenderer.material.HasProperty("_Threshold"))
            {
                // Set the new threshold value
                arrowRenderer.material.SetFloat("_Threshold", value);
            }
        }

        public void TurnOnArrow()
        {

           displayArrow.SetActive(true);
            arrowRenderer.material.SetFloat("_Threshold", 0);
        }

        public void TurnOffArrow()
        {
            displayArrow.SetActive(false);
            arrowRenderer.material.SetFloat("_Threshold", 0);
        }
    }
}

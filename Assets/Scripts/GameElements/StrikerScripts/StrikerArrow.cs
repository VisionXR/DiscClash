using com.VisionXR.ModelClasses;
using UnityEngine;

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
        public GameObject arrow;
        public GameObject displayArrow;
        public Renderer arrowRenderer;
     
        private void OnEnable()
        {
            strikerShooting.StrikeForceChangedEvent += ChangeColorOfArrow;
            strikerShooting.StrikeStartedEvent += TurnOffArrow;
            strikerData.TurnOnStrikerArrowEvent += TurnOnArrow;
            strikerData.TurnOffStrikerArrowEvent += TurnOffArrow;
            TurnOnArrow();
        }

        private void OnDisable()
        {
            strikerShooting.StrikeForceChangedEvent -= ChangeColorOfArrow;
            strikerShooting.StrikeStartedEvent -= TurnOffArrow;
            strikerData.TurnOnStrikerArrowEvent -= TurnOnArrow;
            strikerData.TurnOffStrikerArrowEvent -= TurnOffArrow;
            TurnOffArrow();
        }



        private void ChangeColorOfArrow(float value)
        {

            if (arrowRenderer.material.HasProperty("_Threshold"))
            {
                // Set the new threshold value
                arrowRenderer.material.SetFloat("_Threshold", value);
            }
        }

        public void SetStrikerMaterial(Material m)
        {
          //  strikerRenderer.material = m;
        }

        public void TurnOffArrow()
        {
            arrowRenderer.material.SetFloat("_Threshold", 0);
            arrow.SetActive(false);
            displayArrow.SetActive(false);
          
    
        }
        public void TurnOffArrow(float a,Vector3 dir)
        {
            arrowRenderer.material.SetFloat("_Threshold", 0);
            arrow.SetActive(false);
            displayArrow.SetActive(false);


        }
        public void TurnOnArrow(float a,Vector3 dir)
        {
            arrow.SetActive(true);
            displayArrow.SetActive(true);
           
        }
        public void TurnOnArrow()
        {
            arrow.SetActive(true);
            displayArrow.SetActive(true);

        }

    }
}

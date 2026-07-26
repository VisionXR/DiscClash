using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class StrikerSelectionPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public UIDataSO uiData;


        [Header("Local Objects")]
        public List<GameObject> strikerSelectedImages;
        public string currentState;


        private void OnEnable()
        {
            ResetBoardImages();
            strikerSelectedImages[uiOutputData.MyStrikerId].SetActive(true);
            
        }

        public void StrikerBtnClicked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetBoardImages();
            strikerSelectedImages[id].SetActive(true);
            uiOutputData.SetMyStrikerId(id);
        }

        public void BackBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(currentState, false);
        }

        private void ResetBoardImages()
        {
            foreach (var item in strikerSelectedImages)
            {
                item.SetActive(false);
            }

        }

    }
}


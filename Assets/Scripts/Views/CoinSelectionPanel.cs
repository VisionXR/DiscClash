using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class CoinSelectionPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public UIDataSO uiData;


        [Header("Local Objects")]
        public List<GameObject> coinSelectedImages;
        public string currentState;


        private void OnEnable()
        {
            ResetBoardImages();
            coinSelectedImages[uiOutputData.MyCoinsId].SetActive(true);
            
        }

        public void CoinBtnClicked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetBoardImages();
            coinSelectedImages[id].SetActive(true);
            uiOutputData.SetMyCoinsId(id);
        }

        public void BackBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(currentState, false);
        }

        private void ResetBoardImages()
        {
            foreach (var item in coinSelectedImages)
            {
                item.SetActive(false);
            }

        }

    }
}


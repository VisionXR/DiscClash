using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class BoardSelectionPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public UIDataSO uiData;


        [Header("Local Objects")]
        public List<GameObject> boardSelectedImages;
        public string currentState;


        private void OnEnable()
        {
            ResetBoardImages();
            boardSelectedImages[uiOutputData.MyBoardId].SetActive(true);
            
        }

        public void BoardBtnClicked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetBoardImages();
            boardSelectedImages[id].SetActive(true);
            uiOutputData.SetMyBoardId(id);
        }

        public void BackBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(currentState, false);
        }

        private void ResetBoardImages()
        {
            foreach (var item in boardSelectedImages)
            {
                item.SetActive(false);
            }

        }

    }
}


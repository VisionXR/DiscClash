using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class MatchPanelView : MonoBehaviour
    {
        [Header(" Scriptable Objects ")]
        public UIInputDataSO uiInputData;
        public UIOutputDataSO uiOutputData;
        public UIDataSO uiData;
        public MyPlayerSettings playerSettings;
        public CamPositionSO camPosition;

        [Header(" State variables ")]
        public string matchState;
        public string vsCpuState;
        public string vsFriendsState;

        [Header(" Local variables ")]
        public List<GameObject> matchSelectionImages;


        private void OnEnable()
        {
            ResetImages();
            if(uiOutputData.matchType == MatchType.Single)
            {
                matchSelectionImages[0].SetActive(true);
            }
            else
            {
                matchSelectionImages[1].SetActive(true);
            }
        }

        public void SingleMatchBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.SetMatchType(MatchType.Single);
            ResetImages();
            matchSelectionImages[0].SetActive(true);

        }

        public void TournamentMatchBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.SetMatchType(MatchType.Tournament);
            ResetImages();
            matchSelectionImages[1].SetActive(true);

        }

        public void NextBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            if (uiOutputData.gameType == GameType.VsCPU)
            {
                uiData.uiManager.ChangeState(vsCpuState, true);
            }
            else if (uiOutputData.gameType == GameType.PlayWithFriends)
            {
                uiData.uiManager.ChangeState(vsFriendsState, true);
            }

        }

        public void BackBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(matchState, false);
        }


        private void ResetImages()
        {
            foreach (GameObject go in matchSelectionImages)
            {
                go.SetActive(false);
            }
        }
    }
}

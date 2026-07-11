using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class HomePanelView : MonoBehaviour
    {

        [Header(" Scriptable Objects ")]
        public UIInputDataSO uiInputData;
        public UIOutputDataSO uiOutputData;
        public UIDataSO uiData;
        public MyPlayerSettings playerSettings;

        [Header(" Selection Objects ")]
        public List<GameObject> gameModeSelectionImages;

        [Header(" State variables ")]
        public string vsCpuState;
        public string vsFriendsState;
        public string tutorialState;
        public string settingsState;
        public string leaderBoardState;
        public string rulesState;
        public string infoState;
        public string purchaseState;
        public string achievementsState;

        private void OnEnable()
        {
            ResetImages();
            if(uiOutputData.gameType == GameType.VsCPU)
            {
                gameModeSelectionImages[0].SetActive(true);
            }
            else if (uiOutputData.gameType == GameType.PlayWithFriends || uiOutputData.gameType == GameType.OnlineMultiPlayer)
            {
                gameModeSelectionImages[1].SetActive(true);
            }
            else if (uiOutputData.gameType == GameType.Tutorial)
            {
                gameModeSelectionImages[2].SetActive(true);
            }

        }

        public void SettingsButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(settingsState, true);
        }

        public void LeaderBoardButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(leaderBoardState, true);
        }
        public void RulesButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(rulesState, true);
        }

        public void InfoButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(infoState, true);
             
        }

        public void PurchaseBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(purchaseState, true);
        }

        public void AchievementsBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(achievementsState, true);
      
        }

        public void VsCPUBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.SetGameType(GameType.VsCPU);
            ResetImages();
            gameModeSelectionImages[0].SetActive(true);
            
        }

        public void VsFriendsBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();          
            uiOutputData.SetGameType(GameType.PlayWithFriends);
            ResetImages();
            gameModeSelectionImages[1].SetActive(true);
        }

        public void TutorialBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.SetGameType(GameType.Tutorial);
            ResetImages();
            gameModeSelectionImages[2].SetActive(true);
        }

        public void NextBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            if(uiOutputData.gameType == GameType.VsCPU)
            {
                uiData.uiManager.ChangeState(vsCpuState, true);
            }
            else if (uiOutputData.gameType == GameType.PlayWithFriends)
            {
                uiData.uiManager.ChangeState(vsFriendsState, true);
            }
            else if (uiOutputData.gameType == GameType.Tutorial)
            {
                uiData.uiManager.ChangeState(tutorialState, true);
            }
        }


        private void ResetImages()
        {
            foreach(GameObject go in gameModeSelectionImages)
            {
                go.SetActive(false);
            }
        }

    }
}
    
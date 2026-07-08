using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
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
            uiData.uiManager.ChangeState(vsCpuState, true);
        }

        public void VsFriendsBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.SetGameType(GameType.PlayWithFriends);
            uiData.uiManager.ChangeState(vsFriendsState, true);
        }

        public void TutorialBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(tutorialState, true);
        }


    }
}
    
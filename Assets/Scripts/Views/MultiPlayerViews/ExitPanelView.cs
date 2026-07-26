using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class ExitPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIInputDataSO uIInputData;
        public GameDataSO gameData;
        public UIDataSO uiData;
        public ADDataSO adData;

        [Header("Next And Previous Panels")]
        public string currentState;
        public string settingsState;
        public string rulesState;

        public void ExitBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            uiData.uiManager.ChangeState("SinglePlayer", false);
            uiData.uiManager.ChangeState("MultiPlayer", false);
            uiData.uiManager.ChangeState("Home", true);
            uiData.uiManager.ResetAllBools();

            uIInputData.ExitGame();
            adData.ShowInterstitialAd();

        }

        public void ResumeBtnClicked()
        {

            AudioManager.instance.PlayButtonClickSound();

            uiData.uiManager.ChangeState(currentState, false);
            uIInputData.ResumeGame();
         
        }

        public void SettingsBtnClicked()
        {

            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState("Settings", true);
        }

        public void RulesBtnClicked()
        {

            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState("Rules", true);
        }


    }
}

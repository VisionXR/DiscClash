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


        [Header("Next And Previous Panels")]
        public GameObject InputCanvas;
        public string currentState;

        public void ExitBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            uiData.uiManager.ChangeState("SinglePlayer", false);
            uiData.uiManager.ChangeState("MultiPlayer", false);
            uiData.uiManager.ChangeState("Home", true);
            uiData.uiManager.ResetAllBools();

            uIInputData.ExitGame();
          

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

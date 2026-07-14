using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class ExitPopupPanelView : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public UIInputDataSO uiInputData;
        public UIDataSO uiData;


        public void ExitLobbyBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiInputData.GoToHome();
            uiData.uiManager.ChangeState("SinglePlayer", false);
            uiData.uiManager.ChangeState("MultiPlayer", false);
            uiData.uiManager.ChangeState("Tutorial", false);
            uiData.uiManager.GoToState(StateName.HomeState);
            uiData.uiManager.ResetAllBools();
        }

        public void ResumeButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.GoToState(uiData.uiManager.previousStateName);
        }

        public void QuitApp()
        {
            AudioManager.instance.PlayButtonClickSound();
            Application.Quit();
        }
   
    }
}

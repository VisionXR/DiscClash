using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class ExitPopupPanelView : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public UIInputDataSO uiInputData;
        public UIDataSO uiData;

        [Header(" States ")]
        public string exitLobbyState;
  

        private void OnEnable()
        {
            AudioManager.instance.PlayPopUpSound();
        }
        public void OnYesButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiInputData.ExitGame();
            uiData.uiManager.ChangeState("SinglePlayer", false);
            uiData.uiManager.ChangeState("MultiPlayer", false);
            uiData.uiManager.ChangeState("Home", true);
            uiData.uiManager.ResetAllBools();

       
        }

        public void OnNoButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(exitLobbyState, false);
        }

        public void QuitApp()
        {
            AudioManager.instance.PlayButtonClickSound();
            Application.Quit();
        }
   
    }
}

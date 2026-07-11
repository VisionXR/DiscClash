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

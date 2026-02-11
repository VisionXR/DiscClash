using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class ExitPopupPanelView : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public UIInputDataSO uiInputData;

        private void OnEnable()
        {
            AudioManager.instance.PlayPopUpSound();
        }
        public void OnYesButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiInputData.ExitGame();
         
            gameObject.SetActive(false);
        }

        public void OnNoButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            gameObject.SetActive(false);
        }

        public void QuitAoo()
        {
            AudioManager.instance.PlayButtonClickSound();
            Application.Quit();
        }
   
    }
}

using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class ExitPopupPanelView : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public UIOutputDataSO uiOutputData;

        private void OnEnable()
        {
            AudioManager.instance.PlayPopUpSound();
        }
        public void OnYesButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.ExitGame();
         
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

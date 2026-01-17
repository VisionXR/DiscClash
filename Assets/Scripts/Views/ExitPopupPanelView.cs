using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class ExitPopupPanelView : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public UIOutputDataSO uiOutputData;

        [Header(" Canvas Objects")]
        public GameObject CenterCanvas;

        private void OnEnable()
        {
            AudioManager.instance.PlayPopUpSound();
        }
        public void OnYesButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.ExitGame();
            CenterCanvas.SetActive(false);
            gameObject.SetActive(false);
        }

        public void OnNoButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            CenterCanvas.SetActive(false);
            gameObject.SetActive(false);
        }
   
    }
}

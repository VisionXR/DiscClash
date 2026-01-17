using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class TrickShotExitPanel : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public TrickShotsDataSO trickShotData;


        [Header(" Local Objects")]
        public InGamePanel inGamePanel;

        private void OnEnable()
        {
            AudioManager.instance.PlayPopUpSound();
        }
        public void OnYesButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.StopTrickShotsEvent?.Invoke();
            uiOutputData.ExitGame();  
            gameObject.SetActive(false);
        }

        public void OnNoButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();         
            gameObject.SetActive(false);
            inGamePanel.ResumeBtnClicked();
        }
   
    }
}

using UnityEngine;
using com.VisionXR.ModelClasses;

namespace com.VisionXR.Views
{
    public class PausePanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public InputDataSO inputData;

        public void ResumeGame()
        {
            AudioManager.instance.PlayButtonClickSound();
            inputData.PauseButtonClicked();
            gameObject.SetActive(false);

        }

        public void OnHomeButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            inputData.ResumeGameEvent();
            inputData.DeactivateInput();
            uiOutputData.ExitGame();
            uiOutputData.GoToHome();           
            gameObject.SetActive(false);
        }
    }
}

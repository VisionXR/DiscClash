using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class ChangeDestination : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;

        [Header(" Canvas Objects")]
        public GameObject CenterCanvas;
        public Destination currentDestination;
        private void OnEnable()
        {
            AudioManager.instance.PlayPopUpSound();
        }

        public void SetDestination(Destination d)
        {
            currentDestination = d;
        }

        public void OnYesButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.ExitGame();
            uiInputData.ConnectToDestination(currentDestination);

        }
        public void OnNoButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            CenterCanvas.SetActive(false);
            gameObject.SetActive(false);
        }
   
    }
}

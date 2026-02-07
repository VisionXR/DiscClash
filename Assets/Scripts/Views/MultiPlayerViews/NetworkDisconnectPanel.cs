using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class NetworkDisconnectPanel : MonoBehaviour
    {
        public TMP_Text reason;
        public UIOutputDataSO uiOutputData;
       
        private void OnEnable()
        {
            AudioManager.instance.PlayPopUpSound();
        }
        public void OnHomeButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.GoToHome();
            gameObject.SetActive(false);
        }

        public void OnContinueButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
           
            gameObject.SetActive(false);
        }

        public void SetReason(string msg)
        {
            reason.text = msg;
        }


    }
}

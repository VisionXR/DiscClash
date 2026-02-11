using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class NetworkDisconnectPanel : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public UIInputDataSO uiInputData;

        [Header(" UI Elements")]
        public TMP_Text reason;
      
       
        private void OnEnable()
        {
            AudioManager.instance.PlayPopUpSound();
        }
        public void OnHomeButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiInputData.GoToHome();
            gameObject.SetActive(false);
        }
        public void SetReason(string msg)
        {
            reason.text = msg;
        }

    }
}

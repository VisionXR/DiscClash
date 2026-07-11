using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class NetworkDisconnectPanel : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public UIInputDataSO uiInputData;
        public UIDataSO uiData;

        [Header(" UI Elements")]
        public string  homeState;
      
       
        private void OnEnable()
        {
            AudioManager.instance.PlayPopUpSound();
        }
        public void OnHomeButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiInputData.GoToHome();
            uiData.uiManager.ChangeState(homeState, true);
        }


    }
}

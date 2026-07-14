using com.VisionXR.HelperClasses;
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
            uiData.uiManager.ChangeState("SinglePlayer", false);
            uiData.uiManager.ChangeState("MultiPlayer", false);
            uiData.uiManager.ChangeState("Tutorial", false);
            uiData.uiManager.GoToState(StateName.HomeState);
            uiData.uiManager.ResetAllBools();

        }


    }
}

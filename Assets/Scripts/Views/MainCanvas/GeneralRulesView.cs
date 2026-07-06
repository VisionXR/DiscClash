using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class GeneralRulesView : MonoBehaviour
    {
        [Header(" Game Objects")]
 
        public AppDataSO appData;
        public UIDataSO uiData;

        [Header("Panels")]
        
        public string rulesState;


        public void BackButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(rulesState, false);
        }

    }
}

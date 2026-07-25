using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class AchievementPanelView : MonoBehaviour
    {
        public UIDataSO uiData;
        public string currentState;


        public void BackBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(currentState, false);
        }
    }
}

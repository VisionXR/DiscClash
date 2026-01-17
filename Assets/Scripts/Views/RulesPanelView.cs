using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class RulesPanelView : MonoBehaviour
    {
        public UIOutputDataSO uiOutputData;
        public GameObject BlackAndWhiteRules, FreeStyleRules;

        private void OnEnable()
        {
            RulesButtonClicked();
        }
        public void RulesButtonClicked()
        {

            if (uiOutputData.game == Game.BlackAndWhite)
            {
                BlackAndWhiteRules.SetActive(true);
                FreeStyleRules.SetActive(false);
            }
            else
            {
                BlackAndWhiteRules.SetActive(false);
                FreeStyleRules.SetActive(true);
            }
        }

    }
}

   


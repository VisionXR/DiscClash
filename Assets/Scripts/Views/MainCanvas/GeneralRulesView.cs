using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class GeneralRulesView : MonoBehaviour
    {
        [Header(" Game Objects")]
 
        public AppDataSO appData;
        public UIDataSO uiData;

        [Header("Panels")]
        public List<GameObject> tabObjects;
        public List<GameObject> tabSelectionImages;
        public string rulesState;



        public void TabButtonClicked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            Reset();
            tabObjects[id].SetActive(true);
            tabSelectionImages[id].SetActive(true);

        }

        public void BackButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(rulesState, false);
        }

        private void Reset()
        {
            foreach (var tab in tabObjects)
            {
                tab.SetActive(false);
            }

            foreach (var tab in tabSelectionImages)
            {
                tab.SetActive(false);
            }

        }

    }
}

using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class GeneralRulesView : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public AppDataSO appData;
        public UIDataSO uiData;
        public ScrollRect rulesScrollRect;

        [Header("Panels")]
        public List<GameObject> tabObjects;
        public List<GameObject> tabSelectionImages;
        public string rulesState;


        private void OnEnable()
        {
            StartCoroutine(ResetScroll());
        }

        private IEnumerator ResetScroll()
        {
            yield return new WaitForSeconds(0.5f);
            rulesScrollRect.verticalNormalizedPosition = 1f;
        }

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

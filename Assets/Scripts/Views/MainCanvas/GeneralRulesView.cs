using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class GeneralRulesView : MonoBehaviour
    {
        [Header(" Game Objects")]
        public GameObject GeneralRulesPanel;
        public GameObject GameRulesPanel;

        [Header("Panels")]
        public GameObject MainPanel;
        public GameObject HomePanel;

        [Header("Selection Images")]
        [SerializeField] private Image GameRulesSelectedImage;
        [SerializeField] private Image GeneralRulesSelectedImage;

        private void OnEnable()
        {
            GameRulesPanel.SetActive(true);
            GeneralRulesPanel.SetActive(false);
            ResetImages();
            GameRulesSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            GameRulesSelectedImage.color = Color.white;
        }
        public void OnGameRulesButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            GameRulesPanel.SetActive(true);
            GeneralRulesPanel.SetActive(false);
            ResetImages();
            GameRulesSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            GameRulesSelectedImage.color = Color.white;
        }

        public void OnGeneralRulesButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            GeneralRulesPanel.SetActive(true);
            GameRulesPanel.SetActive(false);
            ResetImages();
            GeneralRulesSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            GeneralRulesSelectedImage.color = Color.white;
        }

        void ResetImages()
        {
            GameRulesSelectedImage.color = AppProperties.instance.IdleColor;
            GeneralRulesSelectedImage.color = AppProperties.instance.IdleColor;

            GameRulesSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            GeneralRulesSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
        }

        public void BackButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            MainPanel.SetActive(true);
            HomePanel.SetActive(true);
            gameObject.SetActive(false);
        }

    }
}

using UnityEngine;

namespace com.VisionXR.Views
{
    public class MainSettingsPanelView : MonoBehaviour
    {
 

        [Header(" Panels ")]
        [SerializeField] private GameObject MainPanel;
        [SerializeField] private GameObject SettingsPanel;
        [SerializeField] private GameObject RulesPanel;
        [SerializeField] private GameObject AboutUsPanel;
        [SerializeField] private GameObject LeaderBoardPanel;
        [SerializeField] private GameObject PurchasePanel;
        [SerializeField] private GameObject AchievementsPanel;

    
        public void HomeButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetPanels();
            MainPanel.SetActive(true);
        }

        public void SettingsButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetPanels();
            SettingsPanel.SetActive(true);
        }

        public void LeaderBoardButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetPanels();
            LeaderBoardPanel.SetActive(true);
        }
        public void RulesButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetPanels();
            RulesPanel.SetActive(true);
        }

        public void InfoButtonClicked()
        {
             AudioManager.instance.PlayButtonClickSound();
             ResetPanels();
             AboutUsPanel.SetActive(true);
        }

        public void PurchaseBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetPanels();
            PurchasePanel.SetActive(true);
            PurchasePanel.GetComponent<PurchasePanel>().Initialise(0);
        }

        public void AchievementsBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetPanels();
            AchievementsPanel.SetActive(true);
        }

       
        private void ResetPanels()
        {
            MainPanel.SetActive(false);
            SettingsPanel.SetActive(false);
            RulesPanel.SetActive(false);
            AboutUsPanel.SetActive(false);
            LeaderBoardPanel.SetActive(false);
            RulesPanel.SetActive(false);
            PurchasePanel.SetActive(false);
            AchievementsPanel.SetActive(false);

        }
    }
}
    
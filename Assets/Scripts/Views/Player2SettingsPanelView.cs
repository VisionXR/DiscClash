using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class Player2SettingsPanelView : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public UIOutputDataSO uIOutputData;

        [Header("UI Objects")]
        public Sprite SpeakerOnSprite;
        public Sprite SpeakerOffSprite;
        public Image SpeakerImage;

        [Header("Panel Objects")]
        public GameObject MainCanvas;
        public GameObject SettingsPanel;
        public GameObject AchievementsPanel;

        // local variables
        private bool isSpeakerOn = true;
        private bool isSettingsOn = false;
       
        

        private void OnDisable()
        {
            isSettingsOn = false;
            SettingsPanel.SetActive(false);
         

        }
        public void OnSpeakerButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            if (isSpeakerOn)
            {
                isSpeakerOn = false;
                SpeakerImage.sprite = SpeakerOffSprite;
                uIOutputData.TurnOffSpeaker();

            }
            else
            {

                isSpeakerOn = true;
                SpeakerImage.sprite = SpeakerOnSprite;
                uIOutputData.TurnOnSpeaker();
            }
        }
    

        public void OnSettingsButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            isSettingsOn = !isSettingsOn;
            SettingsPanel.SetActive(isSettingsOn);
          
        } 

        public void AchievementsBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();           
            MainCanvas.SetActive(true);
            AchievementsPanel.SetActive(true);
        }
    }
}

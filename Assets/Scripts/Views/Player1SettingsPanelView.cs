using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class Player1SettingsPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public PlayersDataSO playersData;
        public UIOutputDataSO uiOutputData;

        [Header("UI Objects")]
        public Image MicImage;
        public Sprite MicOnSprite;
        public Sprite MicOffSprite;


        [Header("Panel Objects")]
        public GameObject CenterCanvas;
        public GameObject MainCanvas;
        public GameObject ExitPanel;
        public GameObject PurchasePanel;
        

        // local variables
        private bool isMicOn = true;
        


        public void OnExitButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            CenterCanvas.SetActive(true);
            ExitPanel.SetActive(true);
        }

        public void OnMicButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            if (isMicOn)
            {
                isMicOn = false;
                MicImage.sprite = MicOffSprite;
                uiOutputData.TurnOffMic();

            }
            else
            {
                isMicOn = true;
                MicImage.sprite = MicOnSprite;
                uiOutputData.TurnOnMic();

            }

        }

        public void PuchaseBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            
            MainCanvas.SetActive(true);
            PurchasePanel.SetActive(true);
        }
    }
}

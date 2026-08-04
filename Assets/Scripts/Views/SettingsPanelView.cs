using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class SettingsPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public MyPlayerSettings userData;
        public UIDataSO uiData;
        public AchievementsDataSO achievementsData;

        [Header("Tab Objects")]
        public List<GameObject> SelectionImages;
        public List<GameObject> TabPanels;


        [Header("Force Selection Images")]
        public GameObject LeftSideSelectedImage;
        public GameObject RightSideSelectedImage;


        [Header("Audio Objects")]
        public PanelOnOff deleteAccountPanel;
        public Toggle hapticsToggle;
        public Slider bgSlider;
        public AudioSource BGAudioSource;


        [Header("Panel Objects")]
        public string currentState;

        private void OnEnable()
        {

            if (userData.myDominantHand == DominantHand.Right)
            {
                LeftSideSelectedImage.SetActive(false);
                RightSideSelectedImage.SetActive(true);
            }
            else
            {
                LeftSideSelectedImage.SetActive(true);
                RightSideSelectedImage.SetActive(false);
            }

            bgSlider.value = BGAudioSource.volume;

            hapticsToggle.isOn = userData.isHapticsEnabled;
        }

        public void TabButtonClicked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetTabs();
            TabPanels[id].SetActive(true);
            SelectionImages[id].SetActive(true);

        }

        private void ResetTabs()
        {
            foreach (var tab in TabPanels)
            {
                tab.SetActive(false);
            }

            foreach (var img in SelectionImages)
            {
                img.SetActive(false);
            }
        }



        public void RightBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            RightSideSelectedImage.SetActive(true);
            LeftSideSelectedImage.SetActive(false);
            userData.SetDominantHand(DominantHand.Right);
            userData.SaveSettings();
        }


        public void LeftBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            RightSideSelectedImage.SetActive(false);
            LeftSideSelectedImage.SetActive(true);
            userData.SetDominantHand(DominantHand.Left);
            userData.SaveSettings();
        }

        public void DeleteAccountBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            deleteAccountPanel.TurnOnPanel();
        }

        public void Logout()
        {
            AudioManager.instance.PlayButtonClickSound();
            PlayerPrefs.DeleteKey("Login");
            PlayerPrefs.Save();
        
            uiData.uiManager.ChangeState("Login", true);
            uiData.uiManager.GoToState(StateName.LoginState);
            uiData.uiManager.ChangeState(currentState, false);
            uiData.uiManager.ChangeState("Home", false);
        }

        public void BGMusicChanged(float val)
        {
            BGAudioSource.volume = val;
        }

        public void HapticsChanged()
        {
            AudioManager.instance.PlayButtonClickSound();
            userData.SetHapticsEnabled(hapticsToggle.isOn);
            userData.SaveSettings();
        }


        public void BackBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            userData.SaveSettings();
            uiData.uiManager.ChangeState(currentState, false);
        }

    }

}
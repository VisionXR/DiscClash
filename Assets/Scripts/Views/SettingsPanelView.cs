using TMPro;
using UnityEngine;
using UnityEngine.UI;
using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;


namespace com.VisionXR.Views
{

    public class SettingsPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public MyPlayerSettings myPlayerSettings;
        public UIDataSO uiData;

        [Header("Panels")]
        public GameObject ProfilePanel;
        public GameObject MainPanel;


        [Space(5)]
        [Header("Local Variables")]
        public Image profileImage;
        public Slider musicSlider;
        public TMP_InputField playerNameIf;
        public string settingsState;



        public void OnEquipmentButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ProfilePanel.SetActive(false);
            ResetImages();
       
        }

        void ResetImages()
        {
           
        }

        public void OnNameChanged(string newName)
        {
            myPlayerSettings.MyName = newName;
        }

        public void OnVolumeChanged(float volume)
        {
            AudioManager.instance.SetBackGroundVolume(musicSlider.value);
        }

      
        public void OnValueChanged(string output)
        {
            myPlayerSettings.MyName = playerNameIf.text;
        }

 
        public void BackButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(settingsState, false);
        }

        public void OnProfileImageClicked()
        {
            if (!Application.isEditor)
            {
              //  AvatarEditorDeeplink.LaunchAvatarEditor();
            }
        }
    }
}

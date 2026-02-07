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
   

        [Header("Panels")]
        [SerializeField] private GameObject ProfilePanel;
        [SerializeField] private GameObject MainPanel;
        [SerializeField] private GameObject HomePanel;

        [Space(5)]
        [Header("Selection Images")]
        [SerializeField] private Image LeftHandSelectedImage;
        [SerializeField] private Image RightHandSelectedImage;

        [Space(5)]
        [Header("Local Variables")]
        [SerializeField] private Image profileImage;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider passThroughSlider;
        [SerializeField] private TMP_InputField playerNameIf;
        

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            profileImage.sprite = myPlayerSettings.MyProfileImage;
            ProfilePanel.SetActive(true);
            ResetImages();
            playerNameIf.text = myPlayerSettings.MyName;
            ResetHandImages();

        }

        private void ResetHandImages()
        {
            RightHandSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            RightHandSelectedImage.color = AppProperties.instance.IdleColor;
            LeftHandSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            LeftHandSelectedImage.color = AppProperties.instance.IdleColor;
        }

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
            HomePanel.SetActive(true);
            MainPanel.SetActive(true);
            gameObject.SetActive(false);
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

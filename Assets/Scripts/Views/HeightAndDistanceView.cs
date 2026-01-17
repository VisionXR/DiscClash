using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class HeightAndDistanceView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public MyPlayerSettings playerSettings;
        public InputDataSO inputData;

        [Header("Selection Images")]
        [SerializeField] private Image LeftHandSelectedImage;
        [SerializeField] private Image RightHandSelectedImage;

        [Header("Local Objects")]
        public GameObject RightControllerSettings;
        public GameObject RightHandSettings;
        public GameObject LeftControllerSettings;
        public GameObject LeftHandSettings;
        public Slider musicSlider;
        public Slider passThroughSlider;



        private void OnEnable()
        {
            ResetSettings();
            if (playerSettings.myDominantHand == HelperClasses.DominantHand.RIGHT && !inputData.isHandTrackingActive)
            {
                RightControllerSettings.SetActive(true);
            }
            else if (playerSettings.myDominantHand == HelperClasses.DominantHand.LEFT && !inputData.isHandTrackingActive)
            {
               LeftControllerSettings.SetActive(true);
            }
            else if (playerSettings.myDominantHand == HelperClasses.DominantHand.RIGHT && inputData.isHandTrackingActive)
            {
                RightHandSettings.SetActive(true);
            }
            else if (playerSettings.myDominantHand == HelperClasses.DominantHand.LEFT && inputData.isHandTrackingActive)
            {
                LeftHandSettings.SetActive(true);
            }

            if (playerSettings.isPassThrough)
            {
                passThroughSlider.value = 1;
            }
            else
            {
                passThroughSlider.value = 0;
            }

            ResetHandImages();
            if (playerSettings.myDominantHand == DominantHand.RIGHT)
            {
                RightHandSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                RightHandSelectedImage.color = Color.white;
            }
            else
            {
                LeftHandSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                LeftHandSelectedImage.color = Color.white;
            }
        }

        private void ResetSettings()
        {
            RightControllerSettings.SetActive(false);
            LeftControllerSettings.SetActive(false);
            LeftHandSettings.SetActive(false);
            RightHandSettings.SetActive(false);
        }

        public void OnMusicSliderChanged()
        {
           
           AudioManager.instance.SetBackGroundVolume(musicSlider.value);
            
        }

        public void OnPassThroughChanged()
        {
            float value = passThroughSlider.value;
            if (value == 0)
            {
                playerSettings.SetPassThrough(false);
            }
            else
            {
                playerSettings.SetPassThrough(true);
            }
            playerSettings.SaveSettings();
        }

        public void OnRightHandClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetHandImages();
            playerSettings.SetDominantHand(DominantHand.RIGHT);
            RightHandSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            RightHandSelectedImage.color = Color.white;
            playerSettings.SaveSettings();
        }

        public void OnLeftHandClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetHandImages();
            playerSettings.SetDominantHand(DominantHand.LEFT);
            LeftHandSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            LeftHandSelectedImage.color = Color.white;
            playerSettings.SaveSettings();
        }

        private void ResetHandImages()
        {
            RightHandSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            RightHandSelectedImage.color = AppProperties.instance.IdleColor;
            LeftHandSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            LeftHandSelectedImage.color = AppProperties.instance.IdleColor;
        }


    }
}

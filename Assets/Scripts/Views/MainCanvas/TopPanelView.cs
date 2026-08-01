using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class TopPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public MyPlayerSettings playerSettings;
        public CloudDataSO cloudData;


        [Header(" Player UI ")]
        public Image playerImage;
        public TMP_Text playerName;
        public TMP_Text playerCoins;

        private void OnEnable()
        {
            playerImage.sprite = playerSettings.MyProfileImage;
            playerName.text = playerSettings.MyName;
            
        }
    }
}

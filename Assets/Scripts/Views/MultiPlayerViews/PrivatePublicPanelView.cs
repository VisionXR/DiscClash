using com.VisionXR.ModelClasses;
using UnityEngine;
using com.VisionXR.HelperClasses;

namespace com.VisionXR.Views
{
    public class PrivatePublicPanelView : MonoBehaviour
    {
        [Header("Scriptable Object")]
      
        public MyPlayerSettings playerSettings;
        public UIOutputDataSO uiOutputData;

        [Header("Panel Object")]
        public GameObject PlayWithFriendsPanel;
        public GameObject PlayWithStrangersPanel;
        public GameObject MainPanel;
        public GameObject MainSettingsPanel;


        public void PlayWithFriendsBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.SetRoomType(RoomType.Private);
   
            PlayWithFriendsPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void PlayWithStarngersClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.SetRoomType(RoomType.Public);
  
            PlayWithStrangersPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnBackButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            MainPanel.SetActive(true);
            MainSettingsPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class MainPanelView : MonoBehaviour
    {

        [Header(" Scriptable Objects ")]
        public UIOutputDataSO uiOutputData;
        public MyPlayerSettings myPlayerSettings;
        public NetworkOutputSO networkOutputData;
        public DestinationDataSO destinationData;
        public MyPlayerSettings playerSettings;
        public CloudDataSO cloudData;


        [Header(" Panels ")]
        public GameObject SinglePlayerPanel;
        public GameObject PlayWithFriendsPanel;
        public GameObject PlayWithStrangersPanel;
        public GameObject InternetToast;
        public GameObject VsCPUPanel;

        [Header(" Player UI ")]
        public Image playerImage;
        public TMP_Text playerName;
        public TMP_Text playerCoins;


        private void OnEnable()
        {
            playerImage.sprite = myPlayerSettings.MyProfileImage;
            playerName.text = myPlayerSettings.MyName;
            playerCoins.text = cloudData.coins.ToString();
        }

        public void PracticeOfflineBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            SinglePlayerPanel.SetActive(true);
            uiOutputData.SetGameType(GameType.PracticeOffline);
            
            gameObject.SetActive(false);
        }

        public void VsCPUBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            VsCPUPanel.SetActive(true);
            uiOutputData.SetGameType(GameType.VsCPU);

            gameObject.SetActive(false);
        }

        public void OnOnlineMultiPlayerClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            if(Application.internetReachability == NetworkReachability.NotReachable)
            {
                DisplayToast();
                return;
            }

            uiOutputData.SetGameType(GameType.OnlineMultiPlayer);
            uiOutputData.SetRoomType(RoomType.Public);
            PlayWithStrangersPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnPlayWithFriendsClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                DisplayToast();
                return;
            }

            uiOutputData.SetGameType(GameType.PlayWithFriends);
            uiOutputData.SetRoomType(RoomType.Private);

            PlayWithFriendsPanel.SetActive(true);
        
            gameObject.SetActive(false);
        }
        public void OnTutorialClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.SetGameType(GameType.Tutorial);
            Destination d = new Destination();
            d.gameType = GameType.Tutorial;
            d.roomName = "NA";
            d.lobbyName = "NA";

         //   destinationData.ConnectToDestination(d, OnConnectionSuccess, OnConnectionFailure);          
         //   gameObject.SetActive(false);
        }


   
        private void DisplayToast()
        {
            InternetToast.SetActive(true);
            InternetToast.GetComponent<Toast>().SetToast("Please check your connectivity and try again.");

        }
       
    }
}

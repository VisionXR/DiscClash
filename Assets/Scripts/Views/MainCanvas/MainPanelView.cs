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
        public GameObject HomePanel;
        public GameObject SinglePlayerPanel;
        public GameObject PlayWithFriendsPanel;
        public GameObject PlayWithStrangersPanel;
        public GameObject InternetToast;
        public GameObject VsCPUPanel;

        [Header(" Player UI ")]
        public Image playerImage;
        public TMP_Text playerName;
        public TMP_Text playerCoins;



        public void VsCPUBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            VsCPUPanel.SetActive(true);
            uiOutputData.SetGameType(GameType.VsCPU);

            HomePanel.SetActive(false);
        }

        public void OnlineMultiPlayerBtnClicked()
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

            HomePanel.SetActive(false);
        }

        public void PlayWithFriendsBtnClicked()
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

            HomePanel.SetActive(false);
        }
        public void TutorialBtnClicked()
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

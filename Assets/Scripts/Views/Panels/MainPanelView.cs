using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class MainPanelView : MonoBehaviour
    {

        [Header(" Scriptable Objects ")]
        public UIOutputDataSO uiOutputData;
        public MyPlayerSettings myPlayerSettings;
        public NetworkOutputSO networkOutputData;
        public OculusDataSO oculusData;


        [Header(" Panels ")]
        public GameObject SinglePlayerPanel;
        public GameObject PrivatePublicPanel;
        public GameObject InternetToast;
        public GameObject MainSettingsPanel;


        private Action OnConnectionSuccess;
        private Action OnConnectionFailure;


        public void OnSinglePlayerClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            SinglePlayerPanel.SetActive(true);
            uiOutputData.SetGameType(GameType.SinglePlayer);
            MainSettingsPanel.SetActive(false);
            gameObject.SetActive(false);
        }

        public void OnMultiPlayerClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            if(Application.internetReachability == NetworkReachability.NotReachable)
            {
                DisplayToast();
                return;
            }

            uiOutputData.SetGameType(GameType.MultiPlayer);
            PrivatePublicPanel.SetActive(true);
            MainSettingsPanel.SetActive(false);
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
            oculusData.ConnectToDestination(d,OnConnectionSuccess,OnConnectionFailure);
            
            MainSettingsPanel.SetActive(false);
            gameObject.SetActive(false);
        }


        public void TrickShotClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.SetGameType(GameType.TrickShots);
            Destination d = new Destination();
            d.gameType = GameType.TrickShots;
            d.roomName = "NA";
            d.lobbyName = "NA";
            oculusData.ConnectToDestination(d, OnConnectionSuccess, OnConnectionFailure);

            MainSettingsPanel.SetActive(false);
            gameObject.SetActive(false);
        }
        private void DisplayToast()
        {
            InternetToast.SetActive(true);
            InternetToast.GetComponent<Toast>().SetToast("Please check your connectivity and try again.");

        }
       
    }
}

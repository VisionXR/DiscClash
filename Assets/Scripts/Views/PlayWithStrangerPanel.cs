using UnityEngine;
using UnityEngine.UI;
using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;

namespace com.VisionXR.Views
{

    public class PlayWithStrangerPanel : MonoBehaviour
    {  

        [Header(" Scriptable Objects ")]
        public MyPlayerSettings playerSettings;
        public UIOutputDataSO uiOutputData;
        public NetworkInputSO networkInput;
        public NetworkOutputSO networkOutput;
        public OculusDataSO oculusData;

       
        [Header(" Panel Objects ")]
        public GameObject TwoPlayerPanel;
        public GameObject FourPlayerPanel;
        public GameObject PrivatePublicPanel;


        [Header(" Other UI Objects ")]
        public TMP_Text roomName;
        public Image BlockerImage;

        [Header(" Selection HightLight Images ")]
        [SerializeField] private Image P1VsP2SelectedImage;
        [SerializeField] private Image P1AIVsP2AISelectedImage;
        [SerializeField] private Image P1P2VsAISelectedImage;
        [SerializeField] private Image P1P2VsP3P4SelectedImage;
        [SerializeField] private Image EasyelectedImage;
        [SerializeField] private Image MediumSelectedImage;
        [SerializeField] private Image HardSelectedImage;
        [SerializeField] private Image BlackAndWhiteSelectedImage;
        [SerializeField] private Image FreeStyleSelectedImage;


        private Action OnConnectionSuccessEvent, OnConnectionFailEvent;
        private void OnEnable()
        {
            ResetButtons();
            Initialize();

            OnConnectionSuccessEvent += OnConnectionSuccess;
            OnConnectionFailEvent += OnConnectionFail;

        }
        private void OnDisable()
        {
            OnConnectionSuccessEvent -= OnConnectionSuccess;
            OnConnectionFailEvent -= OnConnectionFail;
        }

        private void OnConnectionSuccess()
        {
            if(uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
            {
                TwoPlayerPanel.SetActive(true);
            }
            else
            {
                FourPlayerPanel.SetActive(true);
            }

            BlockerImage.gameObject.SetActive(false);
            gameObject.SetActive(false);

            StopAllCoroutines();
        }

        private void OnConnectionFail()
        {
            BlockerImage.gameObject.SetActive(false);
            StopAllCoroutines();
        }

        public void JoinRoomBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            uiOutputData.SetMyBoard(playerSettings.MyBoard);
            uiOutputData.SetMyCoinsId(playerSettings.MyCoinsId);
            Destination d = new Destination();    
            d.multiPlayerGameMode = uiOutputData.multiPlayerGameMode;
            d.gameType = GameType.MultiPlayer;
            d.game = uiOutputData.game;
            d.region = playerSettings.serverRegion;
            d.isJoinable = true;
            d.roomName = "NA";
            BlockerImage.gameObject.SetActive(true);
            oculusData.ConnectToDestination(d, OnConnectionSuccessEvent, OnConnectionFailEvent);
            StartCoroutine(ConnectingToRoom());
        }
      
        private void Initialize()
        {

            roomName.text = "";
            BlockerImage.gameObject.SetActive(false);

            // Game mode selection
            if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
            {
                P1VsP2SelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                P1VsP2SelectedImage.color = Color.white;
               
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1P2vsAI)
            {
                P1P2VsAISelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                P1P2VsAISelectedImage.color = Color.white;
              
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1AIvsP2AI)
            {
                P1AIVsP2AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                P1AIVsP2AISelectedImage.color = Color.white;
               
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1P2vsP3P4)
            {
                P1P2VsP3P4SelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                P1P2VsP3P4SelectedImage.color = Color.white;

            }


            // Game Type selection
            if (uiOutputData.game == Game.BlackAndWhite)
            {
                BlackAndWhiteSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                BlackAndWhiteSelectedImage.color = Color.white;
            }
            else if (uiOutputData.game == Game.FreeStyle)
            {
                FreeStyleSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                FreeStyleSelectedImage.color = Color.white;
            }

            // Difficulty selection
            if (uiOutputData.aIDifficulty == AIDifficulty.Easy)
            {
                EasyelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                EasyelectedImage.color = Color.white;

            }
            else if (uiOutputData.aIDifficulty == AIDifficulty.Medium)
            {
                MediumSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                MediumSelectedImage.color = Color.white;
            }
            else if (uiOutputData.aIDifficulty == AIDifficulty.Hard)
            {
                HardSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                HardSelectedImage.color = Color.white;
            }        
           
        }


        private IEnumerator ConnectingToRoom()
        {
            while (true)
            {
                roomName.text = "Connecting.";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting..";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting...";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting....";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting.....";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting......";
            }
        }


        #region Button Clicks
        public void P1VsP2ButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();

            P1VsP2SelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            P1VsP2SelectedImage.color = Color.white;

            uiOutputData.SetPlayerCount(2);
            uiOutputData.SetGameMode(MultiPlayerGameMode.P1vsP2);
           

        }
        public void P1AIVsP2AIButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();

            P1AIVsP2AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            P1AIVsP2AISelectedImage.color = Color.white;

            uiOutputData.SetPlayerCount(2);
            uiOutputData.SetGameMode(MultiPlayerGameMode.P1AIvsP2AI);
           


        }
        public void P1P2VsAIButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();

            P1P2VsAISelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            P1P2VsAISelectedImage.color = Color.white;

            uiOutputData.SetPlayerCount(2);
            uiOutputData.SetGameMode(MultiPlayerGameMode.P1P2vsAI);
           

        }

        public void P1P2VsP3P4ButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();

            P1P2VsP3P4SelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            P1P2VsP3P4SelectedImage.color = Color.white;

            uiOutputData.SetPlayerCount(4);
            uiOutputData.SetGameMode(MultiPlayerGameMode.P1P2vsP3P4);

        }


        public void OnBlackAndWhiteClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameTypeImages();

            BlackAndWhiteSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            BlackAndWhiteSelectedImage.color = Color.white;

            uiOutputData.SetGame(Game.BlackAndWhite);
           

        }
        public void OnFreeStyleClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameTypeImages();

            FreeStyleSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            FreeStyleSelectedImage.color = Color.white;

            uiOutputData.SetGame(Game.FreeStyle);
           
        }


        public void OnEasyButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetDifficulty();

            EasyelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            EasyelectedImage.color = Color.white;

            uiOutputData.SetAIDifficulty(AIDifficulty.Easy);
        }
        public void OnMediumButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetDifficulty();

            MediumSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            MediumSelectedImage.color = Color.white;

            uiOutputData.SetAIDifficulty(AIDifficulty.Medium);
        }
        public void OnHardButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetDifficulty();

            HardSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            HardSelectedImage.color = Color.white;

            uiOutputData.SetAIDifficulty(AIDifficulty.Hard);
        }


        public void OBackButtonClicked()
        {          
            AudioManager.instance.PlayButtonClickSound();
            PrivatePublicPanel.SetActive(true);
            gameObject.SetActive(false);        
        }


        #endregion

        #region Reset Variables
        private void ResetGameModeImages()
        {
            P1VsP2SelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            P1AIVsP2AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            P1P2VsAISelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            P1P2VsP3P4SelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
           

            P1VsP2SelectedImage.color = AppProperties.instance.IdleColor;
            P1AIVsP2AISelectedImage.color = AppProperties.instance.IdleColor;
            P1P2VsAISelectedImage.color = AppProperties.instance.IdleColor;
            P1P2VsP3P4SelectedImage.color = AppProperties.instance.IdleColor;
           
        }
        public void ResetGameTypeImages()
        {
            BlackAndWhiteSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            FreeStyleSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;

            BlackAndWhiteSelectedImage.color = AppProperties.instance.IdleColor;
            FreeStyleSelectedImage.color = AppProperties.instance.IdleColor;
        }
        public void ResetDifficulty()
        {

            EasyelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            MediumSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            HardSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;

            EasyelectedImage.color = AppProperties.instance.IdleColor;
            MediumSelectedImage.color = AppProperties.instance.IdleColor;
            HardSelectedImage.color = AppProperties.instance.IdleColor;
        }
        private void ResetButtons()
        {
            P1VsP2SelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            P1AIVsP2AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            P1P2VsAISelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            P1P2VsP3P4SelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            

            P1VsP2SelectedImage.color = AppProperties.instance.IdleColor;
            P1AIVsP2AISelectedImage.color = AppProperties.instance.IdleColor;
            P1P2VsAISelectedImage.color = AppProperties.instance.IdleColor;
            P1P2VsP3P4SelectedImage.color = AppProperties.instance.IdleColor;
           

            BlackAndWhiteSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            FreeStyleSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;

            BlackAndWhiteSelectedImage.color = AppProperties.instance.IdleColor;
            FreeStyleSelectedImage.color = AppProperties.instance.IdleColor;

            EasyelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            MediumSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            HardSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;

            EasyelectedImage.color = AppProperties.instance.IdleColor;
            MediumSelectedImage.color = AppProperties.instance.IdleColor;
            HardSelectedImage.color = AppProperties.instance.IdleColor;
        }
    

        #endregion

    }
}

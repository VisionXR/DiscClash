using UnityEngine;
using UnityEngine.UI;
using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;
using TMPro;
using System.Collections;
using System;

namespace com.VisionXR.Views
{

    public class MultiPlayerPanelView : MonoBehaviour
    {

        #region Variables

        [Header(" Scriptable Objects ")]
        public MyPlayerSettings playerSettings;
        public UIOutputDataSO uiOutputData;
        public NetworkOutputSO networkOutputData;
        public ChatDataSO chatData;


        [Space(5)]
        [Header(" Variables ")]
        public GameObject ScorePanel2Players;
        public GameObject ScorePanel4Players;
        public GameObject MainPanel;
        public GameObject MainSettingsPanel;
        public GameObject RoomFailedPanel;
        public GameObject RandomRoomPopUp;
        public GameObject RandomWaitingToast;
        public TMP_Text roomName;
        public TMP_Text joinRoomText;

        [Space(5)]
        [Header(" Selection HightLight Images ")]
        [SerializeField] private Image P1VsP2SelectedImage;
        [SerializeField] private Image P1AIVsP2AISelectedImage;
        [SerializeField] private Image P1P2VsAISelectedImage;
        [SerializeField] private Image EasyelectedImage;
        [SerializeField] private Image MediumSelectedImage;
        [SerializeField] private Image HardSelectedImage;
        [SerializeField] private Image BlackAndWhiteSelectedImage;
        [SerializeField] private Image FreeStyleSelectedImage;
        #endregion

        private Coroutine roomStatusRoutine;

        #region Unity Callbacks
        private void OnEnable()
        {
            ResetButtons();
            Initialize();
            ChangeJoinRoomText();

            if(roomStatusRoutine == null)
            {
                roomStatusRoutine = StartCoroutine(ConnectingToRoom());
            }
        }
        private void OnDisable()
        {

            if (roomStatusRoutine != null)
            {
                StopCoroutine(roomStatusRoutine);
                roomStatusRoutine = null;
            }
        }

        private void RoomSuccess()
        {
            if (roomStatusRoutine != null)
            {
                StopCoroutine(roomStatusRoutine);
                roomStatusRoutine = null;
            }
            roomName.text = "Room ID : " + networkOutputData._runner.SessionInfo.Name;
        }

        private void RoomFailed(string reason)
        {
            if (roomStatusRoutine != null)
            {
                StopCoroutine(roomStatusRoutine);
                roomStatusRoutine = null;
            }
            roomName.text = reason;
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

        private void Initialize()
        {
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

        #endregion region

        #region Create Room Buttons Region
       
        public void JoinButtonClicked()
        {
            if (roomStatusRoutine == null)
            {
                AudioManager.instance.PlayButtonClickSound();
                //chatData.ConnectToChatServer();
                RandomRoomPopUp.SetActive(true);
            }
            else
            {
                RandomWaitingToast.SetActive(true);
                RandomWaitingToast.GetComponent<Toast>().SetToast(" Connecting to room. Please wait ");
            }

        }

        #endregion

        #region Button Clicks
        public void P1VsP2ButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();

            P1VsP2SelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            P1VsP2SelectedImage.color = Color.white;
            uiOutputData.SetGameMode(MultiPlayerGameMode.P1vsP2);
            ChangeJoinRoomText();
           
        }

        public void P1AIVsP2AIButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();

            P1AIVsP2AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            P1AIVsP2AISelectedImage.color = Color.white;

            uiOutputData.SetGameMode(MultiPlayerGameMode.P1AIvsP2AI);
            ChangeJoinRoomText();
          

        }
        public void P1P2VsAIButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();

            P1P2VsAISelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            P1P2VsAISelectedImage.color = Color.white;

            uiOutputData.SetGameMode(MultiPlayerGameMode.P1P2vsAI);
            ChangeJoinRoomText();
           
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

        private void ChangeJoinRoomText()
        {
            joinRoomText.text = " Join Random ( " + Enum.GetName(typeof(MultiPlayerGameMode), uiOutputData.multiPlayerGameMode) + " )";
        }

        public void OnBackButtonClicked()
        {          
            AudioManager.instance.PlayButtonClickSound();
            if(roomStatusRoutine !=null)
            {
                return;
            }
            MainPanel.SetActive(true);
            MainSettingsPanel.SetActive(true);
            gameObject.SetActive(false);
           
        }


        #endregion

        #region Reset Variables
        private void ResetGameModeImages()
        {
            P1VsP2SelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            P1AIVsP2AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            P1P2VsAISelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;

            P1VsP2SelectedImage.color = AppProperties .instance.IdleColor;
            P1AIVsP2AISelectedImage.color = AppProperties .instance.IdleColor;
            P1P2VsAISelectedImage.color = AppProperties.instance.IdleColor;
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

            P1VsP2SelectedImage.color = AppProperties.instance.IdleColor;
            P1AIVsP2AISelectedImage.color = AppProperties.instance.IdleColor;
            P1P2VsAISelectedImage.color = AppProperties.instance.IdleColor;

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

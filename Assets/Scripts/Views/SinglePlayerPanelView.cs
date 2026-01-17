using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{

    public class SinglePlayerPanelView : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public OculusDataSO oculusData;
        public MyPlayerSettings myplayerSettings;

        [Header("Panels")]
        [SerializeField] private GameObject MainPanel;
        [SerializeField] private GameObject MainSettingsPanel;
        [SerializeField] private GameObject TwoPlayerScorePanel;
        [SerializeField] private GameObject FourPlayerScorePanel;

        [Space(5)]
        [Header("Background Images")]
        [SerializeField] private Image EasySelectedImage;
        [SerializeField] private Image MediumSelectedImage;
        [SerializeField] private Image HardSelectedImage;
        [SerializeField] private Image vs1AISelectedImage;
        [SerializeField] private Image vs3AISelectedImage;
        [SerializeField] private Image BlackAndWhiteSelectedImage;
        [SerializeField] private Image FreeStyleSelectedImage;


        private Action OnConnectionSuccessEvent, OnConnectionFailEvent;
        public Destination currentDestination;

        private void OnEnable()
        {
            ResetSelectedImages();
            Initialize();

            OnConnectionSuccessEvent += OnConnectionSuccess;
            OnConnectionFailEvent += OnConnectionFail;
        }

        private void OnDisable()
        {
            OnConnectionSuccessEvent -= OnConnectionSuccess;
            OnConnectionFailEvent -= OnConnectionFail;
        }

        private void Initialize()
        {
            // Diffculty region
            if (uiOutputData.aIDifficulty == AIDifficulty.Easy)
            {
                EasySelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                EasySelectedImage.color = Color.white;
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



            // Game mode selection
            if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
            {
                vs1AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                vs1AISelectedImage.color = Color.white;

               
            }
            else if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PAIvsAI)
            {
                vs3AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
                vs3AISelectedImage.color = Color.white;

               
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

        }

        public void PlayAgainst1AI()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            vs1AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            vs1AISelectedImage.color = Color.white;
            uiOutputData.SetSingleGameMode(SinglePlayerGameMode.PvsAI);
         
        }

        public void PlayAgainst3AI()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            vs3AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            vs3AISelectedImage.color = Color.white;
            uiOutputData.SetSingleGameMode(SinglePlayerGameMode.PAIvsAI);
      
        }

     
        public void OnEasyClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetDifficultyImages();
            EasySelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            EasySelectedImage.color = Color.white;
            uiOutputData.SetAIDifficulty(AIDifficulty.Easy);
        }
        public void OnMediumClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetDifficultyImages();
            MediumSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            MediumSelectedImage.color = Color.white;
            uiOutputData.SetAIDifficulty(AIDifficulty.Medium);
        }
        public void OnHardClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetDifficultyImages();
            HardSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            HardSelectedImage.color = Color.white;
            uiOutputData.SetAIDifficulty(AIDifficulty.Hard);
        }

        public void BlackAndWhiteClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameType();
            BlackAndWhiteSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            BlackAndWhiteSelectedImage.color = Color.white;
            uiOutputData.SetGame(Game.BlackAndWhite);
        }
        public void FreeStyleClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameType();
            FreeStyleSelectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            FreeStyleSelectedImage.color = Color.white;
            uiOutputData.SetGame(Game.FreeStyle);
        }


        public void StartSinglePlayerClicked()
        {          
            AudioManager.instance.PlayButtonClickSound();    
            

            currentDestination = new Destination();
            currentDestination.gameType = GameType.SinglePlayer;
            currentDestination.singlePlayerGameMode = uiOutputData.singlePlayerGameMode;
            currentDestination.game = uiOutputData.game;
            currentDestination.isJoinable = false;
            currentDestination.roomName = "NA";
            currentDestination.lobbyName = "NA";

            uiOutputData.SetMyBoard(myplayerSettings.MyBoard);
            uiOutputData.SetMyCoinsId(myplayerSettings.MyCoinsId);

            oculusData.ConnectToDestination(currentDestination, OnConnectionSuccessEvent, OnConnectionFailEvent);
        }

        public void OnConnectionSuccess()
        {

            Debug.Log(" Mode is " + uiOutputData.singlePlayerGameMode);
            if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
            {
                TwoPlayerScorePanel.SetActive(true);
                FourPlayerScorePanel.SetActive(false);

            }
            else 
            {
                TwoPlayerScorePanel.SetActive(false);
                FourPlayerScorePanel.SetActive(true);
            }

            gameObject.SetActive(false);
        }

        public void OnConnectionFail()
        {
            Debug.Log(" In Connection fail");
        }
        public void BackButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            MainPanel.SetActive(true);
            MainSettingsPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void ResetGameModeImages()
        {
            vs1AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            vs3AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;

            vs1AISelectedImage.color = AppProperties.instance.IdleColor;
            vs3AISelectedImage.color = AppProperties.instance.IdleColor;


        }


        public void ResetGameType()
        {
            BlackAndWhiteSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            FreeStyleSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;

            BlackAndWhiteSelectedImage.color = AppProperties.instance.IdleColor;
            FreeStyleSelectedImage.color = AppProperties.instance.IdleColor;

        }

        public void ResetDifficultyImages()
        {
            EasySelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            MediumSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            HardSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;

            EasySelectedImage.color = AppProperties.instance.IdleColor;
            MediumSelectedImage.color = AppProperties.instance.IdleColor;
            HardSelectedImage.color = AppProperties.instance.IdleColor;
        }
        private void ResetSelectedImages()
        {
           

            vs1AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            vs3AISelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;

            BlackAndWhiteSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            FreeStyleSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;

            EasySelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            MediumSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;
            HardSelectedImage.gameObject.GetComponent<UIGradient>().enabled = false;

          

            vs1AISelectedImage.color = AppProperties.instance.IdleColor;
            vs3AISelectedImage.color = AppProperties.instance.IdleColor;

            BlackAndWhiteSelectedImage.color = AppProperties.instance.IdleColor;
            FreeStyleSelectedImage.color = AppProperties.instance.IdleColor;

            EasySelectedImage.color = AppProperties.instance.IdleColor;
            MediumSelectedImage.color = AppProperties.instance.IdleColor;
            HardSelectedImage.color = AppProperties.instance.IdleColor;
        }


    }
}

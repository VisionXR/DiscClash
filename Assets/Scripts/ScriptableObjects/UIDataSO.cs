using System;
using UnityEngine;
using com.VisionXR.HelperClasses;
using System.Collections.Generic;
using com.VisionXR.Controllers;
using com.VisionXR.Views;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "UIDataSO", menuName = "ScriptableObjects/UIDataSO")]
    public class UIDataSO : ScriptableObject
    {
        // variables
        [Header("References")]
        public UIManager uiManager;


        [Header("OutPut")]
        public GameType currentGameType;
        public GameMode currentGameMode;
        public AIDifficulty currentAIDifficulty;

        public LoginType currentLoginType;
        public float disableTime = 0.5f;
        public Color selectionColor;
        public Color defaultColor;
        // Actions

        public Action HomeEvent;
        public Action<int> ShowTurnEvent;
        public Action ShowFoulEvent;
        public Action ShowFoulHandlingEvent;
        public Action PlaceStrikerEvent;

        public Action SetCoinsEvent;
        public Action UpdateCoinsEvent;
        public Action<int> SetPlayerDataEvent;

        public Action ExitBtnClickedEvent;
        public Action ResetAllPanelsEvent;

        // Mic and speaker Actions
        public Action TurnOnMicEvent;
        public Action TurnOffMicEvent;

        public Action TurnOnSpeakerEvent;
        public Action TurnOffSpeakerEvent;

        //Methods

        public void SetUIMachine(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }

        public void UpdateCoins()
        {
            UpdateCoinsEvent?.Invoke();
        }

        public void SetCoins()
        {
            SetCoinsEvent?.Invoke();
        }

        public void SetPlayerData(int id)
        {
            SetPlayerDataEvent?.Invoke(id);
        }

        public void ShowFoulHandling()
        {
            ShowFoulHandlingEvent?.Invoke();
        }

        public void ShowFoul()
        {
            ShowFoulEvent?.Invoke();
        }

        public void ShowTurn(int playerNumber)
        {
            ShowTurnEvent?.Invoke(playerNumber);
        }

        public void TriggerHomeEvent()
        {
            HomeEvent?.Invoke();
        }

        public void SetGameType(GameType gameType)
        {
            currentGameType = gameType;
        }

        public void SetGameMode(GameMode gameMode)
        {
            currentGameMode = gameMode;
        }



        public void SetLoginType(LoginType loginType)
        {
            currentLoginType = loginType;
        }

        public void SetAIDifficulty(AIDifficulty aiDifficulty)
        {
            currentAIDifficulty = aiDifficulty;
        }



        public void ExitButtonClicked()
        {
            ExitBtnClickedEvent?.Invoke();
        }


        public void ResetAllPanels()
        {
            ResetAllPanelsEvent?.Invoke();
        }
    }
}

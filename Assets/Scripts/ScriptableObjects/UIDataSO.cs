using System;
using UnityEngine;
using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "UIDataSO", menuName = "ScriptableObjects/UIDataSO")]
    public class UIDataSO : ScriptableObject
    {
        // variables
        [Header("References")]
        public UIManager uiManager;
        public BoardType currentBoardType;


        [Header("OutPut")]
        public float disableTime = 0.3f;

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


        public void ResetAllPanels()
        {
            ResetAllPanelsEvent?.Invoke();
        }
    }
}

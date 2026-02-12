using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "UIInputDataSO", menuName = "ScriptableObjects/UIInputDataSO", order = 1)]    
    public class UIInputDataSO : ScriptableObject   
    {
        // variables


        // Game Events
        public Action StartSinglePlayerGameEvent;
        public Action StartMultiPlayerGameEvent;
        public Action StartTutorialEvent;

        public Action ExitGameEvent;
        public Action HomeEvent;
        public Action PlayAgainEvent;
        public Action ShowLoginEvent;
        public Action<GameResult> ShowGameResultEvent;


        //Network  Events    
        public Action<Destination> ShowDestinationPanelEvent;
        public Action<Player> ShowPlayerDetailsEvent;
        public Action OtherPlayerLeftGameEvent;


        // Mic And Speaker Events
        public Action TurnOnMicEvent;
        public Action TurnOffMicEvent;
        public Action TurnOnSpeakerEvent;
        public Action TurnOffSpeakerEvent;



        // Methods

        public void StartSinglePlayerGame()
        {
            StartSinglePlayerGameEvent?.Invoke();
        }
        public void StartMultiPlayerGame()
        {
            StartMultiPlayerGameEvent?.Invoke();
        }
        public void StartTutorial()
        {
            StartTutorialEvent?.Invoke();
        }
        public void GameCompleted(GameResult gameResult)
        {
            ShowGameResultEvent?.Invoke(gameResult);
        }

        public void ExitGame()
        {
            ExitGameEvent?.Invoke();
        }

        public void GoToHome()
        {
            HomeEvent?.Invoke();
        }

        public void PlayAgain()
        {
            PlayAgainEvent?.Invoke();
        }

        public void ShowLogin()
        {
            ShowLoginEvent?.Invoke();
        }   


        public void ShowDestination(Destination destination)
        {
            ShowDestinationPanelEvent?.Invoke(destination);
        }

        public void ShowPlayerDetails(Player p)
        {
            ShowPlayerDetailsEvent?.Invoke(p);      
        }   
        
        public void OtherPlayerLeft()
        {
            OtherPlayerLeftGameEvent?.Invoke();
        }

        public void TurnOnMic()
        {
            TurnOnMicEvent?.Invoke();
        }
        public void TurnOffMic()
        {
            TurnOffMicEvent?.Invoke();
        }

        public void TurnOnSpeaker()
        {
            TurnOnSpeakerEvent?.Invoke();
        }

        public void TurnOffSpeaker()
        {
            TurnOffSpeakerEvent?.Invoke();
        }
    }
}
        
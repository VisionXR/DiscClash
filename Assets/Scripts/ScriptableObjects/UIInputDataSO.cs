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
    

        // Events
        public Action<GameResult> ShowGameResultEvent;       
        public Action ResetPanelsEvent;
        public Action<Player> ShowPlayerDetailsEvent;
        public Action OtherPlayerLeftGameEvent;

        public Action<MultiPlayerGameMode> GoToGamePanelEvent;
        public Action DestinationFailedEvent;

        public Action<Destination> ConnectToDestinationEvent;
        public Action<Destination> ChangeDestinationEvent;


        public Action<int> SetButtonEvent;
        public Action<int,string> SetPlayerStatusEvent;

        // Methods



        public void ResetPanels()
        {
            ResetPanelsEvent?.Invoke();
        }

        public void ShowPlayerDetails(Player p)
        {
            ShowPlayerDetailsEvent?.Invoke(p);      
        }   
        
        public void OtherPlayerLeft()
        {
            OtherPlayerLeftGameEvent?.Invoke();
        }

        public void GoToGamePanel(MultiPlayerGameMode mode)
        {
            GoToGamePanelEvent?.Invoke(mode);
        }

        public void ConnectToDestination(Destination d)
        {
            ConnectToDestinationEvent?.Invoke(d);
        }

        public void ChangeDestination(Destination d)
        {
            ChangeDestinationEvent?.Invoke(d);
        }
    }
}
        
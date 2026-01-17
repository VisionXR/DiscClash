using com.VisionXR.HelperClasses;
using Fusion;
using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "NetworkInputSO", menuName = "ScriptableObjects/NetworkInputSO", order = 1)]
    public class NetworkInputSO : ScriptableObject
    {
        // In Game Events
        public Action<int> StartGameEvent;
        public Action<int> PlayerReadyEvent;
        public Action<PlayerCoin> PutFineEvent;
        public Action<string> DestroyCoinsFellInThisTurnEvent;
        public Action<CurrentGameData> CurrentGameDataReceivedEvent;
        public Action<GameResult> GameResultReceivedEvent;


        public Action PlayerStrikeStartedEvent;
        public Action PlayerStrikeEndedEvent;


        // Events
        public Action<ServerRegion,string,Action,Action<string>> CreateRoomEvent;
        public Action<ServerRegion, string,Action,Action<string>> JoinLobbyEvent;
        public Action<ServerRegion,string,Action,Action<string>> JoinRoomEvent;
        public Action LeaveRoomEvent;
      

        
        // Methods      

        public void CreateRoom(ServerRegion region,string RoomName,Action RoomSuccessEvent,Action<string> RoomFailedEvent)
        {
            CreateRoomEvent?.Invoke(region,RoomName,RoomSuccessEvent,RoomFailedEvent);
        }

        public void JoinRoom(ServerRegion region,string roomName, Action RoomSuccessEvent, Action<string> RoomFailedEvent)
        {
            JoinRoomEvent?.Invoke(region, roomName,RoomSuccessEvent,RoomFailedEvent);
        }

        public void JoinLobby(ServerRegion region, string lobbyName, Action LobbySuccessEvent, Action<string> LobbyFailedEvent)
        {
            JoinLobbyEvent?.Invoke(region, lobbyName, LobbySuccessEvent, LobbyFailedEvent);  
        }
        public void LeaveRoom()
        {
            LeaveRoomEvent?.Invoke();
        }



        public void StartGame(int id)
        {
            StartGameEvent?.Invoke(id);
        }

        public void DestroyCoinsFellInThisTurn(string coins)
        {
            DestroyCoinsFellInThisTurnEvent?.Invoke(coins);
        }

        public void SetCurrentGameData(CurrentGameData currentGameData)
        {

            CurrentGameDataReceivedEvent?.Invoke(currentGameData);
        }

        public void SetGameResult(GameResult gameResult)
        {
            GameResultReceivedEvent?.Invoke(gameResult);
        }

        public void PutFine(PlayerCoin coin)
        {
            PutFineEvent?.Invoke(coin);
        }

    }
}

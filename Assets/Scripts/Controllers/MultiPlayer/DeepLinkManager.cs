using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;
using System;
using UnityEngine;
using System.Collections.Generic;

public class DeepLinkManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public MyPlayerSettings playerSettings;
    public OculusDataSO oculusData;
    public UIInputDataSO uiInputData;
    public UIOutputDataSO uiOutputData;
    public NetworkInputSO networkInput;
    public NetworkOutputSO networkOutput;


    [Header("Scripts")]
    public DestinationManager destinationManager;
    // Action
    public Destination currentDestination;
    public Action OnConnectedEvent;
    public Action OnFailedEvent;
    public Action RoomCreateSuccessEvent, RoomJoinSuccessEvent, LobbyJoinSuccessEvent;
    public Action<string> RoomCreateFailedEvent, RoomJoinFailedEvent, LobbyJoinFailedEvent;
    private void OnEnable()
    {
        oculusData.ConnectToDestinationEvent += ConnectToDestination;

        RoomCreateSuccessEvent += RoomCreateSuccess;
        RoomCreateFailedEvent += RoomJoinFailed;

        RoomJoinSuccessEvent += RoomJoinSuccess;
        RoomJoinFailedEvent += RoomJoinFailed;

        LobbyJoinSuccessEvent += LobbyJoinSuccess;
        LobbyJoinFailedEvent += LobbyJoinFailed;

        networkOutput.RoomListUpdatedEvent += RoomListUpdated;
    }

    private void OnDisable()
    {
        oculusData.ConnectToDestinationEvent -= ConnectToDestination;

        RoomCreateSuccessEvent -= RoomCreateSuccess;
        RoomCreateFailedEvent -= RoomCreateFailed;

        RoomJoinSuccessEvent -= RoomJoinSuccess;
        RoomJoinFailedEvent -= RoomJoinFailed;

        LobbyJoinSuccessEvent -= LobbyJoinSuccess;
        LobbyJoinFailedEvent -= LobbyJoinFailed;

        networkOutput.RoomListUpdatedEvent -= RoomListUpdated;
    }

    private void ConnectToDestination(Destination destination, Action OnConnected, Action OnFailed)
    {
        currentDestination = destination;
        uiOutputData.gameType = destination.gameType;
   

        this.OnConnectedEvent = OnConnected;
        this.OnFailedEvent = OnFailed;

        if(destination.gameType == GameType.SinglePlayer)
        {
            uiOutputData.game = destination.game;
            uiOutputData.singlePlayerGameMode = destination.singlePlayerGameMode;
           

            OnConnectedEvent?.Invoke();
            uiOutputData.StartSinglePlayerGame();
            destinationManager.SetDestination(destination);
            
        }
        else if (destination.gameType == GameType.MultiPlayer)
        {
            uiOutputData.game = destination.game;
            uiOutputData.multiPlayerGameMode = destination.multiPlayerGameMode;
        
            if(destination.roomName == "NA")
            {
                JoinLobby();
            }
            else
            {
                networkInput.JoinRoom(destination.GetRegion(), currentDestination.roomName, RoomJoinSuccessEvent, RoomJoinFailedEvent);
            }
        }
        else if (destination.gameType == GameType.Tutorial)
        {
            OnConnectedEvent?.Invoke();
            uiOutputData.StartTutorial();
            destinationManager.SetDestination(destination);
        }

        else if (destination.gameType == GameType.TrickShots)
        {
            OnConnectedEvent?.Invoke();
            uiOutputData.StartTrickShots();
            destinationManager.SetDestination(destination);
        }

    }


    public void JoinLobby()
    {
        string lobbyName = Enum.GetName(typeof(MultiPlayerGameMode), uiOutputData.multiPlayerGameMode) + Enum.GetName(typeof(Game), uiOutputData.game);
        networkOutput.CommonLobby = lobbyName;
        networkInput.JoinLobby(playerSettings.serverRegion, lobbyName, LobbyJoinSuccessEvent, LobbyJoinFailedEvent);
    }

    public void CreateRoom()
    {
        string roomName = playerSettings.MyOculusId.ToString();
        networkInput.CreateRoom(playerSettings.serverRegion, roomName, RoomCreateSuccessEvent, RoomCreateFailedEvent);
    }

    private void RoomCreateSuccess()
    {
       
        currentDestination.lobbyName = networkOutput._runner.SessionInfo.Region ;
        currentDestination.roomName = networkOutput._runner.SessionInfo.Name;
        currentDestination.region = playerSettings.serverRegion;
             
       

        uiOutputData.StartMultiPlayerGame();
        currentDestination.isJoinable = true;
        destinationManager.SetDestination(currentDestination);

        OnConnectedEvent?.Invoke();
    }
    private void RoomCreateFailed(string reason)
    {
       
        OnFailedEvent?.Invoke();
        uiInputData.DestinationFailedEvent?.Invoke();
    }
    public void RoomJoinSuccess()
    {
       
        currentDestination.region = playerSettings.serverRegion;
        currentDestination.lobbyName = networkOutput._runner.SessionInfo.Region;
        currentDestination.roomName = networkOutput._runner.SessionInfo.Name;
     
      
      
        uiOutputData.StartMultiPlayerGame();
        uiInputData.GoToGamePanel(uiOutputData.multiPlayerGameMode);

        currentDestination.isJoinable = false;
        destinationManager.SetDestination(currentDestination);

        OnConnectedEvent?.Invoke();

    }

    public void RoomJoinFailed(string reason)
    {
       
        OnFailedEvent?.Invoke();
        uiInputData.DestinationFailedEvent?.Invoke();
    }

    private void LobbyJoinSuccess()
    {
        
    }
    private void LobbyJoinFailed(string reason)
    {
       
        OnFailedEvent?.Invoke();
        uiInputData.DestinationFailedEvent?.Invoke();
    }

    private void RoomListUpdated(List<AvailableRooms> roomList)
    {
       
        if (roomList != null && roomList.Count > 0)
        {
             networkInput.JoinRoom(playerSettings.serverRegion, roomList[0].roomName, RoomJoinSuccessEvent, RoomJoinFailedEvent);
        }
        else
        {
              CreateRoom();
        }
    }
}
                    
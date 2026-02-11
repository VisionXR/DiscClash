using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;
using System;
using UnityEngine;

public class DeepLinkManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public MyPlayerSettings playerSettings;
    public DestinationDataSO destinationData;
    public UIInputDataSO uiInputData;
    public UIOutputDataSO uiOutputData;
    public NetworkInputSO networkInput;
    public NetworkOutputSO networkOutput;



    // Action
    public Destination currentDestination;
    public Action OnDestinationSuccesEvent;
    public Action OnDestinationFailEvent;
    public Action RoomCreateSuccessEvent, RoomJoinSuccessEvent;
    public Action<string> RoomCreateFailedEvent, RoomJoinFailedEvent;


    private void Awake()
    {
        // Subscribe to the event for when the app is already running
        Application.deepLinkActivated += OnDeepLinkActivated;

        // Check if the app was started via a deep link
        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            OnDeepLinkActivated(Application.absoluteURL);
        }
        else
        {
            Debug.Log("Disc Clash: No deep link detected on startup.");
        }
    }
    private void OnEnable()
    {
        destinationData.ConnectToDestinationEvent += ConnectToDestination;

        RoomCreateSuccessEvent += RoomCreateSuccess;
        RoomCreateFailedEvent += RoomJoinFailed;

        RoomJoinSuccessEvent += RoomJoinSuccess;
        RoomJoinFailedEvent += RoomJoinFailed;
      
    }

    private void OnDisable()
    {
        destinationData.ConnectToDestinationEvent -= ConnectToDestination;

        RoomCreateSuccessEvent -= RoomCreateSuccess;
        RoomCreateFailedEvent -= RoomCreateFailed;

        RoomJoinSuccessEvent -= RoomJoinSuccess;
        RoomJoinFailedEvent -= RoomJoinFailed;

    }

    private void OnDeepLinkActivated(string url)
    {
        Debug.Log("Disc Clash: Link Received: " + url);
        // Example URL: discclash://pvp?lobbyId=XYZ

      
    }

    private void ConnectToDestination(Destination destination, Action OnConnected, Action OnFailed)
    {
        currentDestination = destination;
        uiOutputData.gameType = destination.gameType;
   

        OnDestinationSuccesEvent = OnConnected;
        OnDestinationFailEvent = OnFailed;

        if(destination.gameType == GameType.SinglePlayer)
        {
            uiOutputData.game = destination.game;
            uiOutputData.singlePlayerGameMode = destination.singlePlayerGameMode;
           

            OnDestinationSuccesEvent?.Invoke();
            uiInputData.StartSinglePlayerGame();
           
            
        }
        else if ((destination.gameType == GameType.OnlineMultiPlayer|| destination.gameType == GameType.PlayWithFriends))
        {
            uiOutputData.game = destination.game;
            uiOutputData.multiPlayerGameMode = destination.multiPlayerGameMode;
        
            if(destination.roomName == "")
            {
                string roomName = playerSettings.MyId.ToString();
                networkInput.CreateRoom(playerSettings.serverRegion, roomName, RoomCreateSuccessEvent, RoomCreateFailedEvent);
            }
            else
            {
                networkInput.JoinRoom(destination.GetRegion(), currentDestination.roomName, RoomJoinSuccessEvent, RoomJoinFailedEvent);
            }
        }
        else if (destination.gameType == GameType.Tutorial)
        {
            OnDestinationSuccesEvent?.Invoke();
            uiInputData.StartTutorial();
        
        }

    }

    private void RoomCreateSuccess()
    {
       
        currentDestination.lobbyName = networkOutput._runner.SessionInfo.Region ;
        currentDestination.roomName = networkOutput._runner.SessionInfo.Name;
        currentDestination.region = playerSettings.serverRegion;

        uiInputData.StartMultiPlayerGame();
        currentDestination.isJoinable = true;
   
        OnDestinationSuccesEvent?.Invoke();
    }
    private void RoomCreateFailed(string reason)
    {

        OnDestinationFailEvent?.Invoke();
        
    }
    public void RoomJoinSuccess()
    {
       
        currentDestination.region = playerSettings.serverRegion;
        currentDestination.lobbyName = networkOutput._runner.SessionInfo.Region;
        currentDestination.roomName = networkOutput._runner.SessionInfo.Name;

        uiInputData.StartMultiPlayerGame();
        currentDestination.isJoinable = false;
        OnDestinationSuccesEvent?.Invoke();

    }

    public void RoomJoinFailed(string reason)
    {       
        OnDestinationFailEvent?.Invoke();    
    }

}
                    
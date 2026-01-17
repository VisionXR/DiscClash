using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Fusion;
using Fusion.Photon.Realtime;
using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;
using Photon.Voice.Unity;

namespace com.VisionXR.Controllers
{
    /// <summary>
    /// Manages network operations, including room creation, joining, lobby management, and disconnection for multiplayer games.
    /// </summary>
    public class NetworkManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public NetworkInputSO networkInputData;
        public NetworkOutputSO networkOutputData;
        public PhotonAppSettings settings;
        public MyPlayerSettings playerSettings;
        public UIOutputDataSO uiOutputData;

        [Header("Game Objects")]
        public GameObject NetworkRunnerObject; // Prefab for creating a network runner instance
        private NetworkRunner runner;
        // Local variables
        private List<AvailableRooms> rooms;
     

        /// <summary>
        /// Subscribes to network-related events on enable.
        /// </summary>
        void OnEnable()
        {
            networkInputData.CreateRoomEvent += CreateRoom;
            networkInputData.JoinRoomEvent += JoinRoom;
            networkInputData.JoinLobbyEvent += JoinLobby;

            networkInputData.LeaveRoomEvent += DisconnectFromRoom;

        }

        /// <summary>
        /// Unsubscribes from network-related events on disable.
        /// </summary>
        void OnDisable()
        {
            networkInputData.CreateRoomEvent -= CreateRoom;
            networkInputData.JoinRoomEvent -= JoinRoom;
            networkInputData.JoinLobbyEvent -= JoinLobby;

            networkInputData.LeaveRoomEvent -= DisconnectFromRoom;
        }

        /// <summary>
        /// Initializes the network runner on start.
        /// </summary>
        void Start()
        {
            runner = GetComponent<NetworkRunner>();
        }

        /// <summary>
        /// Sets the server region for the network session based on the provided enum.
        /// </summary>
        private void SetRegion(ServerRegion region)
        {
            string regionName = region == ServerRegion.any ? "" : region.ToString().ToLower();
            settings.AppSettings.FixedRegion = regionName;
        }

        /// <summary>
        /// Creates a new room and starts a game session.
        /// </summary>
        private void CreateRoom(ServerRegion region, string roomName, Action RoomSuccessEvent, Action<string> RoomFailedEvent)
        {
            SetRegion(region);
            StartGame(roomName,RoomSuccessEvent,RoomFailedEvent);
        }


        /// <summary>
        /// Joins a specific room by name.
        /// </summary>
        private void JoinRoom(ServerRegion region,string roomName, Action RoomSuccessEvent, Action<string> RoomFailedEvent)
        {
            SetRegion(region);
            JoinGame(roomName,  RoomSuccessEvent, RoomFailedEvent);
        }


        /// <summary>
        /// Joins a lobby to fetch available sessions.
        /// </summary>
        private void JoinLobby(ServerRegion region,string lobbyName,Action LobbySuccessEvent,Action<string> LobbyFailedEvent)
        {
            SetRegion(region);
            JoinLobbyToFetchSessions(lobbyName,LobbySuccessEvent,LobbyFailedEvent);
        }

        /// <summary>
        /// Starts a new game session, setting properties and configurations based on UI and player settings.
        /// </summary>
        public async Task StartGame(string roomName,Action RoomSuccessEvent,Action<string> RoomFailedEvent)
        {
               InitializeNetworkRunner();
            
                var customRoomProps = new Dictionary<string, SessionProperty>
                {
                    { "gamemode", (int)uiOutputData.multiPlayerGameMode },
                    { "game", (int)uiOutputData.game },
                    { "board", uiOutputData.MyBoard },
                    { "difficulty", (int)uiOutputData.aIDifficulty },
                    { "myCoinsId", uiOutputData.MyCoinsId }
       
                };

                Fusion.GameMode gameMode = Fusion.GameMode.Shared;
                bool isRoomOpen = (uiOutputData.roomType == RoomType.Public);

                var result = await runner.StartGame(new StartGameArgs
                {
                    GameMode = gameMode,
                    SessionProperties = customRoomProps,
                    IsVisible = isRoomOpen,
                    AuthValues = new AuthenticationValues(playerSettings.MyOculusId.ToString()),
                    CustomLobbyName = networkOutputData.CommonLobby,
                    PlayerCount = uiOutputData.NoOfPlayers,
                    SessionName = roomName
                });

                if (result.Ok)
                {
                    RoomSuccessEvent?.Invoke();
                }
                else
                {

                    RoomFailedEvent?.Invoke("Could not create room ");
                }
            
        }

        /// <summary>
        /// Joins an existing game session.
        /// </summary>
        public async Task JoinGame(string roomName, Action RoomSuccessEvent, Action<string> RoomFailedEvent)
        {
            InitializeNetworkRunner();

            Debug.Log(" Joining " + roomName);

            var result = await runner.StartGame(new StartGameArgs
            {
                GameMode = Fusion.GameMode.Shared,
                CustomLobbyName = networkOutputData.CommonLobby,
                SessionName = roomName
            });

            if (result.Ok)
            {
              
                ReadRoomSessionProperties();
                RoomSuccessEvent?.Invoke();
            }
            else
            {
                RoomFailedEvent?.Invoke("Could not join room ");
            }
        }

        /// <summary>
        /// Joins a custom lobby to retrieve available sessions.
        /// </summary>
        public async Task JoinLobbyToFetchSessions(string lobbyName, Action LobbySuccessEvent, Action<string> LobbyFailedEvent)
        {
            InitializeNetworkRunner();

            var result = await runner.JoinSessionLobby(SessionLobby.Custom, lobbyName);

            if (result.Ok)
            {
                LobbySuccessEvent?.Invoke();

                
            }
            else
            {
                LobbyFailedEvent?.Invoke(" Could not join lobby");
            }
        }

        /// <summary>
        /// Disconnects from the current room and shuts down the runner.
        /// </summary>
        public void DisconnectFromRoom()
        {
            if (runner != null)
            {
                runner.Shutdown();
                runner = null;
                networkOutputData._runner = null;
            }
        }

        /// <summary>
        /// Initializes the NetworkRunner object and assigns it to the runner variable.
        /// </summary>
        private void InitializeNetworkRunner()
        {
             if(runner != null)
            {
                Destroy(runner.gameObject);
            }

             GameObject tmpObject = Instantiate(NetworkRunnerObject, transform);
             runner = tmpObject.GetComponent<NetworkRunner>();
             runner.ProvideInput = true;
             networkOutputData._runner = runner;
            
        }


        public void ReadRoomSessionProperties()
        {
            if (runner != null && runner.SessionInfo != null && runner.SessionInfo.Properties != null)
            {
                var props = runner.SessionInfo.Properties;

                if (props.TryGetValue("gamemode", out var gameModeProp))
                    uiOutputData.SetGameMode((HelperClasses.MultiPlayerGameMode)(int)gameModeProp);

                if (props.TryGetValue("game", out var gameProp))
                    uiOutputData.SetGame((Game)(int)gameProp);

                if (props.TryGetValue("board", out var boardProp))
                    uiOutputData.SetMyBoard((int)boardProp);

                if (props.TryGetValue("difficulty", out var difficultyProp))
                    uiOutputData.SetAIDifficulty((AIDifficulty)(int)difficultyProp);

                if (props.TryGetValue("myCoinsId", out var coinProp))
                    uiOutputData.SetMyCoinsId((int)coinProp);

                Debug.Log("[Network] Session properties received and assigned.");
            }
            else
            {
                Debug.LogWarning("SessionInfo or Properties are null, cannot read session properties.");
            }
        }

        public async Task FetchSessions(string lobbyName, Action LobbySuccessEvent, Action<string> LobbyFailedEvent)
        {
            
        }
    }

}


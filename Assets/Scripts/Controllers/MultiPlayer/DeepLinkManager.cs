using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;
using System;
using UnityEngine;
using System.Collections;

namespace com.VisionXR.Controllers
{

    public class DeepLinkManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public MyPlayerSettings playerSettings;
        public DestinationDataSO destinationData;
        public UIInputDataSO uiInputData;
        public UIOutputDataSO uiOutputData;
        public NetworkInputSO networkInput;
        public NetworkOutputSO networkOutput;
        public CloudDataSO cloudData;


        // Action
        public Destination homeDestination;
        public Destination currentDestination;
        public Action OnDestinationSuccesEvent;
        public Action OnDestinationFailEvent;
        public Action RoomCreateSuccessEvent, RoomJoinSuccessEvent;
        public Action<string> RoomCreateFailedEvent, RoomJoinFailedEvent;

        // scripts



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
                currentDestination = homeDestination;

                StartCoroutine(CheckLogin());
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
            // Example URL: DiscClash://{"region":1,"gameType":0,"game":0,"multiPlayerGameMode":1,"singlePlayerGameMode":0,
            // "lobbyName":"US_West","roomName":"DragonDen","isJoinable":true}

            Destination linkData = ParseDeepLink(url);
            if (linkData != null)
            {
                // Handle the parsed link data

                currentDestination = linkData;
            }

            StartCoroutine(CheckLogin());
        }

        public Destination ParseDeepLink(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;

            try
            {
                // 1. Strip the custom scheme
                string prefix = "DiscClash://";
                if (!url.StartsWith(prefix)) return null;

                string jsonPart = url.Substring(prefix.Length);

                // 2. Decode URL characters (e.g., %22 to ", %20 to space)
                string decodedJson = Uri.UnescapeDataString(jsonPart);

                // 3. Deserialize into your Destination class
                // Note: JsonUtility works if the JSON keys match field names exactly.
                // Newtonsoft.Json is more forgiving with formatting.
                Destination dest = JsonUtility.FromJson<Destination>(decodedJson);

                return dest;
            }
            catch (Exception e)
            {
                Debug.LogError($"Deep Link Parse Error: {e.Message}");
                return null;
            }
        }

        private IEnumerator CheckLogin()
        {
            yield return new WaitForSeconds(1); // Small delay to ensure all systems are initialized

            playerSettings.LoadSettings();
            if (!playerSettings.IsLoggedIn)
            {
                Debug.Log("Disc Clash: User not logged in. Redirecting to login.");
                uiInputData.ShowLogin();
            }
            else
            {
                Debug.Log("Disc Clash: User already logged in. Proceeding to destination.");

                if (Application.isEditor)
                {
                   cloudData.EditorLogin();
                }
                else
                {
                    cloudData.LoginToGoogle();
                }
            }
        }


        private void ConnectToDestination(Destination destination, Action OnConnected, Action OnFailed)
        {
            currentDestination = destination;
            uiOutputData.gameType = destination.gameType;


            OnDestinationSuccesEvent = OnConnected;
            OnDestinationFailEvent = OnFailed;

            if (destination.gameType == GameType.SinglePlayer)
            {
                uiOutputData.game = destination.game;
                uiOutputData.singlePlayerGameMode = destination.singlePlayerGameMode;


                OnDestinationSuccesEvent?.Invoke();
                uiInputData.StartSinglePlayerGame();


            }
            else if ((destination.gameType == GameType.OnlineMultiPlayer || destination.gameType == GameType.PlayWithFriends))
            {
                uiOutputData.game = destination.game;
                uiOutputData.multiPlayerGameMode = destination.multiPlayerGameMode;

                if (destination.roomName == "")
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
            else if (destination.gameType == GameType.Home)
            {
                OnDestinationSuccesEvent?.Invoke();
                uiInputData.GoToHome();

            }

        }
        private void RoomCreateSuccess()
        {

            currentDestination.lobbyName = networkOutput._runner.SessionInfo.Region;
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
}
                    
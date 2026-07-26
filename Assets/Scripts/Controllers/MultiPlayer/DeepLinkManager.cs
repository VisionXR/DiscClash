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
        public UIDataSO uiData;

        [Header("States")]
        public string loginState;
        public string homeState;

        // Action
        
        public Action OnDestinationSuccesEvent;
        public Action OnDestinationFailEvent;
        public Action RoomCreateSuccessEvent;
        public Action RoomJoinSuccessEvent;

        public Action<string> RoomCreateFailedEvent;
        public Action<string>  RoomJoinFailedEvent;


        // Game loop start here
        private void Awake()
        {
            Application.deepLinkActivated += OnDeepLinkActivated;

            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                OnDeepLinkActivated(Application.absoluteURL);
            }
            else
            {
                Debug.Log("Real Carrom 3D: No deep link detected on startup.");
                destinationData.currentDestination = destinationData.homeDestination;
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
        
            Destination linkData = ParseDeepLink(url);
            destinationData.currentDestination = linkData ?? destinationData.homeDestination;

            StartCoroutine(CheckLogin());
        }

        public Destination ParseDeepLink(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;

            try
            {
                string prefix = "DiscClash://";
                if (!url.StartsWith(prefix)) return null;

                string jsonPart = url.Substring(prefix.Length);
                string decodedJson = Uri.UnescapeDataString(jsonPart);
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
            yield return new WaitForSeconds(1);

            playerSettings.LoadSettings();

            if (PlayerPrefs.HasKey("Login"))
            {
                if (Application.isEditor)
                {
                    cloudData.EditorLogin();
                }
                else
                {
                    cloudData.LoginToGoogle();
                }

                Debug.Log("Real Carrom 3D : Login Found Going to Home.");
                            
            }
            else
            {
                Debug.Log("Real Carrom 3D : User not logged in. Redirecting to login.");
                uiData.uiManager.ChangeState(loginState, true);
                yield break;
            }

        }


        public void ConnectToDestination(Destination destination, Action OnConnected, Action OnFailed)
        {
            destinationData.currentDestination = destination;
          
            OnDestinationSuccesEvent = OnConnected;
            OnDestinationFailEvent = OnFailed;

            if (destination.gameType == GameType.VsCPU)
            {
                
                OnDestinationSuccesEvent?.Invoke();
                uiInputData.StartSinglePlayerGame();
            }
            else if ( destination.gameType == GameType.PlayWithFriends)
            {
               

                if (destination.roomName == "")
                {
                    string roomName = playerSettings.MyId.ToString();
                    networkInput.CreateRoom(playerSettings.serverRegion, roomName, RoomCreateSuccessEvent, RoomCreateFailedEvent);
                }
                else
                {
                    Debug.Log("Joining room in destination");
                    networkInput.JoinRoom(destination.GetRegion(), destinationData.currentDestination.roomName, RoomJoinSuccessEvent, RoomJoinFailedEvent);
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
            destinationData.currentDestination.lobbyName = networkOutput.runner.SessionInfo.Region;
            destinationData.currentDestination.roomName = networkOutput.runner.SessionInfo.Name;
            destinationData.currentDestination.region = playerSettings.serverRegion;

            uiInputData.StartMultiPlayerGame();
            destinationData.currentDestination.isJoinable = true;
            OnDestinationSuccesEvent?.Invoke();
        }

        private void RoomCreateFailed(string reason)
        {
            OnDestinationFailEvent?.Invoke();
        }

        public void RoomJoinSuccess()
        {
            destinationData.currentDestination.region = playerSettings.serverRegion;
            destinationData.currentDestination.lobbyName = networkOutput.runner.SessionInfo.Region;
            destinationData.currentDestination.roomName = networkOutput.runner.SessionInfo.Name;

            uiInputData.StartMultiPlayerGame();
            destinationData.currentDestination.isJoinable = false;
            OnDestinationSuccesEvent?.Invoke();
        }

        public void RoomJoinFailed(string reason)
        {
            OnDestinationFailEvent?.Invoke();
        }

        public void ProcessGameFlow()
        {         
            uiData.uiManager.GoToState(StateName.HomeState);
        }
    }
}
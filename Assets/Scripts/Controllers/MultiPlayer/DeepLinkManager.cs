using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
using Fusion;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

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
        public string tutorialState;

        [Header("Local Objects")]
        public GameObject tutorialManager;
        public Destination multiPlayerDestination;
        
        public DestinationPanelView destinationPanelView;
        public ChangeDestinationView changeDestinationPanelView;
        public TMP_Text errorText;
        public Sprite GuestPlayerIcon;
        public bool isLoggedIn = false;
        public bool isLink = false;
        private bool isFirstTime = true;

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

            isLink = true;
            string linkurl = ParseDeepLink(url);

            if (string.IsNullOrEmpty(linkurl))
            {
                return;
            }


            UrlLinkData newData = ConvertStringToLinkData(linkurl);
            uiOutputData.SetGameType(GameType.PlayWithFriends);

            string fullInput = newData.r;
            string actualRoomName = fullInput;
            ServerRegion targetRegion = ServerRegion.any;

            // Ensure the input has at least 2 characters before splitting
            if (!string.IsNullOrEmpty(fullInput) && fullInput.Length >= 2)
            {
                string regionCodeStr = fullInput.Substring(0, 2);
                actualRoomName = fullInput.Substring(2);

                // 2. Convert the 2-digit string to an integer
                if (int.TryParse(regionCodeStr, out int regionIndex))
                {
                    // 3. Explicitly cast the integer to your ServerRegion enum
                    // (Note: This assumes the parsed integer maps directly to your enum indexes)
                    targetRegion = (ServerRegion)regionIndex;
                }
                else
                {
                    Debug.LogWarning($"Could not parse '{regionCodeStr}' into an integer. Defaulting to 'any'.");
                }
            }
            else
            {
                Debug.LogWarning("Room code entered is too short! Using fallback handling.");
            }

            multiPlayerDestination.roomName = actualRoomName;
            multiPlayerDestination.region = targetRegion;
            multiPlayerDestination.multiPlayerGameMode = (MultiPlayerGameMode)(int.Parse(newData.g));
            multiPlayerDestination.time = newData.t;
       

            if (isFirstTime)
            {
                isFirstTime = false;
                StartCoroutine(CheckLogin());

            }
            else
            {

                changeDestinationPanelView.SetDestination(multiPlayerDestination);
                uiData.uiManager.GoToState(StateName.ChangeDestinationState);
                isLink = false;
            }
        }

        public string ParseDeepLink(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;

            try
            {
                string prefix = "realcarrom3d://";
                if (!url.StartsWith(prefix)) return null;

                string jsonPart = url.Substring(prefix.Length);


                return jsonPart;
            }
            catch (Exception e)
            {
                Debug.LogError($"Deep Link Parse Error: {e.Message}");
                return null;
            }
        }

        public UrlLinkData ConvertStringToLinkData(string queryString)
        {
            // Create a new instance of your class to populate
            UrlLinkData data = new UrlLinkData();

            // 1. Split the string by '&' to get each individual parameter pair
            string[] pairs = queryString.Split('&');

            foreach (string pair in pairs)
            {
                // 2. Split each pair by '=' to separate the key from the value
                string[] keyValue = pair.Split('=');

                // Ensure we actually have a valid key and value pair to avoid errors
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();

                    // 3. Match the key and assign the value to the correct class property
                    switch (key)
                    {
                        case "r":
                            data.r = value;
                            break;
                        case "g":
                            data.g = value;
                            break;
                        case "t":
                            data.t = value;
                            break;
                    }
                }
            }

            return data;
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
            OnDestinationSuccesEvent?.Invoke();
        }

        public void RoomJoinFailed(string reason)
        {
            OnDestinationFailEvent?.Invoke();
        }

        public void ProcessGameFlow()
        {
            if (!isLink)
            {

                isFirstTime = false;

                if (!PlayerPrefs.HasKey("Tutorial"))
                {
                    tutorialManager.SetActive(true);
                    uiData.uiManager.ChangeState("Tutorial", true);
                    uiData.uiManager.GoToState(StateName.Tutorial);
                    PlayerPrefs.SetString("Tutorial", "true");
                }
                else
                {

                    uiData.uiManager.GoToState(StateName.HomeState);
                }

            }
            else
            {

                destinationPanelView.SetDestination(multiPlayerDestination);
                uiData.uiManager.ChangeState("Link", true);            
                isLink = false;
            }
        }

    }
}


[Serializable]
public class UrlLinkData
{
    public string r;
    public string g;
    public string t;
}

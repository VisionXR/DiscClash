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
            // G04: Subscribe and unsubscribe the same failure handler.
            RoomCreateFailedEvent += RoomCreateFailed;

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

        // G04: Do not leave a stale deep-link callback after the scene is destroyed.
        private void OnDestroy() { Application.deepLinkActivated -= OnDeepLinkActivated; }

        private void OnDeepLinkActivated(string url)
        {
            UrlLinkData data;
            if (!TryParseInvite(url, out data))
            {
                // G04: Invalid cold-start links must still start the normal login flow.
                if (errorText != null) errorText.text = "Invalid invite link. Ask the host for a new link.";
                if (isFirstTime) { isFirstTime = false; isLink = false; StartCoroutine(CheckLogin()); }
                return;
            }
            // G04: Only validated values may change the selected game or destination.
            isLink = true;
            uiOutputData.SetGameType(GameType.PlayWithFriends);
            multiPlayerDestination.roomName = data.r.Substring(2);
            multiPlayerDestination.region = (ServerRegion)int.Parse(data.r.Substring(0, 2));
            multiPlayerDestination.multiPlayerGameMode = (MultiPlayerGameMode)int.Parse(data.g);
            multiPlayerDestination.challenge = (Challenge)int.Parse(data.c);
            multiPlayerDestination.time = data.t;
            if (isFirstTime) { isFirstTime = false; StartCoroutine(CheckLogin()); }
            else
            {
                changeDestinationPanelView.SetDestination(multiPlayerDestination);
                uiData.uiManager.GoToState(StateName.ChangeDestinationState);
                isLink = false;
            }
        }

        // G04: Validate room/region, supported mode, challenge and timestamp without throwing.
        public bool TryParseInvite(string url, out UrlLinkData data)
        {
            data = null;
            string query = ParseDeepLink(url);
            if (string.IsNullOrEmpty(query)) return false;
            try
            {
                data = ConvertStringToLinkData(query);
                int region, mode, challenge;
                if (string.IsNullOrEmpty(data.r) || data.r.Length != 7
                    || !System.Text.RegularExpressions.Regex.IsMatch(data.r, @"^\d{7}$")
                    || !int.TryParse(data.r.Substring(0, 2), out region)
                    || !Enum.IsDefined(typeof(ServerRegion), region)
                    || !int.TryParse(data.g, out mode) || !Enum.IsDefined(typeof(MultiPlayerGameMode), mode)
                    || mode == (int)MultiPlayerGameMode.P1P2vsP3P4
                    || !int.TryParse(data.c, out challenge) || !Enum.IsDefined(typeof(Challenge), challenge)
                    || challenge == (int)Challenge.TrickShots) return false;
                DateTime timestamp;
                return string.IsNullOrEmpty(data.t) || DateTime.TryParseExact(data.t, "yyyyMMddHHmm",
                    System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out timestamp);
            }
            catch (Exception) { data = null; return false; }
        }

        public string ParseDeepLink(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;

            try
            {
                string prefix = "realcarrom3d://";
                // G04: URL schemes are case-insensitive.
                if (!url.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) return null;

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
                    // G04: Decode values before numeric/date validation.
                    string value = Uri.UnescapeDataString(keyValue[1].Trim());

                    // 3. Match the key and assign the value to the correct class property
                    switch (key)
                    {
                        case "r":
                            data.r = value;
                            break;
                        case "c":
                            data.c = value;
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
                // G02: Offline sessions cannot enter online matchmaking; login is required first.
                if (cloudData.isOfflineSession)
                {
                    OnDestinationFailEvent?.Invoke();
                    if (errorText != null) errorText.text = "Log in to play online.";
                    return;
                }
               
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
    public string c;
    public string g;
    public string t;
}

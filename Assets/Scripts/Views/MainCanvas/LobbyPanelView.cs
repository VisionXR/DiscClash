using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Player = com.VisionXR.GameElements.Player;

namespace com.VisionXR.Views
{
    public class LobbyPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public PlayersDataSO playerData;
        public UIOutputDataSO uioutputData;
        public NetworkOutputSO networkOutPutData;
        public NetworkInputSO networkInputData;
        public UIDataSO uiData;
        public GameDataSO gameData;
        public MyPlayerSettings userData;

        [Header("Player Objects")]
        public Image player1Image;
        public Image player2Image;

        public TMP_Text player1Name;
        public TMP_Text player2Name;

        [Header("Local Objects")]
        public TMP_Text timeText;
        public TMP_Text roomNameText;

        public GameObject hostStartObject;
        public GameObject clientStartObject;

        public GameObject clientWaitingObject;
        public GameObject hostWaitingOBject;

        public TMP_Text clientNotReadyText;
        public TMP_Text hostNotReadyText;

        // local variables
        private Coroutine timeRoutine;
        private float elapsedTime = 0f; // Variable to store the elapsed time
        public bool isHostJoined = false;
        public bool isClientJoined = false;
        public string roomId;
        public string regionCode;



        private void OnEnable()
        {
            ResetTexts();
            ResetTime();
            // Start the coroutine when the GameObject becomes enabled
            // Ensure only one instance of the coroutine is running
            if (timeRoutine == null)
            {
                timeRoutine = StartCoroutine(ShowTime());
            }

            networkOutPutData.HostReadyEvent += ResetTime;
            networkOutPutData.ClientReadyEvent += ResetTime;

            roomId = networkOutPutData.runner.SessionInfo.Name;

            ServerRegion serverRegion = ServerRegion.any;

            if (Enum.TryParse<ServerRegion>(networkOutPutData.runner.SessionInfo.Region, ignoreCase: true, out serverRegion))
            {
                Debug.Log($"Successfully connected to region: {serverRegion}");
            }

            // Convert the enum int value to a padded 2-digit string
            regionCode = ((int)serverRegion).ToString("D2");
            roomNameText.text = "Room ID : " + regionCode + roomId;

        }

        private void OnDisable()
        {
            // Stop the coroutine when the GameObject becomes disabled
            // This prevents errors if the GameObject is destroyed or deactivated
            if (timeRoutine != null)
            {
                StopCoroutine(timeRoutine);
                timeRoutine = null;
            }

            networkOutPutData.HostReadyEvent -= ResetTime;
            networkOutPutData.ClientReadyEvent -= ResetTime;


            networkOutPutData.SetHostReady(false);
            networkOutPutData.SetClientReady(false);
            isHostJoined = false;
            isClientJoined = false;

            ResetTexts();
            ResetTime();
        }

        private IEnumerator ShowTime()
        {
            while (true)
            {
                // Increment the elapsed time by the time passed since the last frame
                elapsedTime += Time.deltaTime;

                // Calculate minutes and seconds
                int minutes = Mathf.FloorToInt(elapsedTime / 60);
                int seconds = Mathf.FloorToInt(elapsedTime % 60);

                // Format the time as "MM:SS" with leading zeros if necessary
                // "D2" ensures two digits, padding with a leading zero if the number is less than 10
                timeText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);



                Player mp = playerData.GetPlayer(1);
                if (mp != null && !isHostJoined)
                {

                    player1Image.sprite = mp.myImage;
                    player1Name.text = mp.myName;
                    isHostJoined = true;
                    ResetTime();
                }
                else if (mp != null)
                {
                    player1Image.sprite = mp.myImage;
                    player1Name.text = mp.myName;
                }
                else if (mp == null)
                {
                    
                    player1Name.text = "";
                    player2Name.text = "";
                    isHostJoined = false;
                }

                Player op;

                if(uioutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
                {
                    op = playerData.GetPlayer(2);
                }
                else if(uioutputData.multiPlayerGameMode == MultiPlayerGameMode.P1AIvsP2AI)
                {
                    op = playerData.GetPlayer(3);
                }
                else
                {
                    op = playerData.GetPlayer(2);
                }


                if (op != null && !isClientJoined)
                {

                    player2Image.sprite = op.myImage;
                    player2Name.text = op.myName;

                    isClientJoined = true;
                    ResetTime();
                }
                else if (op != null)
                {
                    player2Image.sprite = op.myImage;
                    player2Name.text = op.myName;

                }
                else if (op == null)
                {
                    player2Image.sprite = null;
                    player2Name.text = "";

                    isClientJoined = false;
                }

                if (isHostJoined && isClientJoined)
                {
                    if (networkOutPutData.IsHostReady() && networkOutPutData.IsClientReady())
                    {
                        if (networkOutPutData.isHost)
                        {

                            ShowHostStartButton();
                        }
                        else
                        {

                            ShowClientStartButton();
                        }
                    }
                    else if (networkOutPutData.IsHostReady() && !networkOutPutData.IsClientReady())
                    {
                        clientNotReadyText.gameObject.SetActive(true);
                    }
                    else if (!networkOutPutData.IsHostReady() && networkOutPutData.IsClientReady())
                    {
                        hostNotReadyText.gameObject.SetActive(true);
                    }
                }
                else if (isHostJoined && !isClientJoined)
                {

                    if (!clientWaitingObject.activeInHierarchy)
                    {
                        hostStartObject.SetActive(false);
                        clientStartObject.SetActive(false);
                        clientWaitingObject.SetActive(true);
                        hostWaitingOBject.SetActive(false);
                    }

                }
                else if (!isHostJoined && isClientJoined)
                {

                    if (!hostWaitingOBject.activeInHierarchy)
                    {
                        hostStartObject.SetActive(false);
                        clientStartObject.SetActive(false);
                        hostWaitingOBject.SetActive(true);
                    }
                }
                //else if (!isHostJoined && !isClientJoined)
                //{

                //    if (!hostWaitingOBject.activeInHierarchy)
                //    {
                //        hostStartObject.SetActive(false);
                //        clientStartObject.SetActive(false);
                //        hostWaitingOBject.SetActive(true);
                //        clientWaitingObject.SetActive(false);
                //    }
                //}

                // Wait for the next frame before continuing the loop
                yield return new WaitForEndOfFrame();
            }


        }

        // Optional: Method to reset the timer
        public void ResetTime()
        {

            hostStartObject.SetActive(false);
            clientStartObject.SetActive(false);

            hostWaitingOBject.SetActive(false);
            clientWaitingObject.gameObject.SetActive(false);


            hostNotReadyText.gameObject.SetActive(false);
            clientNotReadyText.gameObject.SetActive(false);
        }

        private void ResetTexts()
        {
            elapsedTime = 0f;
            timeText.text = "00:00"; // Reset display immediately        
            player1Image.sprite = null;
            player1Name.text = "";
            player2Image.sprite = null;
            player2Name.text = "";
        }

        //public void LaunchInvitePanel()
        //{

            

        //    string wixBaseUrl = "https://visionxr.co.in/carrompool360/join";


        //    // Escape Room ID to keep URL formatting valid
        //    string escapedRoomId = UnityWebRequest.EscapeURL(roomId);

        //    // Generates something like: 202606011805 (Year, Month, Day, Hour, Minute)
        //    string shortTime = DateTime.UtcNow.ToString("yyyyMMddHHmm");

        //    string shareUrl = $"{wixBaseUrl}?r={regionCode + roomId}&g={(int)userData.myCoins}&t={shortTime}";

        //    // Craft a clean message. Line breaks (\n) push the ugly link out of focus.
        //    // Messaging apps will read the URL at the bottom, build the OG card, and look clean.
        //    string inviteMessage = shareUrl;

        //    //     Debug.Log("Url is " + shareUrl);

        //    new NativeShare()
        //        .SetSubject("Carrom Pool 360 : Multiplayer Challenge")
        //        .SetText(inviteMessage)
        //        .Share();
        //}


        public void ShowHostStartButton()
        {

            ResetTime();
            hostStartObject.SetActive(true);
        }

        public void ShowClientStartButton()
        {
            ResetTime();
            clientStartObject.SetActive(true);
        }

        public void StartGameBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            PlayerNetworkData playerNetworkData = playerData.GetMainPlayer().GetComponent<PlayerNetworkData>();

            int id = 1;

            if (uioutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
            {
                if (gameData.firstTurnId == 1)
                {
                    id = 2;
                }
            }
            else if (uioutputData.multiPlayerGameMode == MultiPlayerGameMode.P1AIvsP2AI)
            {
                if (gameData.firstTurnId == 1)
                {
                    id = 3;
                }
            }

            playerNetworkData.RPC_StartGame(id, (int)uioutputData.MyCoinsId);
        }

        public void BackBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState("Pause", true);
        }
    }
}

[System.Serializable]
public class LinkRequest
{
    public string roomId;
    public MultiPlayerGameMode multiPlayerGameMode;
    public string region;
    public string startTime;
}

using UnityEngine;
using System;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace com.VisionXR.Views
{
    public class NoticationPanelView : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public NotificationDataSO notificationData;
        public UIOutputDataSO uiOutputData;
        public NetworkOutputSO networkOutputData;
        public MyPlayerSettings playerSettings;
        public GameDataSO gameData;

        [Header("Panel Objects")]
        public GameObject RoomJoinFailedPanel;
        public GameObject ExitGamePanel;
        public GameObject TwoPlayerPanel;
        public GameObject FourPlayerPanel;

        [Header("Game Objects")]
        public GameObject notificationPrefab;
        public GameObject uiBlocker;
        public Transform contentContainer;

        // local variables
        private TMP_Text currentAcceptBtnText;
        private NotificationMessage currentNotificationMessage;
        private Coroutine roomStatusRoutine;
        private bool isNewRoom = false;

        private void OnEnable()
        {
            notificationData.NotificationAcceptedEvent += OnNotificationAccepted;
            notificationData.NotificationRejectedEvent += DisplayNotificationList;

            DisplayNotificationList();
            uiBlocker.SetActive(false); 
        }

        private void OnDisable()
        {
            notificationData.NotificationAcceptedEvent -= OnNotificationAccepted;
            notificationData.NotificationRejectedEvent -= DisplayNotificationList;

            

        }

        private void RoomFailed(string obj)
        {
            if (roomStatusRoutine != null)
            {
                StopCoroutine(roomStatusRoutine);
                roomStatusRoutine = null;
                uiBlocker.SetActive(false);
            }

            // go to home here
            currentAcceptBtnText.text = "Accept";
            RoomJoinFailedPanel.SetActive(true);
            gameObject.SetActive(false);

        }

        private void RoomSuccess()
        {
            Debug.Log(" In notification room success");

            if (roomStatusRoutine != null)
            {
                StopCoroutine(roomStatusRoutine);
                roomStatusRoutine = null;
                uiBlocker.SetActive(false);
            }

            

            if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
            {
                TwoPlayerPanel.SetActive(true);
                
            }
            else
            {
                FourPlayerPanel.SetActive(true);
                
            }

            currentAcceptBtnText.text = "Accept";
            notificationData.NotificationAccepted();
         

        }

        public void YesButtonClicked()
        {
            if (isNewRoom)
            {
                isNewRoom = false;
                StartCoroutine(WaitAndJoinNewGame());
            }
          
        }

        private IEnumerator WaitAndJoinNewGame()
        {
            yield return new WaitUntil(() => networkOutputData._runner == null);  
            JoinNewGame(currentNotificationMessage, currentAcceptBtnText);
        }

        public void NoButtonClicked()
        {
            isNewRoom = false;         
        }

        private void OnNotificationAccepted()
        {
            gameObject.SetActive(false);
        }

        public void DisplayNotificationList()
        {
           
            // Clear existing notifications in the panel
            foreach (Transform child in contentContainer)
            {
                Destroy(child.gameObject);
            }

            // Create UI objects for each friend
            foreach (NotificationMessage message in notificationData.GetAllNotifications())
            {
                // Instantiate notification prefab
                GameObject notificationObjectprefab = Instantiate(notificationPrefab, contentContainer);
                NotificationObject notificationObject = notificationObjectprefab.GetComponent<NotificationObject>();

                // Set notification details
                notificationObject.NotificationMessage.text = message.playerName + " wants to play " + message.game.ToString()+" "+message.multiPlayerGameMode.ToString();
                notificationObject.NotificaionNo.text = (notificationData.GetAllNotifications().IndexOf(message) + 1).ToString();

                // Set accept button callback
                Button acceptBtn = notificationObject.AcceptBtn;

                acceptBtn.onClick.AddListener(() =>
                {
                    AcceptButtonClicked(message, notificationObject.AcceptBtnText);
                });

                // Set accept button callback
                Button declineBtn = notificationObject.DeclineBtn;

                declineBtn.onClick.AddListener(() =>
                {
                    DeclineButtonClicked(message);
                });

            }
        }

        public void OnCrossButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            gameObject.SetActive(false);
        }

        public void OnRefreshButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            DisplayNotificationList();
        }

        public void AcceptButtonClicked(NotificationMessage notificationMessage,TMP_Text buttonText)
        {
            AudioManager.instance.PlayButtonClickSound();
            currentAcceptBtnText = buttonText;
            currentNotificationMessage = notificationMessage;

            if (notificationMessage != null)
            {
                notificationData.RemoveNotification(notificationMessage);
                         

            }
        }

        public void DeclineButtonClicked(NotificationMessage notificationMessage)
        {
            AudioManager.instance.PlayButtonClickSound();
            if (notificationMessage != null)
            {

                notificationData.RemoveNotification(notificationMessage);
                notificationData.NotificationRejected();

            }
        }
        private IEnumerator ConnectingToRoom(TMP_Text roomName)
        {
            while (true)
            {
                roomName.text = "Connecting.";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting..";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting...";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting....";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting.....";
                yield return new WaitForSeconds(0.5f);
                roomName.text = "Connecting......";
            }
        }

        private void JoinNewGame(NotificationMessage notificationMessage, TMP_Text buttonText)
        {
         //   networkData.SetRegion(notificationMessage.region);
            
          //  uiData.SetRoomName(notificationMessage.roomName);
            playerSettings.SetBoard(notificationMessage.MyBoard);

       //     networkData.JoinRoom(notificationMessage.roomName);

            if (roomStatusRoutine == null)
            {
                roomStatusRoutine = StartCoroutine(ConnectingToRoom(buttonText));
                uiBlocker.SetActive(true);
            }
        }
    }
}



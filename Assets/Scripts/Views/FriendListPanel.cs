using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class FriendListPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public NetworkOutputSO networkOutputData;
        public UIOutputDataSO uiOutputData;
        public ChatDataSO chatData;
        public MyPlayerSettings myPlayerSettings;

        [Header("Panel Objects")]
        public GameObject PlayWithFriendPanel;
        public GameObject TwoPlayerPanel;
        public GameObject FourPlayerPanel;

        [Header("Game Objects ")]
        [SerializeField] private GameObject friendPrefab;
        [SerializeField] private Transform contentContainer;

        private void OnEnable()
        {
           
            myPlayerSettings.UserFriendsReceived += NewFriendsListReceived;
          
            DisplayFriendList(myPlayerSettings.GetMyFriends());
        }

        private void OnDisable()
        {
           
            myPlayerSettings.UserFriendsReceived -= NewFriendsListReceived;
        }

        private void NewFriendsListReceived(List<Friend> list)
        {
            DisplayFriendList(myPlayerSettings.GetMyFriends());
        }

        public void DisplayFriendList(List<Friend> friends)
        {
            // Clear existing friends in the panel
            foreach (Transform child in contentContainer)
            {
                Destroy(child.gameObject);
            }

            // Create UI objects for each friend
            foreach (Friend friend in friends)
            {
                // Instantiate friend prefab
                GameObject friendObjectprefab = Instantiate(friendPrefab, contentContainer);
                FriendObject friendObject = friendObjectprefab.GetComponent<FriendObject>();


                // Set friend name text
               
                // Set challenge button callback
                Button challengeButton = friendObject.sendChallengeButton;
                challengeButton.onClick.AddListener(() =>
                {
                    SendChallenge(friend.FriendID.ToString(), friendObject);
                });
            }
        }

        public void SendChallenge(string friendUserId, FriendObject friendObject = null)
        {
            AudioManager.instance.PlayButtonClickSound();
            // Send challenge logic

            NotificationMessage notificationMessage = new NotificationMessage();
            notificationMessage.region = myPlayerSettings.serverRegion ;
            notificationMessage.multiPlayerGameMode = uiOutputData.multiPlayerGameMode;
            notificationMessage.game = uiOutputData.game;
            notificationMessage.difficulty = uiOutputData.aIDifficulty;
            notificationMessage.MyBoard = myPlayerSettings.MyBoard;
            notificationMessage.roomName = myPlayerSettings.MyOculusId.ToString();
            notificationMessage.playerName = myPlayerSettings.MyName;
            chatData.SendPrivateMessage(friendUserId, JsonUtility.ToJson(notificationMessage));

            if (friendObject != null)
            {
                StartCoroutine(DisableChallengeButton(friendObject));
            }
        }

        private IEnumerator DisableChallengeButton(FriendObject friendObject)
        {
            friendObject.btnDisplayText.text = " Challenged ";
            friendObject.sendChallengeButton.enabled = false;
            yield return new WaitForSeconds(30);
            if (friendObject != null)
            {
                friendObject.btnDisplayText.text = "Send Challenge";
                friendObject.sendChallengeButton.enabled = true;
            }
        }

        private void OnPlayerJoined(PlayerRef @ref,bool isLocal)
        {
            if(uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
            {
                TwoPlayerPanel.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                FourPlayerPanel.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        public void OnBackButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
     
            PlayWithFriendPanel.SetActive(true );
            gameObject.SetActive(false);
        }

        public void RefreshButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            myPlayerSettings.GetFriendsEvent?.Invoke();
        }
    }
}

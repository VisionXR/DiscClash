using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;

namespace com.VisionXR.Views
{
    public class FriendsListPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects ")]
        public UIOutputDataSO uiOutputData;
        public ChatDataSO chatData;
        public MyPlayerSettings myPlayerSettings;

        [Header("Game Objects ")]
        [SerializeField] private GameObject friendPrefab;
        [SerializeField] private Transform contentContainer;
        


        private void OnEnable()
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
                friendObject.friendName.text = friend.FriendName.ToString();
                friendObject.friendNo.text = (friends.IndexOf(friend) + 1).ToString();
             
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
        //    notificationMessage.region = 
            notificationMessage.multiPlayerGameMode = uiOutputData.multiPlayerGameMode;
            notificationMessage.game = uiOutputData.game;
            notificationMessage.difficulty = uiOutputData.aIDifficulty;
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
            yield return new WaitForSeconds(60);
            friendObject.btnDisplayText.text = "Send Challenge";
            friendObject.sendChallengeButton.enabled = true;
        }
    }
}



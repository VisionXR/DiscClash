using UnityEngine;  
using Photon.Chat;
using ExitGames.Client.Photon;
using System;
using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;
using System.Collections;

namespace com.VisionXR.Controllers
{
    public class ChatManager : MonoBehaviour, IChatClientListener
    {

        [Header("Scriptable Objects")]
        private ChatClient chatClient;
        public ChatDataSO chatData;
        public NotificationDataSO notificationData;
        public MyPlayerSettings playerSettings;

        [Header("Local variables")]
        public bool isConnected = false;
        public string chatAppID = "YOUR_PHOTON_CHAT_APP_ID";
        public string chatUserName;
        public string defaultChannelName = "RealCarrom";
       

        void OnEnable()
        {
            playerSettings.UserDataReceived += ConnectToPhotonChat;
            chatData.SendPrivateMessageEvent += SendPrivateMessage;
           
        }
        void OnDisable()
        {
            chatData.SendPrivateMessageEvent -= SendPrivateMessage;
            playerSettings.UserDataReceived -= ConnectToPhotonChat;          
        }

        private void ConnectToPhotonChat(ulong id)
        {

            Debug.Log(" Connecting to chat");

            chatUserName = id.ToString();

            chatClient = new ChatClient(this);

            StartCoroutine(ConnectRoutine());

        }



        private IEnumerator ConnectRoutine()
        {
            yield return new WaitForSeconds(2);

            chatClient.Connect(chatAppID, "1.0", new AuthenticationValues(chatUserName));                       
            
        }

        // Implement other methods...

        public void OnPrivateMessage(string sender, object message, string channelName) // Add this method
        {
            if ((string)sender != chatUserName) // if sender is not me then only show
            {
              
                notificationData.NotificationReceived(JsonUtility.FromJson<NotificationMessage>((string)message));
            }
        }
        public void SendPrivateMessage(string receiver, string message)
        {
            bool isSent = false;

            if (chatClient.State == ChatState.ConnectedToFrontEnd)
            {
                isSent = chatClient.SendPrivateMessage(receiver, message);

                if (isSent)
                {
                    Debug.Log("Message successfully queued for sending to " + receiver);
                }
                else
                {
                    Debug.LogWarning("Failed to queue message for sending to " + receiver);
                }
            }
            else
            {
                Debug.LogWarning("Cannot send message. Not connected to Photon Chat.");
            }

            
        }


        // Implement other methods...

        private void Update()
        {
            // Update the chat client
            if (chatClient != null)
            {
                chatClient.Service();
               
            }

        }

        void IChatClientListener.DebugReturn(DebugLevel level, string message)
        {
            Debug.Log("Chat debug " + message);
        }


        void IChatClientListener.OnChatStateChange(ChatState state)
        {
            if(state== ChatState.ConnectedToFrontEnd)
            {
                
            }
            else if(state == ChatState.Disconnected)
            {
                isConnected = false;
            }
        }

        void IChatClientListener.OnGetMessages(string channelName, string[] senders, object[] messages)
        {

        }

        void IChatClientListener.OnSubscribed(string[] channels, bool[] results)
        {

        }

        void IChatClientListener.OnUnsubscribed(string[] channels)
        {

        }

        void IChatClientListener.OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {

        }

        void IChatClientListener.OnUserSubscribed(string channel, string user)
        {

        }

        void IChatClientListener.OnUserUnsubscribed(string channel, string user)
        {

        }

        public void GetFriendStatus(string friendId)
        {
          
        }

        // Callback for friend status updates
        public void OnFriendStatusUpdate(string userId, int status, bool gotMessage, object message)
        {

        }

        private void OnDestroy()
        {
            // Disconnect from Photon Chat
            if (chatClient != null)
            {
                chatClient.Disconnect();
            }

        }

        public void OnDisconnected()
        {
             isConnected = false;
             StartCoroutine(ConnectRoutine());
        }

        public void OnConnected()
        {
            isConnected = true;
        }
    }
}

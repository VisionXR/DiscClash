using com.VisionXR.HelperClasses;
using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "ChatDataSO", menuName = "ScriptableObjects/ChatDataSO", order = 1)]
    public class ChatDataSO : ScriptableObject
    {
        // variables
        public bool isConnectedToChat = false;

        // events
        public Action ConnectToChatServerEvent;
        public Action<string,string> PrivateMessageReceivedEvent;
        public Action<string,string> SendPrivateMessageEvent;


        // methods

        private void OnEnable()
        {
            isConnectedToChat = false;
        }
        public void ConnectToChatServer()
        {
            ConnectToChatServerEvent?.Invoke();
        }

        public void MessageReceived(string sender,string message)
        {
            PrivateMessageReceivedEvent?.Invoke(sender, message);
        }

        public void SendPrivateMessage(string receiver,string message)
        {        
            SendPrivateMessageEvent?.Invoke(receiver, message);           
        }
    }
}

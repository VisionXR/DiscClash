using com.VisionXR.HelperClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "NotificationDataSO", menuName = "ScriptableObjects/NotificationDataSO", order = 1)]
    public class NotificationDataSO : ScriptableObject
    {
        // variables
        public List<NotificationMessage> AllNotifications = new List<NotificationMessage>();
       

        // Actions
        public Action<NotificationMessage> NotificationReceivedEvent;
        public Action NotificationAcceptedEvent;
        public Action NotificationRejectedEvent;

        // Methods

        private void OnEnable()
        {
            AllNotifications.Clear();
        }

        public void AddNotification(NotificationMessage message)
        {
            AllNotifications.Add(message);
           
        }

        public void RemoveNotification(NotificationMessage message)
        {
            if(AllNotifications.Contains(message))
            {
                AllNotifications.Remove(message);
            }
        }

        public void NotificationReceived(NotificationMessage message)
        {
            NotificationReceivedEvent?.Invoke(message);
        }

        public List<NotificationMessage> GetAllNotifications()
        {
            return AllNotifications;
        }

        public void NotificationAccepted()
        {
            NotificationAcceptedEvent?.Invoke();
        }

        public void NotificationRejected()
        {
            NotificationRejectedEvent?.Invoke();
        }
    }
}

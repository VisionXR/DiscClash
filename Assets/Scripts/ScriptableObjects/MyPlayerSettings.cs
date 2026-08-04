using com.VisionXR.HelperClasses;
using System;
using UnityEngine;

 namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "MyPlayerSettings", menuName = "ScriptableObjects/MyPlayerSettings", order = 1)]
    public class MyPlayerSettings : ScriptableObject
    {
        // Player Data
        public LoginType loginType;
     
        public string MyName;
        public Sprite MyProfileImage;
        public string ImageUrl;
        public string MyId;

        public ServerRegion serverRegion;
        public DominantHand myDominantHand;
        public bool isHapticsEnabled;

        // Events
        public Action<string> UserDataReceived;
        
        public Action<Sprite> UserProfileImageReceived;        
        public Action<ServerRegion> ServerRegionChangedEvent;
        public Action SaveSettingsEvent;
        public Action LoadSettingsEvent;
        public Action DeleteAccountEvent;


        public Action<int> BoardChangedEvent;
        public Action<int> StrikerChangedEvent;
        public Action<int> CoinsChangedEvent;


        private void OnEnable()
        {
          
            isHapticsEnabled = true;
        }

        public void SetHapticsEnabled(bool status)
        {
            isHapticsEnabled = status;
        }

        public void SetDominantHand(DominantHand hand)
        {
            myDominantHand = hand;
        }

        public void SetLoginType(LoginType type)
        {
            loginType = type;
        }
        public void SetUserNameAndId(string userName, string Id)
        {
            MyName = userName;
            MyId = Id;
            UserDataReceived?.Invoke(MyId);
        }


        public void SetUserProfileImageUrl(string url)
        {
            ImageUrl = url;
            
        }

        public void SetUserProfileImage(Sprite s)
        {
            MyProfileImage = s;

        }

        public void SaveSettings()
        {
            SaveSettingsEvent?.Invoke();
        }

        public void LoadSettings()
        {
            LoadSettingsEvent?.Invoke();
        }

        public void DeleteAccount()
        {
            DeleteAccountEvent?.Invoke();
        }
    }
}

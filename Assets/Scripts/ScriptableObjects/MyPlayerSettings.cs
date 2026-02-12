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
        public bool IsLoggedIn;
        public int MyStrikerId;
        public int MyBoard;
        public int MyCoinsId;
        public int MyAvatar;
        public string MyName;
        public Sprite MyProfileImage;
        public string ImageUrl;
        public string MyId;
        public int MyCoins;
        public int MyPoints;
        public ServerRegion serverRegion;
       

        // Events
        public Action<string> UserDataReceived;
        
        public Action<Sprite> UserProfileImageReceived;        
        public Action<ServerRegion> ServerRegionChangedEvent;
        public Action SaveSettingsEvent;
        public Action LoadSettingsEvent;
        

        public Action<int> BoardChangedEvent;
        public Action<int> StrikerChangedEvent;
        public Action<int> CoinsChangedEvent;


        private void OnEnable()
        {
            IsLoggedIn = false;
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

        public void SetBoard(int id)
        {
            MyBoard = id;
            BoardChangedEvent?.Invoke(id);
        }

        public void SetStriker(int id)
        {
            MyStrikerId = id;
            StrikerChangedEvent?.Invoke(id);
        }

        public void SetCoins(int id)
        {
            MyCoinsId = id;
            CoinsChangedEvent?.Invoke(id);
        }

        public void SetUserProfileImage(Sprite s)
        {
            MyProfileImage = s;

        }

        public void SetServerRegion(ServerRegion region)
        {
            serverRegion = region;
            ServerRegionChangedEvent?.Invoke(serverRegion);
        }

        public void SetLogIn(bool status)
        {
            IsLoggedIn = status;
        }

        public void SaveSettings()
        {
            SaveSettingsEvent?.Invoke();
        }

        public void LoadSettings()
        {
            LoadSettingsEvent?.Invoke();
        }
    }
}

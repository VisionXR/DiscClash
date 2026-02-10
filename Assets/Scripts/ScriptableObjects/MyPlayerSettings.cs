using com.VisionXR.HelperClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

 namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "MyPlayerSettings", menuName = "ScriptableObjects/MyPlayerSettings", order = 1)]
    public class MyPlayerSettings : ScriptableObject
    {
        // Player Data
        public int MyStrikerId;
        public int MyBoard;
        public int MyCoinsId;
        public int MyArena;
        public int MyAvatar;
        public string MyName;
        public Sprite MyProfileImage;
        public string ImageUrl;
        public string MyOculusId;
        public int MyCoins;
        public int MyPoints;
      
      
        public List<Friend> MyFriends = new List<Friend>();
        public ServerRegion serverRegion;
       

        // Events
        public Action<string> UserDataReceived;
        
        public Action<Sprite> UserProfileImageReceived;
        public Action<List<Friend>> UserFriendsReceived;
        public Action<ServerRegion> ServerRegionChangedEvent;
        public Action SaveSettingsEvent;
        public Action LoadSettingsEvent;
        
        public Action GetFriendsEvent;
        public Action<int> BoardChangedEvent;
        public Action<int> StrikerChangedEvent;
        public Action<int> CoinsChangedEvent;


        public void SetUserNameAndId(string userName, string Id)
        {
            MyName = userName;
            MyOculusId = Id;
            UserDataReceived?.Invoke(MyOculusId);
        }

        public void SetMyName(string Name)
        {
            MyName = Name;
        }

        public void SetMyPoints(int points)
        {
            MyPoints = points;
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


        public void SetProfileUrl(string url)
        {
            ImageUrl = url;

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



        public string GetMyName()
        {
            return MyName;
        }
        public int GeyMyPoints()
        {
            return MyPoints;
        }
        public string GetMyImage()
        {
            return ImageUrl;
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

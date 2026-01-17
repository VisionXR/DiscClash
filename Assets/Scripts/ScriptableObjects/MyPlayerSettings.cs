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
        public ulong MyOculusId;
        public int MyCoins;
        public int MyPoints;
        public bool isPassThrough = false;
        public DominantHand myDominantHand;
        public List<Friend> MyFriends = new List<Friend>();
        public ServerRegion serverRegion;
        public Device device;

        // Events
        public Action<ulong> UserDataReceived;
        public Action<DominantHand> DominantHandChanged;
        public Action<Sprite> UserProfileImageReceived;
        public Action<List<Friend>> UserFriendsReceived;
        public Action<ServerRegion> ServerRegionChangedEvent;
        public Action SaveSettingsEvent;
        public Action LoadSettingsEvent;
        public Action PassThroughChangedEvent;
        public Action GetFriendsEvent;
        public Action<int> BoardChangedEvent;
        public Action<int> StrikerChangedEvent;
        public Action<int> CoinsChangedEvent;


        public void SetMyName(string Name)
        {
            MyName = Name;
        }

        public string GetMyName()
        {
            return MyName;
        }

        public void SetMyPoints(int points)
        {
            MyPoints = points;
        }

        public int GeyMyPoints()
        {
            return MyPoints;
        }
        public void SetUserNameAndId(string userName, ulong Id)
        {
            MyName = userName;
            MyOculusId = Id;
            UserDataReceived?.Invoke(MyOculusId);
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


        public void SetPassThrough(bool value)
        {
         
            isPassThrough = value;
            PassThroughChangedEvent?.Invoke();
        }
        public void SetProfileUrl(string url)
        {
            ImageUrl = url;
          
        }
        public void SetUserProfileImage(Sprite s)
        {
            MyProfileImage = s;
           
        }

        public string GetMyImage()
        {
            return ImageUrl;
        }

        public static string SpriteToString(Sprite sprite)
        {
            Texture2D texture = sprite.texture;
            byte[] textureData = texture.EncodeToPNG(); // Encode the texture data to PNG format

            // Convert the byte array to Base64 string
            string base64String = Convert.ToBase64String(textureData);

            // Optionally, you can include additional metadata such as sprite dimensions, format, etc.
            string metadata = string.Format("{0},{1},{2}", texture.width, texture.height, "PNG");

            // Combine metadata and texture data string
            string combinedString = metadata + "|" + base64String;

            return combinedString;
        }

        public static Sprite StringToSprite(string data)
        {
            // Split the combined string into metadata and texture data parts
            string[] parts = data.Split('|');
            string metadata = parts[0];
            string base64String = parts[1];

            // Extract metadata
            string[] metadataParts = metadata.Split(',');
            int width = int.Parse(metadataParts[0]);
            int height = int.Parse(metadataParts[1]);
            string format = metadataParts[2];

            // Convert Base64 string back to byte array
            byte[] textureData = Convert.FromBase64String(base64String);

            // Create a new texture and load the texture data
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(textureData);

            // Create a sprite from the texture
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.one * 0.5f);

            return sprite;
        }

 

        public void SetServerRegion(ServerRegion region)
        {
            serverRegion = region;
            ServerRegionChangedEvent?.Invoke(serverRegion);
        }

        public void SetDominantHand(DominantHand hand)
        {
            myDominantHand = hand;
           
        }

        public void ChangeHand(DominantHand hand)
        {
            DominantHandChanged?.Invoke(hand);
        }
        public List<Friend> GetMyFriends()
        {
            return MyFriends;
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

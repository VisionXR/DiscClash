using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.IO;
using UnityEngine;

namespace com.VisionXR.Controller
{
    public class SaveAndLoadManager : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public MyPlayerSettings playerSettings;

        [Header(" Local Objects")]
        public string Key = "DiscClash";
        public PlayerData data;


        private void OnEnable()
        {
            playerSettings.SaveSettingsEvent += SaveData;
            playerSettings.LoadSettingsEvent += LoadData;
        }

        private void OnDisable()
        {
            playerSettings.SaveSettingsEvent -= SaveData;
            playerSettings.LoadSettingsEvent -= LoadData;
        }

        private void SaveData()
        {
            PlayerData newPlayerData = new PlayerData();
        
            newPlayerData.BoardId = playerSettings.MyBoard;
            newPlayerData.StrikerId = playerSettings.MyStrikerId;
            newPlayerData.CoinsId = playerSettings.MyCoins;
            newPlayerData.region = playerSettings.serverRegion;
            newPlayerData.isLoggedIn = playerSettings.IsLoggedIn;

            SaveData(Key, JsonUtility.ToJson(newPlayerData));
        }

        private void LoadData()
        {
            string playerData = LoadData(Key);

            if(!string.IsNullOrEmpty(playerData))
            {
              
                try
                {
                    data = JsonUtility.FromJson<PlayerData>(playerData);
                  
                    playerSettings.SetBoard(data.BoardId);
                    playerSettings.SetStriker(data.StrikerId);
                    playerSettings.SetCoins(data.CoinsId);
                    playerSettings.SetServerRegion(data.region);
                    playerSettings.SetLogIn(data.isLoggedIn);

                }
                catch (Exception e)
                {
                    Debug.Log(" Something wrong with loading data");
                }
            }

          

        }

        public void SaveData(string fileName, string data)
        {
            // 1. Define the full path
            // Path.Combine handles the slashes (/) correctly for Windows, Mac, iOS, or Android
            string path = Path.Combine(Application.persistentDataPath, fileName + ".txt");

            // 3. Write the string to the file
            File.WriteAllText(path, data);

          
        }

        public string LoadData(string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName + ".txt");

            if (File.Exists(path))
            {
               
                string json = File.ReadAllText(path);
                return json;
            }
            else
            {
                Debug.Log("Save file not found.");
                return "";
            }
        }
    }
}

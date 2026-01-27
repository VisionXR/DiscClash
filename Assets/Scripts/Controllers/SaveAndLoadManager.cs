using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace com.VisionXR.Controller
{
    public class SaveAndLoadManager : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public MyPlayerSettings playerSettings;
        public BoardDataSO boardData;

        [Header(" Local Objects")]
        public string Key = "RealCarromData";
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
            newPlayerData.dominantHand = playerSettings.myDominantHand;
            newPlayerData.BoardId = playerSettings.MyBoard;
            newPlayerData.StrikerId = playerSettings.MyStrikerId;
            newPlayerData.region = playerSettings.serverRegion;
            newPlayerData.isPassThroughOn = playerSettings.isPassThrough;


            //  PlayerPrefs.SetString(Key,JsonUtility.ToJson(newPlayerData));
            SaveData(Key, JsonUtility.ToJson(newPlayerData));
        }

        private void LoadData()
        {
            string playerData = LoadData(Key);

            if(playerData != "")
            {
              
                try
                {
                    data = JsonUtility.FromJson<PlayerData>(playerData);
                    playerSettings.SetDominantHand(data.dominantHand);
                    playerSettings.SetBoard(data.BoardId);
                    playerSettings.SetStriker(data.StrikerId);
                    playerSettings.SetServerRegion(data.region);
                    playerSettings.SetPassThrough(data.isPassThroughOn);
                }
                catch (Exception e)
                {
                    Debug.Log(" Something wrong with loading data");
                }
            }

            if(PlayerPrefs.HasKey(Key))
            {
                
                string playerDataString = PlayerPrefs.GetString(Key);
                try
                {
                    data = JsonUtility.FromJson<PlayerData>(playerDataString);
                    playerSettings.SetDominantHand(data.dominantHand);
                    playerSettings.SetBoard(data.BoardId);
                    playerSettings.SetStriker(data.StrikerId);
                    playerSettings.SetServerRegion(data.region);
                    playerSettings.SetPassThrough(data.isPassThroughOn);
                }
                catch(Exception e)
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

            // 2. Convert your data object to a JSON string


            // 3. Write the string to the file
            File.WriteAllText(path, data);

            Debug.Log($"Data saved to: {path}");
        }

        public string LoadData(string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName + ".txt");

           

            if (File.Exists(path))
            {
                Debug.Log("Loading data from " + path);
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

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "TrickShotsDataSO", menuName = "ScriptableObjects/TrickShotsDataSO", order = 1)]
    public class TrickShotsDataSO : ScriptableObject
    {
        [Serializable]
        public class LevelEntry
        {
            public int levelNo;      // 0-based
            public bool unlocked;
            public int bestStars;    // 0..3
        }

        [Serializable]
        private class ProgressData
        {
            public int lastPlayedLevel;            // 0-based
            public List<LevelEntry> levels = new();
        }

        private const string PREFS_KEY = "TrickShot_Progress_v1";

        [Header("Config")]
        [Tooltip("Total Trick Shot levels (0..totalLevels-1)")]
        public int totalLevels = 50;

        public int currentLevelNo;      // 0-based
        public float currentLevelTime;

        public Action<int> LoadTrickShotLevelEvent;
        public Action<int> LevelSuccessEvent;
        public Action LevelFailEvent;

        [SerializeField] private ProgressData progress = new();

        // ---------- Public API ----------

        private void OnDisable()
        {
            SaveProgress();
        }

        public void InitOrLoadProgress()
        {
            if (!LoadProgress())
            {
                progress = new ProgressData();
                // Create 0..totalLevels-1; unlock level 0
                for (int i = 0; i < Mathf.Max(1, totalLevels); i++)
                {
                    progress.levels.Add(new LevelEntry
                    {
                        levelNo = i,
                        unlocked = (i == 0),
                        bestStars = 0
                    });
                }
                progress.lastPlayedLevel = 0;
                SaveProgress();
            }

            // Expand if total increased (0-based)
            if (progress.levels.Count < totalLevels)
            {
                for (int i = progress.levels.Count; i < totalLevels; i++)
                {
                    progress.levels.Add(new LevelEntry
                    {
                        levelNo = i,
                        unlocked = false,
                        bestStars = 0
                    });
                }
                SaveProgress();
            }

            // Clamp last played
            progress.lastPlayedLevel = Mathf.Clamp(progress.lastPlayedLevel, 0, Mathf.Max(0, totalLevels - 1));
        }

        public bool IsUnlocked(int level)
        {
            var e = progress.levels.Find(le => le.levelNo == level);
          
            return e != null && e.unlocked;
        }

        public int GetBestStars(int level)      
        {
            var e = progress.levels.Find(le => le.levelNo == level);
            return e != null ? e.bestStars : 0;
        }

        public int GetLastPlayedLevel() => progress.lastPlayedLevel;

        public void SetCurrentLevelTime(float time) => currentLevelTime = time;

        public void LoadTrickShotLevel(int level)
        {
            currentLevelNo = Mathf.Clamp(level, 0, Mathf.Max(0, totalLevels - 1));
            progress.lastPlayedLevel = currentLevelNo;
            SaveProgress();
            LoadTrickShotLevelEvent?.Invoke(currentLevelNo);
        }

        public void LevelSuccess(int stars)
        {
            stars = Mathf.Clamp(stars, 0, 3);

            var e = progress.levels.Find(le => le.levelNo == currentLevelNo);
            if (e != null) e.bestStars = Mathf.Max(e.bestStars, stars);

            // Unlock next if within range
            int next = currentLevelNo + 1;
            if (next < totalLevels)
            {
                var n = progress.levels.Find(le => le.levelNo == next);
                if (n != null) n.unlocked = true;
            }

            SaveProgress();
            LevelSuccessEvent?.Invoke(stars);
        }

        public void LevelFail() => LevelFailEvent?.Invoke();

        // ---------- Persistence ----------

        private bool LoadProgress()
        {
          

            try
            {
                string newJson = LoadData(PREFS_KEY);

                var loadednew = JsonUtility.FromJson<ProgressData>(newJson);
                if (loadednew == null || loadednew.levels == null || loadednew.levels.Count == 0)
                {
                    string json = PlayerPrefs.GetString(PREFS_KEY, "");
                    if (string.IsNullOrEmpty(json)) return false;

                    var loaded = JsonUtility.FromJson<ProgressData>(json);
                    if (loaded == null || loaded.levels == null || loaded.levels.Count == 0) return false;
                    progress = loaded;
                    return true;
                }
                progress = loadednew;
                return true;
              
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[TrickShotsDataSO] LoadProgress failed: {ex.Message}");
                return false;
            }
        }

        public void SaveProgress()
        {
            try
            {
                string json = JsonUtility.ToJson(progress);
                SaveData(PREFS_KEY, json);
                //PlayerPrefs.SetString(PREFS_KEY, json);
                //PlayerPrefs.Save();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[TrickShotsDataSO] SaveProgress failed: {ex.Message}");
            }
        }

        public void ResetAllProgress()
        {
            PlayerPrefs.DeleteKey(PREFS_KEY);
            InitOrLoadProgress();
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

            Debug.Log(" Fetching data from " + fileName);

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

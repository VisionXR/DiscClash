using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "AchievementsDataSO", menuName = "ScriptableObjects/AchievementsDataSO", order = 1)]
    public class AchievementsDataSO : ScriptableObject
    {
        // Variables
        public List<AchievementInfo> AllAchievementInfo;


        // Actions
        public Action GetAllAchievementsEvent;
        public Action<string> UnlockAchievementEvent;
        public Action SinglePlayerGameWonEvent;
        public Action MultiPlayerGameWonEvent;
        public Action MultiPlayerGameStartEvent;


        public void MultiPlayerGameStart()
        {
            MultiPlayerGameStartEvent?.Invoke();
        }

        public void MultiPlayerGameWOn()
        {
            MultiPlayerGameWonEvent?.Invoke();
        }

        public void SinglePlayerWon()
        {
            SinglePlayerGameWonEvent?.Invoke();
        }
        public void GetAllAchievemnets()
        {
            GetAllAchievementsEvent?.Invoke();
        }

        public void UnlockAchievement(string apiName)
        {
           UnlockAchievementEvent?.Invoke(apiName);
        }

        public string[] GetAPINames()
        {
            string[] result = new string[AllAchievementInfo.Count];
            for (int i = 0; i < AllAchievementInfo.Count; i++)
            {
                result[i] = AllAchievementInfo[i].apiName;
                Debug.Log("API Name: " + result[i]);
            }
            return result;
        }


        

        public bool isAchievementUnlocked(string apiName)
        {
            foreach (AchievementInfo info in AllAchievementInfo)
            {
                if (info.apiName == apiName)
                {
                    return info.isAchieved;
                }
            }
            return false;
        }
    }

    [Serializable]

    public class AchievementInfo
    {
        public string apiName;
        public string achievementName;
        public string description;
        public bool isAchieved;
    }
}

using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;
using GooglePlayGames;
using System.Collections;


public class AchievementManager : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public AchievementsDataSO achievementData;
    public UIDataSO uiData;
    public GameDataSO gameData;
    public CloudDataSO cloudData;

    [Header("Local Objects")]
    public AudioSource achievementAS;



    // local variables
    private Coroutine unlockRoutine;

    private void OnEnable()
    {
      
        achievementData.GetAllAchievementsEvent += GetAllAchievements;
        //gameData.StartGameEvent += GameStarted;
        //gameData.GameCompletedEvent += GameCompleted;
        //achievementData.UserLoggedInEvent += AddLogin;


    }

    private void OnDisable()
    {
        achievementData.GetAllAchievementsEvent -= GetAllAchievements;
        //gameData.StartGameEvent -= GameStarted;
        //gameData.GameCompletedEvent -= GameCompleted;
        //achievementData.UserLoggedInEvent -= AddLogin;
    }

    public void GetAllAchievements()
    {
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Debug.LogWarning("User is not authenticated with Google Play Games Services");
            return;
        }

        PlayGamesPlatform.Instance.LoadAchievements((achievements) =>
        {
            if (achievements == null)
            {
                Debug.LogError("Failed to load achievements from Google Play Games Services");
                return;
            }

            foreach (var achievement in achievements)
            {
                // Use achievement.id to query your local config
                AchievementInfo info = achievementData.GetAchievementByApiId(achievement.id);

                if (info != null)
                {
                    // Calculate actual step progress locally if it's an incremental achievement
                    int calculatedProgress = 0;

                    if (info.achievementType == AchievementType.Progess && info.target > 0)
                    {
                        // Convert Google's server percentage back into your local step count
                        calculatedProgress = Mathf.RoundToInt(((float)achievement.percentCompleted / 100f) * info.target);
                    }
                    else
                    {
                        // For standard achievements, percentCompleted is either 0 or 100
                        calculatedProgress = achievement.completed ? info.target : 0;
                    }

                    // Update your local cache with the true step progress
                    achievementData.UpdateLocalProgress(achievement.id, calculatedProgress);

                    // Check if fully unlocked
                    if (achievement.completed)
                    {
                        achievementData.UnLockLocal(achievement.id);
                    }

                   // Debug.Log($"Progress synced for {info.name}: Server reporting {achievement.percentCompleted}%. Local step calculation: {calculatedProgress}/{info.target}");
                }
            }
        });
    }

    public void GameStarted()
    {
       
     

        SaveUserData();
    }

    public void GameCompleted(int id)
    {
       
               
           

      
           
        
    }

    private IEnumerator UnLockWin()
    {
        SetTotalWins();
        SaveUserData();
        yield return StartCoroutine(UnLockWinAchievements());
        yield return StartCoroutine(UnLockBoardAchievements());
        yield return StartCoroutine(UnLockOverallAchievements());
    }


    /// <summary>
    /// Instantly unlocks a standard, one-time achievement.
    /// </summary>
    /// <param name="achievementId">The exact alphanumeric string ID from the Google Play Console</param>
    public void UnlockSimpleAchievement(AchievementInfo info)
    {

        string achievementId = info.apiName;
    //    Debug.Log("Trying to unlock" + info.name);

        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Debug.LogWarning($"[Achievements] User not authenticated. Cannot unlock ID: {achievementId}");
            return;
        }

        // Passing 100.0 instantly triggers a full unlock for standard achievements
        Social.ReportProgress(achievementId, 100.0, (bool success) =>
        {
            if (success)
            {
              //  Debug.Log($"[Achievements] Successfully unlocked standard achievement: {achievementId}");

                // Mark it unlocked in your local ScriptableObject
                achievementData.UnLockLocal(achievementId);

                // Play your sound effect cleanly if a source is set
                if (achievementAS != null && !achievementAS.isPlaying)
                {
                    achievementAS.Play();
                }
            }
            else
            {
                Debug.LogError($"[Achievements] Failed to unlock standard achievement: {achievementId}");
            }
        });
    }

    /// <summary>
    /// Updates an incremental achievement directly to your current absolute count 
    /// and checks if it has been fully unlocked.
    /// </summary>
    /// <param name="achievementId">The exact alphanumeric string ID from the Google Play Console</param>
    /// <param name="currentCount">The current absolute total step count</param>
    /// <param name="targetCount">The unlock threshold for this achievement (e.g., 10 for 10 wins)</param>
    public void UpdateIncrementalAchievement(AchievementInfo info, int currentCount, int targetCount)
    {
        string achievementId = info.apiName;

        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Debug.LogWarning($"[Achievements] User not authenticated. Cannot update incremental ID: {achievementId}");
            return;
        }

        // FIX: Use PlayGamesPlatform specific API for exact step targeting
        // SetStepsAtLeast ensures the server updates to your latest local progress smoothly
        PlayGamesPlatform.Instance.SetStepsAtLeast(achievementId, currentCount, (bool success) =>
        {
            if (success)
            {
              //  Debug.Log($"[Achievements] Successfully set incremental achievement {info.name} steps to: {currentCount}/{targetCount}");

                // Check if your local count has officially hit or crossed the required server target
                if (currentCount >= targetCount)
                {
                  //  Debug.Log($"[Achievements] TARGET REACHED! Achievement {achievementId} is now FULLY UNLOCKED!");

                    achievementData.UnLockLocal(achievementId);

                    if (achievementAS != null && !achievementAS.isPlaying)
                    {
                        achievementAS.Play();
                    }
                }
            }
            else
            {
                Debug.LogError($"[Achievements] Failed to update incremental achievement steps via SetStepsAtLeast for ID: {achievementId}");
            }
        });
    }

    public void SetTotalWins()
    {

        achievementData.defaultBoardWinsData.spTotalWins = achievementData.defaultBoardWinsData.spBWEasyWins + achievementData.defaultBoardWinsData.spBWMediumWins + achievementData.defaultBoardWinsData.spBWHardWins
        + achievementData.defaultBoardWinsData.spFSEasyWins + achievementData.defaultBoardWinsData.spFSMediumWins + achievementData.defaultBoardWinsData.spFSHardWins;


        achievementData.defaultBoardWinsData.mpTotalWins = achievementData.defaultBoardWinsData.mpBWWins + achievementData.defaultBoardWinsData.mpFSWins;


        foreach (BoardStats stats in achievementData.specialBoardWinsStats.boardStats)
        {
            achievementData.defaultBoardWinsData.spTotalWins += (stats.spBWWins + stats.spFSWins);
            achievementData.defaultBoardWinsData.mpTotalWins += (stats.mpBWWins + stats.mpFSWins);
        }


    }

    public void AddLogin()
    {
        // If we have no record, count this as first login
        if (string.IsNullOrEmpty(achievementData.defaultBoardWinsData.lastLoginDate))
        {

            achievementData.defaultBoardWinsData.lastLoginDate = DateTime.Now.ToLongDateString();
            achievementData.defaultBoardWinsData.totalLogins += 1;

            return;
        }

        // Parse stored date and compare calendar date only
        DateTime.TryParse(achievementData.defaultBoardWinsData.lastLoginDate, out DateTime lastLogin);


        if (lastLogin.Date != DateTime.Now.Date)
        {
            achievementData.defaultBoardWinsData.lastLoginDate = DateTime.Now.ToLongDateString();
            achievementData.defaultBoardWinsData.totalLogins += 1;
            StartCoroutine(UnLockLoginAchievements());
           
        }


        achievementData.defaultBoardWinsData.spTotalWins = achievementData.defaultBoardWinsData.spBWEasyWins + achievementData.defaultBoardWinsData.spBWMediumWins + achievementData.defaultBoardWinsData.spBWHardWins
            + achievementData.defaultBoardWinsData.spFSEasyWins + achievementData.defaultBoardWinsData.spFSMediumWins + achievementData.defaultBoardWinsData.spFSHardWins;

        achievementData.defaultBoardWinsData.mpTotalWins = achievementData.defaultBoardWinsData.mpBWWins + achievementData.defaultBoardWinsData.mpFSWins;

        SaveUserData();
    }

    public void AddClient(string clientId)
    {
        if (!achievementData.specialBoardWinsStats.clientNames.Contains(clientId))
        {
            achievementData.specialBoardWinsStats.clientNames.Add(clientId);
            StartCoroutine(UnLockTableHostAchievements());
        }
    }

    public IEnumerator UnLockLoginAchievements()
    {

        yield return null;
        if (achievementData.defaultBoardWinsData.totalLogins >= 1)
        {
            if (!achievementData.IsAchievementUnlockedByName("login1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("login1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (achievementData.defaultBoardWinsData.totalLogins >= 3)
        {
            if (!achievementData.IsAchievementUnlockedByName("login3"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("login3");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (achievementData.defaultBoardWinsData.totalLogins >= 5)
        {
            if (!achievementData.IsAchievementUnlockedByName("login5"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("login5");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }



        if (!achievementData.IsAchievementUnlockedByName("login10"))
        {
            AchievementInfo info = achievementData.GetAchievementByName("login10");
            if (achievementData.defaultBoardWinsData.spBWHardWins > 10)
            {
                info.actual = 10;
            }
            else
            {
                info.actual = achievementData.defaultBoardWinsData.totalLogins;
            }
            UpdateIncrementalAchievement(info, info.actual, info.target);
            yield return new WaitForSeconds(1);
        }



    }
    public IEnumerator UnLockTableHostAchievements()
    {
        yield return null;

        if (achievementData.specialBoardWinsStats.clientNames.Count >= 1)
        {
            if (!achievementData.IsAchievementUnlockedByName("invite1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("invite1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (achievementData.specialBoardWinsStats.clientNames.Count >= 3)
        {
            if (!achievementData.IsAchievementUnlockedByName("invite3"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("invite3");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }

        if (achievementData.specialBoardWinsStats.clientNames.Count >= 5)
        {
            if (!achievementData.IsAchievementUnlockedByName("invite5"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("invite5");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (!achievementData.IsAchievementUnlockedByName("invite10"))
        {
            AchievementInfo info = achievementData.GetAchievementByName("invite10");
            if (achievementData.defaultBoardWinsData.spBWHardWins > 10)
            {
                info.actual = 10;
            }
            else
            {
                info.actual = achievementData.specialBoardWinsStats.clientNames.Count;
            }
            UpdateIncrementalAchievement(info, info.actual, info.target);
            yield return new WaitForSeconds(1);
        }


    }
    public IEnumerator UnLockWinAchievements()
    {
        yield return null;

        if (achievementData.defaultBoardWinsData.spBWEasyWins >= 1)
        {
            if (!achievementData.IsAchievementUnlockedByName("spPoolEasyWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spPoolEasyWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (achievementData.defaultBoardWinsData.spFSMediumWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spPoolMediumWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spPoolMediumWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (achievementData.defaultBoardWinsData.spBWHardWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spPoolHardWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spPoolHardWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }

        }


        if (!achievementData.IsAchievementUnlockedByName("spPoolHardWins10"))
        {
            AchievementInfo info = achievementData.GetAchievementByName("spPoolHardWins10");
            if (achievementData.defaultBoardWinsData.spBWHardWins > 10)
            {
                info.actual = 10;
            }
            else
            {
                info.actual = achievementData.defaultBoardWinsData.spBWHardWins;
            }
            UpdateIncrementalAchievement(info, info.actual, info.target);
            
            yield return new WaitForSeconds(1);
        }

        if (achievementData.defaultBoardWinsData.spFSEasyWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spSnookerEasyWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spSnookerEasyWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (achievementData.defaultBoardWinsData.spFSMediumWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spSnookerMediumWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spSnookerMediumWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (achievementData.defaultBoardWinsData.spFSHardWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spSnookerHardWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spSnookerHardWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }

        }



        if (!achievementData.IsAchievementUnlockedByName("spSnookerHardWins10"))
        {
            AchievementInfo info = achievementData.GetAchievementByName("spSnookerHardWins10");
            
            if (achievementData.defaultBoardWinsData.spFSHardWins > 10)
            {
                info.actual = 10;
            }
            else
            {
                info.actual = achievementData.defaultBoardWinsData.spFSHardWins;
            }

            UpdateIncrementalAchievement(info, info.actual, info.target);
            yield return new WaitForSeconds(1);
        }




        if (achievementData.defaultBoardWinsData.mpBWWins >= 1)
        {
            if (!achievementData.IsAchievementUnlockedByName("mpPoolWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpPoolWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }
        if (achievementData.defaultBoardWinsData.mpBWWins >= 3)
        {

            if (!achievementData.IsAchievementUnlockedByName("mpPoolWins3"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpPoolWins3");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }
        if (achievementData.defaultBoardWinsData.mpBWWins >= 5)
        {

            if (!achievementData.IsAchievementUnlockedByName("mpPoolWins5"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpPoolWins5");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (!achievementData.IsAchievementUnlockedByName("mpPoolWins10"))
        {
            AchievementInfo info = achievementData.GetAchievementByName("mpPoolWins10");
            if (achievementData.defaultBoardWinsData.mpBWWins > 10)
            {
                info.actual = 10;
            }
            else
            {
                info.actual = achievementData.defaultBoardWinsData.mpBWWins;
            }
            
            UpdateIncrementalAchievement(info, info.actual, info.target);
            yield return new WaitForSeconds(1);
        }



        if (achievementData.defaultBoardWinsData.mpFSWins >= 1)
        {
            if (!achievementData.IsAchievementUnlockedByName("mpSnookerWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpSnookerWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }
        if (achievementData.defaultBoardWinsData.mpFSWins >= 3)
        {

            if (!achievementData.IsAchievementUnlockedByName("mpSnookerWins3"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpSnookerWins3");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }
        if (achievementData.defaultBoardWinsData.mpFSWins >= 5)
        {

            if (!achievementData.IsAchievementUnlockedByName("mpSnookerWins5"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpSnookerWins5");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }

        if (!achievementData.IsAchievementUnlockedByName("mpSnookerWins10"))
        {
            AchievementInfo info = achievementData.GetAchievementByName("mpSnookerWins10");
            if (achievementData.defaultBoardWinsData.mpFSWins > 10)
            {
                info.actual = 10;
            }
            else
            {
                info.actual = achievementData.defaultBoardWinsData.mpFSWins;
            }

            UpdateIncrementalAchievement(info, info.actual, info.target);
            yield return new WaitForSeconds(1);
        }

    }
    public IEnumerator UnLockOverallAchievements()
    {
        yield return null;


        if (!achievementData.IsAchievementUnlockedByName("spTotalWins50"))
        {
            AchievementInfo info = achievementData.GetAchievementByName("spTotalWins50");
            if (achievementData.defaultBoardWinsData.spTotalWins > 50)
            {
                info.actual = 50;
            }
            else
            {
                info.actual = achievementData.defaultBoardWinsData.spTotalWins;
            }
            info.actual = achievementData.defaultBoardWinsData.spTotalWins;
            UpdateIncrementalAchievement(info, info.actual, info.target);
            yield return new WaitForSeconds(1);
        }

        if (!achievementData.IsAchievementUnlockedByName("mpTotalWins50"))
        {
            AchievementInfo info = achievementData.GetAchievementByName("mpTotalWins50");
            if (achievementData.defaultBoardWinsData.mpTotalWins > 50)
            {
                info.actual = 50;
            }
            else
            {
                info.actual = achievementData.defaultBoardWinsData.mpTotalWins;
            }
            UpdateIncrementalAchievement(info, info.actual, info.target);
            yield return new WaitForSeconds(1);
        }



        int totalWins = achievementData.defaultBoardWinsData.spTotalWins + achievementData.defaultBoardWinsData.mpTotalWins;


        if (!achievementData.IsAchievementUnlockedByName("GrandChampion"))
        {
            AchievementInfo info = achievementData.GetAchievementByName("GrandChampion");
            if (totalWins <= 100)
            {
                info.actual = totalWins;
            }
            else
            {
                info.actual = 100;
            }
            UpdateIncrementalAchievement(info, info.actual, info.target);
            yield return new WaitForSeconds(1);
        }


    }

    public IEnumerator UnLockBoardAchievements()
    {
        yield return null;

        AchievementInfo achievementInfo = null;

        foreach (BoardStats stats in achievementData.specialBoardWinsStats.boardStats)
        {
            achievementInfo = achievementData.GetAchievementByName("spPool" + Enum.GetName(typeof(BoardType), stats.boardType));
           
            if (achievementInfo != null)
            {
                if (stats.spBWWins >= 1)
                {
                    if (!achievementData.IsAchievementUnlockedByName(achievementInfo.name))
                    {
                        Debug.Log("Trying to unlock pool " + achievementInfo.name);
                        UnlockSimpleAchievement(achievementInfo);
                        yield return new WaitForSeconds(1);
                    }
                }

            }

            achievementInfo = achievementData.GetAchievementByName("spSnooker" + Enum.GetName(typeof(BoardType), stats.boardType));

            if (achievementInfo != null)
            {
                if (stats.spFSWins >= 1)
                {
                    if (!achievementData.IsAchievementUnlockedByName(achievementInfo.name))
                    {
                        Debug.Log("Trying to unlock snooker " + achievementInfo.name);
                        UnlockSimpleAchievement(achievementInfo);
                        yield return new WaitForSeconds(1);
                    }
                }

            }

            achievementInfo = achievementData.GetAchievementByName("mpPool" + Enum.GetName(typeof(BoardType), stats.boardType));

            if (achievementInfo != null)
            {
                if (stats.mpBWWins >= 1)
                {
                    if (!achievementData.IsAchievementUnlockedByName(achievementInfo.name))
                    {
                        UnlockSimpleAchievement(achievementInfo);
                        yield return new WaitForSeconds(1);
                    }
                }

            }

            achievementInfo = achievementData.GetAchievementByName("mpSnooker" + Enum.GetName(typeof(BoardType), stats.boardType));

            if (achievementInfo != null)
            {
                if (stats.mpFSWins >= 1)
                {
                    if (!achievementData.IsAchievementUnlockedByName(achievementInfo.name))
                    {
                        UnlockSimpleAchievement(achievementInfo);
                        yield return new WaitForSeconds(1);
                    }
                }

            }
        }

    }

    public void SaveUserData()
    {
        //cloudData.SavePlayerData();
    }

}


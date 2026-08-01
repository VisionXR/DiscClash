using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;
using GooglePlayGames;
using System.Collections;
using com.VisionXR.GameElements;


public class AchievementManager : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public AchievementsDataSO achievementData;
    public UIDataSO uiData;
    public UIInputDataSO uiInputData;
    public CloudDataSO cloudData;
    public DestinationDataSO destinationData;
    public PlayersDataSO playersData;

    [Header("Local Objects")]
    public AudioSource achievementAS;


    private void OnEnable()
    {
      
        achievementData.GetAllAchievementsEvent += GetAllAchievements;
        uiInputData.StartGameEvent += GameStarted;
        uiInputData.GameWonEvent += GameCompleted;
        achievementData.UserLoggedInEvent += AddLogin;


    }

    private void OnDisable()
    {
        achievementData.GetAllAchievementsEvent -= GetAllAchievements;
        uiInputData.StartGameEvent -= GameStarted;
        uiInputData.GameWonEvent -= GameCompleted;
        achievementData.UserLoggedInEvent -= AddLogin;
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

        Debug.Log("Game Started in achievements");

        Destination d = destinationData.currentDestination;

        if (d != null)
        {
            if (d.gameType == GameType.VsCPU)
            {
                achievementData.defaultBoardWinsData.spTotalGames++;

            }

            else if (d.gameType == GameType.PlayWithFriends)
            {
                achievementData.defaultBoardWinsData.mpTotalGames++;

                Player otherPlayer = playersData.GetOtherPlayer();
                if (otherPlayer != null)
                {
                    AddClient(otherPlayer.myOculusID);
                }

            }
        }

        SaveUserData();
    }

    public void GameCompleted()
    {

        Debug.Log("Game Completed in achievements");

        Destination d = destinationData.currentDestination;

        if (d != null)
        {
            if (d.gameType == GameType.VsCPU)
            {
                if (d.challenge == Challenge.BlackAndWhite)
                {
                    if (d.difficulty == AIDifficulty.Easy)
                    {
                        achievementData.defaultBoardWinsData.spBWEasyWins++;
                    }
                    else if (d.difficulty == AIDifficulty.Medium)
                    {
                        achievementData.defaultBoardWinsData.spBWMediumWins++;
                    }
                    else if (d.difficulty == AIDifficulty.Hard)
                    {
                        achievementData.defaultBoardWinsData.spBWHardWins++;
                    }

                }
                else if (d.challenge == Challenge.FreeStyle)
                {
                    if (d.difficulty == AIDifficulty.Easy)
                    {
                        achievementData.defaultBoardWinsData.spFSEasyWins++;
                    }
                    else if (d.difficulty == AIDifficulty.Medium)
                    {
                        achievementData.defaultBoardWinsData.spFSMediumWins++;
                    }
                    else if (d.difficulty == AIDifficulty.Hard)
                    {
                        achievementData.defaultBoardWinsData.spFSHardWins++;

                    }

                }

            }

            else if (d.gameType == GameType.PlayWithFriends)
            {
                if (d.challenge == Challenge.BlackAndWhite)
                {
                    achievementData.defaultBoardWinsData.mpBWWins++;
                }
                else if (d.challenge == Challenge.FreeStyle)
                {
                    achievementData.defaultBoardWinsData.mpFSWins++;
                }
            }

        }

        SaveUserData();
        StartCoroutine(UnLockWinAchievements());

    }

    public void AddLogin()
    {
        Debug.Log("Adding login in achievements");

        // If we have no record, count this as first login
        if (string.IsNullOrEmpty(achievementData.defaultBoardWinsData.lastLoginDate))
        {

            achievementData.defaultBoardWinsData.lastLoginDate = DateTime.Now.ToLongDateString();
            achievementData.defaultBoardWinsData.totalLogins += 1;
            SaveUserData();
            StartCoroutine(UnLockLoginAchievements());
            return;
        }

        // Parse stored date and compare calendar date only
        DateTime.TryParse(achievementData.defaultBoardWinsData.lastLoginDate, out DateTime lastLogin);


        if (lastLogin.Date != DateTime.Now.Date)
        {
            achievementData.defaultBoardWinsData.lastLoginDate = DateTime.Now.ToLongDateString();
            achievementData.defaultBoardWinsData.totalLogins += 1;
            SaveUserData();
            

        }

        StartCoroutine(UnLockLoginAchievements());
    }

    public void AddClient(string clientId)
    {
        if (!achievementData.defaultBoardWinsData.clientNames.Contains(clientId))
        {
            achievementData.defaultBoardWinsData.clientNames.Add(clientId);
            StartCoroutine(UnLockTableHostAchievements());
        }
    }

    public IEnumerator UnLockLoginAchievements()
    {

        yield return null;

        Debug.Log("Unlocking login achievements. Total logins: " + achievementData.defaultBoardWinsData.totalLogins);

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
            if (achievementData.defaultBoardWinsData.totalLogins > 10)
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

        if (achievementData.defaultBoardWinsData.clientNames.Count >= 1)
        {
            if (!achievementData.IsAchievementUnlockedByName("host1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("host1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (achievementData.defaultBoardWinsData.clientNames.Count >= 3)
        {
            if (!achievementData.IsAchievementUnlockedByName("host3"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("host3");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }

        if (achievementData.defaultBoardWinsData.clientNames.Count >= 5)
        {
            if (!achievementData.IsAchievementUnlockedByName("host5"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("host5");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (!achievementData.IsAchievementUnlockedByName("host10"))
        {
            AchievementInfo info = achievementData.GetAchievementByName("host10");
            if (achievementData.defaultBoardWinsData.clientNames.Count > 10)
            {
                info.actual = 10;
            }
            else
            {
                info.actual = achievementData.defaultBoardWinsData.clientNames.Count;
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
            if (!achievementData.IsAchievementUnlockedByName("spBWEasyWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spBWEasyWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }
        if (achievementData.defaultBoardWinsData.spBWEasyWins >= 3)
        {
            if (!achievementData.IsAchievementUnlockedByName("spBWEasyWins3"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spBWEasyWins3");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (achievementData.defaultBoardWinsData.spBWMediumWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spBWMediumWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spBWMediumWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }
        if (achievementData.defaultBoardWinsData.spBWMediumWins >= 3)
        {

            if (!achievementData.IsAchievementUnlockedByName("spBWMediumWins3"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spBWMediumWins3");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (achievementData.defaultBoardWinsData.spBWHardWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spBWHardWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spBWHardWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }

        }
        if (achievementData.defaultBoardWinsData.spBWHardWins >= 3)
        {

            if (!achievementData.IsAchievementUnlockedByName("spBWHardWins3"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spBWHardWins3");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }

        }
 

        if (achievementData.defaultBoardWinsData.spFSEasyWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spFSEasyWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spFSEasyWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }
        if (achievementData.defaultBoardWinsData.spFSEasyWins >= 3)
        {

            if (!achievementData.IsAchievementUnlockedByName("spFSEasyWins3"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spFSEasyWins3");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (achievementData.defaultBoardWinsData.spFSMediumWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spFSMediumWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spFSMediumWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }
        if (achievementData.defaultBoardWinsData.spFSMediumWins >= 3)
        {

            if (!achievementData.IsAchievementUnlockedByName("spFSMediumWins3"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spFSMediumWins3");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


        if (achievementData.defaultBoardWinsData.spFSHardWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spFSHardWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spFSHardWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }

        }
        if (achievementData.defaultBoardWinsData.spFSHardWins >= 3)
        {

            if (!achievementData.IsAchievementUnlockedByName("spFSHardWins3"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spFSHardWins3");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }

        }



        if (achievementData.defaultBoardWinsData.mpBWWins >= 1)
        {
            if (!achievementData.IsAchievementUnlockedByName("mpBWWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpBWWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }
        if (achievementData.defaultBoardWinsData.mpBWWins >= 3)
        {

            if (!achievementData.IsAchievementUnlockedByName("mpBWWins3"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpBWWins3");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }
        if (achievementData.defaultBoardWinsData.mpBWWins >= 5)
        {

            if (!achievementData.IsAchievementUnlockedByName("mpBWWins5"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpBWWins5");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }



        if (achievementData.defaultBoardWinsData.mpFSWins >= 1)
        {
            if (!achievementData.IsAchievementUnlockedByName("mpFSWins1"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpFSWins1");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }
        if (achievementData.defaultBoardWinsData.mpFSWins >= 3)
        {

            if (!achievementData.IsAchievementUnlockedByName("mpFSWins3"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpFSWins3");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }
        if (achievementData.defaultBoardWinsData.mpFSWins >= 5)
        {

            if (!achievementData.IsAchievementUnlockedByName("mpFSWins5"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpFSWins5");
                UnlockSimpleAchievement(info);
                yield return new WaitForSeconds(1);
            }
        }


    }
  
    public void SaveUserData()
    {
        cloudData.SavePlayerData();
    }

}


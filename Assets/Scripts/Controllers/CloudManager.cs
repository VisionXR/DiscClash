using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections; // Required for Coroutines
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class CloudManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public CloudDataSO cloudData;
        public MyPlayerSettings userData;
        public AchievementsDataSO achievementsData;

        [Header("Keys")]
        public string defaultBoardsWinsKey = "CarromDefaultBoardsData";


        // Actions
        private Action OnDataFetchSuccessEvent;
        private Action OnDataFetchFailureEvent;

        private Coroutine loadTimeoutCoroutine;
        // G03: Invalidate callbacks from an earlier request after timeout, retry, disable or offline play.
        private int loadGeneration;
        private bool awaitingLoad;

        public void CancelLoad()
        {
            loadGeneration++;
            awaitingLoad = false;
            if (loadTimeoutCoroutine != null) StopCoroutine(loadTimeoutCoroutine);
            loadTimeoutCoroutine = null;
            OnDataFetchSuccessEvent = null;
            OnDataFetchFailureEvent = null;
        }

        private void OnEnable()
        {
            cloudData.LoadPlayerDataEvent += LoadPlayerData;
            cloudData.SavePlayerDataEvent += SaveUserData;
            userData.DeleteAccountEvent += DeleteUserData;
        }

        private void OnDisable()
        {
            cloudData.LoadPlayerDataEvent -= LoadPlayerData;
            cloudData.SavePlayerDataEvent -= SaveUserData;
            userData.DeleteAccountEvent -= DeleteUserData;

            // Safety check to prevent memory leaks if destroyed during loading
            // G03: No stale response may update data after this manager is disabled.
            CancelLoad();
        }

        /// <summary>
        /// Fetches both Inventory (Coins) and Custom User Data (DiscPoolUserData)
        /// </summary>
        public void LoadPlayerData(Action OnSuccess, Action OnFailure)
        {
            // G03: One request per attempt, with a real deadline and one completion callback.
            CancelLoad();
            OnDataFetchSuccessEvent = OnSuccess;
            OnDataFetchFailureEvent = OnFailure;
            cloudData.DataLoaded(false);
            awaitingLoad = true;
            int generation = loadGeneration;
            loadTimeoutCoroutine = StartCoroutine(LoadPlayerDataRoutine(5f, generation));
            RequestUserData(generation);
        }

        private IEnumerator LoadPlayerDataRoutine(float timeoutDuration, int generation)
        {
            float deadline = Time.realtimeSinceStartup + timeoutDuration;
            while (awaitingLoad && generation == loadGeneration && Time.realtimeSinceStartup < deadline)
                yield return null;
            if (awaitingLoad && generation == loadGeneration) FinishLoad(generation, false);
        }

        // G03: Clear state before invoking user callbacks, which may synchronously request another load.
        private void FinishLoad(int generation, bool success)
        {
            if (!awaitingLoad || generation != loadGeneration) return;
            Action callback = success ? OnDataFetchSuccessEvent : OnDataFetchFailureEvent;
            CancelLoad();
            cloudData.DataLoaded(success);
            callback?.Invoke();
        }

        // --- SAVE DATA ---
        public void SaveUserData()
        {
            try
            {
                string jsonString = JsonUtility.ToJson(achievementsData.defaultBoardWinsData);

                var request = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string> {
                        { defaultBoardsWinsKey, jsonString }
                    }
                };

                PlayFabClientAPI.UpdateUserData(request, result => { }, OnDataFetchError);
            }
            catch (Exception e)
            {
                Debug.Log("Error saving user data: " + e.Message);
            }

          
        }

        // --- LOAD DATA ---
        public void LoadUserData()
        {
            // G03: Keep the public entry point while using the guarded load lifecycle.
            LoadPlayerData(null, null);
        }

        private void RequestUserData(int generation)
        {
            var request = new GetUserDataRequest { Keys = new List<string> { defaultBoardsWinsKey } };
            try
            {
                PlayFabClientAPI.GetUserData(request, result =>
                {
                    // G03: Ignore late replies instead of overwriting a newer/offline session's progress.
                    if (!awaitingLoad || generation != loadGeneration || cloudData.isOfflineSession) return;
                    try
                    {
                        if (result.Data != null && result.Data.ContainsKey(defaultBoardsWinsKey))
                        {
                            var data = JsonUtility.FromJson<DefaultBoardWinsData>(result.Data[defaultBoardsWinsKey].Value);
                            if (data == null) throw new InvalidOperationException("Invalid player data.");
                            // G03: Older cloud records may omit the client list used by online achievements.
                            if (data.clientNames == null) data.clientNames = new List<string>();
                            achievementsData.defaultBoardWinsData = data;
                            achievementsData.UserLoggedIn();
                        }
                        FinishLoad(generation, true);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning("Player data could not be read: " + e.Message);
                        FinishLoad(generation, false);
                    }
                }, error => FinishLoad(generation, false));
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error loading data: " + e.Message);
                FinishLoad(generation, false);
            }
        }

        // --- DELETE DATA ---
        public void DeleteUserData()
        {
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> {
                    { defaultBoardsWinsKey, null }
                   
                }
            };

            try
            {
                PlayFabClientAPI.UpdateUserData(request, result =>
                {
                    
                    achievementsData.Clear();

                }, OnDataFetchError);
            }
            catch (Exception e)
            {
                Debug.Log("Error deleting user data: " + e.Message);
            }
        }

        private void OnDataFetchError(PlayFabError error)
        {
            // G03: Save/delete failures must not cancel an unrelated player-data fetch.
            Debug.LogWarning("Cloud write failed: " + error.ErrorMessage);
        }
    }
}
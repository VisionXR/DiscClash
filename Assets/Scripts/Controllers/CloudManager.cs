using com.VisionXR.ModelClasses;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class CloudManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public CloudDataSO cloudData;



        private void OnEnable()
        {
            cloudData.FetchCoinsEvent += FetchUserCoins;
        }

        private void OnDisable()
        {
            cloudData.FetchCoinsEvent -= FetchUserCoins;
        }

        

        /// <summary>
        /// Call this after a successful Login to sync the player's wallet.
        /// </summary>
        public void FetchUserCoins()
        {
            var request = new GetUserInventoryRequest();

            PlayFabClientAPI.GetUserInventory(request, OnFetchSuccess, OnFetchError);
        }

        private void OnFetchSuccess(GetUserInventoryResult result)
        {
            // "CN" is your currency code for Coins
            if (result.VirtualCurrency.ContainsKey("CN"))
            {
                int coinBalance = result.VirtualCurrency["CN"];

                // Update your ScriptableObject so the UI updates automatically
                if (cloudData != null)
                {
                    cloudData.coins = coinBalance;
                }

                Debug.Log($"[CloudManager] Coins Synced: {coinBalance}");
            }
        }

        private void OnFetchError(PlayFabError error)
        {
            Debug.LogError($"[CloudManager] Failed to fetch coins: {error.GenerateErrorReport()}");

            // If internet fails here, you might want to trigger a 'Retry' popup
            // as we discussed for the game flow.
        }



        public void CheckDailyBonus()
        {
            Debug.Log("[CloudManager] Checking Daily Bonus eligibility...");

            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = "ClaimDailyBonus", // Must match the name in your PlayFab Automation tab
                GeneratePlayStreamEvent = true
            };

            PlayFabClientAPI.ExecuteCloudScript(request, OnDailyBonusResult, OnFetchError);
        }

        private void OnDailyBonusResult(ExecuteCloudScriptResult result)
        {
            if (result.FunctionResult != null)
            {
                // Convert the object to a JSON string, then into our class
                string jsonString = result.FunctionResult.ToString();
                DailyBonusResponse response = JsonUtility.FromJson<DailyBonusResponse>(jsonString);

                if (response.success)
                {
                    Debug.Log($"[CloudManager] Bonus Claimed! Added {response.amount} coins.");

                    // Refresh the coins in your SO by calling the fetch function we wrote
                    FetchUserCoins();

                    // Trigger your UI here (e.g., showing the coin animation)
                }
                else
                {
                    // This is the "Too early" message (e.g., "Available in 5 hours")
                    Debug.Log($"[CloudManager] Daily Bonus: {response.message}");
                }
            }
        }

        public void GrantWinnings(int amount)
        {
            var request = new AddUserVirtualCurrencyRequest
            {
                VirtualCurrency = "CN",
                Amount = amount
            };

            PlayFabClientAPI.AddUserVirtualCurrency(request, result => {
                Debug.Log("Winnings added! Total Coins: " + result.Balance);
                
            }, error => {
                Debug.LogError("Failed to add coins: " + error.GenerateErrorReport());
            });
        }

        public void DeductEntryFee(int amount)
        {
            var request = new SubtractUserVirtualCurrencyRequest
            {
                VirtualCurrency = "CN", // Your currency code
                Amount = amount
            };

            PlayFabClientAPI.SubtractUserVirtualCurrency(request, result => {
                Debug.Log("Entry fee deducted! New Balance: " + result.Balance);
                // Start the Disc Clash match here
            }, error => {
                    Debug.LogError("Not enough coins to play!");               
            });
        }
    }


    [System.Serializable]
    public class DailyBonusResponse
    {
        public bool success;
        public int amount;
        public string message;
    }
}
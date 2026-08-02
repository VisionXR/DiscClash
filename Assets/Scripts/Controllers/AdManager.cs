using UnityEngine;
using GoogleMobileAds.Api;
using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;
using System.Collections;

namespace com.VisionXR.Controllers
{
    public class AdManager : MonoBehaviour
    {
        [Header("Scriptable Object References")]
        public ADDataSO adDataSO;
        public PurchaseDataSO purchaseData;

        [Header("Ad Unit IDs")]
        public string testInterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712"; // Test Interstitial Ad Unit ID
        public string testRewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917"; // Test Rewarded Ad Unit ID
        public string _interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
        public string _rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";

        // Local variables to hold the loaded ads
        private InterstitialAd _interstitialAd;
        private RewardedAd _rewardedAd;
        private bool isSdkInitialized = false;
        private bool isRewardedAdLoading = false;

        private void OnEnable()
        {
            adDataSO.LoadInterstitialAdEvent += LoadInterstitialAd;
            adDataSO.LoadRewardedAdEvent += LoadRewardedAd;
            adDataSO.ShowInterstitialAdEvent += ShowInterstitialAd;
            adDataSO.ShowRewardedAdEvent += ShowRewardedAd;
        }

        private void OnDisable()
        {
            adDataSO.LoadInterstitialAdEvent -= LoadInterstitialAd;
            adDataSO.LoadRewardedAdEvent -= LoadRewardedAd;
            adDataSO.ShowInterstitialAdEvent -= ShowInterstitialAd;
            adDataSO.ShowRewardedAdEvent -= ShowRewardedAd;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);

            MobileAds.Initialize((InitializationStatus status) =>
            {
                isSdkInitialized = true;
            });

            while (!isSdkInitialized)
            {
                yield return null;
            }

            Debug.Log("Google Mobile Ads SDK Initialized. Loading ads...");
            yield return new WaitForSeconds(1f);
            LoadInterstitialAd();

            yield return new WaitForSeconds(1f);
            LoadRewardedAd();
        }

        #region Interstitial Ad Methods

        public void LoadInterstitialAd()
        {
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

            var adRequest = new AdRequest();

            InterstitialAd.Load(testInterstitialAdUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Interstitial ad failed to load: " + error);
                    return;
                }

                _interstitialAd = ad;

                _interstitialAd.OnAdFullScreenContentClosed += () =>
                {
                    adDataSO.OnInterstitialAdSuccess();
                    LoadInterstitialAd();
                };
            });
        }

        public void ShowInterstitialAd()
        {
            AssetData noAdsData = purchaseData.GetItemByProductId(purchaseData.AllItemsData.Count - 1);
            if (noAdsData != null && noAdsData.isPurchased)
            {
                Debug.Log("Purchase made for no ads");
                return;
            }

            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                _interstitialAd.Show();
            }
            else
            {
                LoadInterstitialAd();
            }
        }

        #endregion

        #region Rewarded Ad Methods

        public void LoadRewardedAd()
        {
            // Prevent multiple concurrent load requests
            if (isRewardedAdLoading) return;

            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            isRewardedAdLoading = true;
            var adRequest = new AdRequest();

            RewardedAd.Load(testRewardedAdUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                isRewardedAdLoading = false;

                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load: " + error);
                    return;
                }

                _rewardedAd = ad;

                // Handle cleanup and preloading next ad when closed
                _rewardedAd.OnAdFullScreenContentClosed += () =>
                {
                    LoadRewardedAd();
                };

                // Optional: Handle ad failed to present
                _rewardedAd.OnAdFullScreenContentFailed += (AdError adError) =>
                {
                    Debug.LogError("Rewarded ad failed to show: " + adError);
                    LoadRewardedAd();
                };
            });
        }

        public void ShowRewardedAd()
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                // 1. Keep a local reference to the ad about to be displayed
                RewardedAd adToShow = _rewardedAd;

                // 2. Clear the main reference variable right away
                _rewardedAd = null;

                // 3. Immediately start preloading the NEXT ad in the background while this one plays
                LoadRewardedAd();

                // 4. Show the current ad using the local reference
                adToShow.Show((Reward reward) =>
                {
                    Debug.Log($"User earned reward: {reward.Amount} {reward.Type}");
                    adDataSO.OnRewardedAdSuccess();
                });
            }
            else if (isRewardedAdLoading)
            {
                Debug.LogWarning("Ad is still downloading. Please wait a second!");
                adDataSO.RewardAdFailedToLoad();
            }
            else
            {
                Debug.LogWarning("Rewarded ad is not ready yet. Reloading...");
                adDataSO.RewardAdFailedToLoad();
                LoadRewardedAd();
            }
        }

        #endregion
    }
}
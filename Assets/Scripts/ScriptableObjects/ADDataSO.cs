using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "ADDataSO", menuName = "ScriptableObjects/ADDataSO", order = 1)]
    public class ADDataSO : ScriptableObject
    {
        // variables



        // Events

        public Action LoadInterstitialAdEvent;
        public Action LoadRewardedAdEvent;
        public Action ShowInterstitialAdEvent;
        public Action ShowRewardedAdEvent;

        public Action OnInterstitialAdSuccessEvent;
        public Action OnRewardedAdSuccessEvent;


        public Action OnRewardedAdFailedToLoadEvent;

        // Methods

        public void LoadInterstitialAd()
        {
            LoadInterstitialAdEvent?.Invoke();
        }

        public void LoadRewardedAd()
        {
            LoadRewardedAdEvent?.Invoke();
        }

        public void ShowInterstitialAd()
        {
            ShowInterstitialAdEvent?.Invoke();
        }

        public void ShowRewardedAd()
        {
            ShowRewardedAdEvent?.Invoke();
        }

        public void RewardAdFailedToLoad()
        {
            OnRewardedAdFailedToLoadEvent?.Invoke();
        }

        public void OnInterstitialAdSuccess()
        {
         
            OnInterstitialAdSuccessEvent?.Invoke();
        }

        public void OnRewardedAdSuccess()
        {
            
            OnRewardedAdSuccessEvent?.Invoke();
        }

    }
}

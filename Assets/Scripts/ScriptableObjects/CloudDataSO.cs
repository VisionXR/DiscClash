using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "CloudDataSO", menuName = "ScriptableObjects/CloudDataSO", order = 1)]    
    public class CloudDataSO : ScriptableObject   
    {
        // variables
        public int coins;


        //Login  Events    
        public Action LoginToGoogleEvent;
        public Action GuestLoginEvent;
        public Action EditorLoginEvent;

        public Action StartFetchEvent;
        public Action RetryEvent;

        public Action PlayFabLoginSuccessEvent;
        public Action PlayFabLoginFailureEvent;



        // coin events
        public Action<int,Action,Action> DeductEntryFeeEvent;
        public Action<int> GrantWinningsEvent;
        public Action<Action,Action> FetchCoinsEvent;

        public Action FetchSuccessEvent;
        public Action FetchFailureEvent;



        // Methods

        private void OnEnable()
        {
            coins = 0;
        }
        public void LoginToGoogle()
        {
            LoginToGoogleEvent?.Invoke();
        }

        public void GuestLogin()
        {
            GuestLoginEvent?.Invoke();
        }

        public void EditorLogin()
        {
            EditorLoginEvent?.Invoke();
        }

        public void FetchCoins(Action OnSuccess,Action OnFailure)
        {
            FetchCoinsEvent?.Invoke(OnSuccess,OnFailure);
        }

        public void PlayFabLoginSuccess()
        {
            PlayFabLoginSuccessEvent?.Invoke();
        }

        public void PlayFabLoginFailure()
        {
            PlayFabLoginFailureEvent?.Invoke();
        }

        public void DeductEntryFee(int amount,Action OnSuccess,Action Onfailure)
        {
            DeductEntryFeeEvent?.Invoke(amount,OnSuccess,Onfailure);
        }

        public void GrantWinnings(int amount)
        {
            GrantWinningsEvent?.Invoke(amount);
        }

        public void StartFetch()
        {
            StartFetchEvent?.Invoke();
        }

        public void Retry()
        {
            RetryEvent?.Invoke();
        }

        public void FetchSuccess()
        {
            FetchSuccessEvent?.Invoke();
        }

        public void FetchFailure()
        {
            FetchFailureEvent?.Invoke();
        }


    }
}
        
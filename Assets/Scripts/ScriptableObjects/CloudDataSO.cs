using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "CloudDataSO", menuName = "ScriptableObjects/CloudDataSO", order = 1)]    
    public class CloudDataSO : ScriptableObject   
    {
        // variables
        private bool isPlayerDataLoaded = false;


        //Login  Events    
        public Action LoginToGoogleEvent;
        public Action GuestLoginEvent;
        public Action EditorLoginEvent;

        public Action StartFetchEvent;
        public Action RetryEvent;

        public Action PlayFabLoginSuccessEvent;
        public Action PlayFabLoginFailureEvent;

        // save and load data
        public Action<Action, Action> LoadPlayerDataEvent;
        public Action SavePlayerDataEvent;

        public Action FetchSuccessEvent;
        public Action FetchFailureEvent;

        // Methods

        private void OnEnable()
        {
            isPlayerDataLoaded = false;
        }

        public void LoadPlayerData(Action OnSuccess, Action OnFailure)
        {
            LoadPlayerDataEvent?.Invoke(OnSuccess, OnFailure);
        }

        public void SavePlayerData()
        {
            if (isPlayerDataLoaded)
            {
                SavePlayerDataEvent?.Invoke();
            }
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


        public void PlayFabLoginSuccess()
        {
            PlayFabLoginSuccessEvent?.Invoke();
        }

        public void PlayFabLoginFailure()
        {
            PlayFabLoginFailureEvent?.Invoke();
        }


        public void DataLoaded(bool status)
        {
            isPlayerDataLoaded = status;
        }

        public bool isDataLoaded()
        {
            return isPlayerDataLoaded;
        }

        public void FetchSuccess()
        {
            FetchSuccessEvent?.Invoke();
        }

        public void FetchFailure()
        {
            FetchFailureEvent?.Invoke();
        }

        public void Retry()
        {
            RetryEvent?.Invoke();
        }



    }
}
        
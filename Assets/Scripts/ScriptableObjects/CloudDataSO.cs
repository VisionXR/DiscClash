using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "CloudDataSO", menuName = "ScriptableObjects/CloudDataSO", order = 1)]    
    public class CloudDataSO : ScriptableObject   
    {
        // variables
        public int coins;


        //Network  Events    
        public Action LoginToGoogleEvent;
        public Action GuestLoginEvent;
        public Action EditorLoginEvent;
        public Action<int> DeductEntryFeeEvent;
        public Action<int> GrantWinningsEvent;


        public Action<Action,Action> FetchCoinsEvent;


        public Action PlayFabLoginSuccessEvent;
        public Action PlayFabLoginFailureEvent;

        // Methods
  

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

        public void DeductEntryFee(int amount)
        {
            DeductEntryFeeEvent?.Invoke(amount);
        }

        public void GrantWinnings(int amount)
        {
            GrantWinningsEvent?.Invoke(amount);
        }


    }
}
        
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

        public Action FetchCoinsEvent;


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

        public void FetchCoins()
        {
            FetchCoinsEvent?.Invoke();
        }


    }
}
        
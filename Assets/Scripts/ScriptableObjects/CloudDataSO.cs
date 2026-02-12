using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "CloudDataSO", menuName = "ScriptableObjects/CloudDataSO", order = 1)]    
    public class CloudDataSO : ScriptableObject   
    {
        // variables



        //Network  Events    
        public Action LoginToGoogleEvent;
        public Action GuestLoginEvent;


        // Methods
  

        public void LoginToGoogle()
        {
            LoginToGoogleEvent?.Invoke();
        }

        public void GuestLogin()
        {
            GuestLoginEvent?.Invoke();
        }

       
    }
}
        
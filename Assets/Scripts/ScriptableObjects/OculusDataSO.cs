using com.VisionXR.HelperClasses;
using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "OculusDataSO", menuName = "ScriptableObjects/OculusDataSO", order = 1)]
    public class OculusDataSO : ScriptableObject
    {
        // variables


        // events
        public Action<MultiPlayerGameMode, string, string, bool> SetDestinationEvent;
        public Action<string, string, string> GoToDestinationEvent;
        public Action ClearDestinationEvent;
        public Action<Destination,Action,Action> ConnectToDestinationEvent;


        // methods

        public void ConnectToDestination(Destination destination,Action OnConnectionSuccess,Action OnConnectionFail)
        {
            ConnectToDestinationEvent?.Invoke(destination,OnConnectionSuccess, OnConnectionFail);
        }

        public void ClearDestination()
        {
            ClearDestinationEvent?.Invoke();
        }


    }
}

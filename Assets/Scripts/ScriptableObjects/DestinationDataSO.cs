using com.VisionXR.HelperClasses;
using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "DestinationDataSO", menuName = "ScriptableObjects/DestinationDataSO", order = 1)]
    public class DestinationDataSO : ScriptableObject
    {
        // variables
        public Destination currentDestination;

        // events

        public Action ClearDestinationEvent;
        public Action<Destination,Action,Action> ConnectToDestinationEvent;


        // methods

        public void ConnectToDestination(Destination destination,Action OnConnectionSuccess,Action OnConnectionFail)
        {
           
            ConnectToDestinationEvent?.Invoke(destination,OnConnectionSuccess, OnConnectionFail);
        }

    }
}

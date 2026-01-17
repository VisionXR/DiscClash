using com.VisionXR.HelperClasses;
using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "NetworkOutputSO", menuName = "ScriptableObjects/NetworkOutputSO", order = 1)]
    public class NetworkOutputSO : ScriptableObject
    {
        // variables
        public ServerRegion currentRegion;
        public List<AvailableRooms> roomsAvailable = new List<AvailableRooms>();
        public bool isHost;
        public string RoomName;
        public string CommonLobby = "Real Carrom Lobby";
        public NetworkRunner _runner;

        // Actions
        public Action<List<AvailableRooms>> RoomListUpdatedEvent;
        
        // Methods      

        public void SetHost(bool value)
        {
            isHost = value;
        }

        public bool IsHost()
        {
            return isHost;
        }

        public void SetAvailableRooms(List<AvailableRooms> availableRooms)
        {
            roomsAvailable = availableRooms;
            RoomListUpdatedEvent?.Invoke(roomsAvailable);
        }
    }
}

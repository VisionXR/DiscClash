using com.VisionXR.HelperClasses;
using Fusion;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "NetworkOutputSO", menuName = "ScriptableObjects/NetworkOutputSO", order = 1)]
    public class NetworkOutputSO : ScriptableObject
    {
        // variables
        public ServerRegion currentRegion;
        public bool isHost;
        public string RoomName;
        public string CommonLobby = "DiscClashLobby";
        public NetworkRunner runner;


        // local
        private bool isHostReady = false;
        private bool isClientReady = false;

        // Methods      

        private void OnEnable()
        {
            isHost = false;
        }

        public void SetHost(bool value)
        {
            isHost = value;
        }

        public bool IsHost()
        {
            return isHost;
        }

        public bool IsHostReady()
        {
            return isHostReady;
        }

        public bool IsClientReady()
        {
            return isClientReady;
        }

    }
}

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
        public NetworkRunner _runner;


  

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

    }
}

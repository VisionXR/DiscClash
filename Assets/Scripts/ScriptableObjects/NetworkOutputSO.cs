using com.VisionXR.HelperClasses;
using Fusion;
using System;
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

        // Actions
        // G01: Report unexpected local disconnects once per runner; intentional exits are suppressed.
        public Action<string> ConnectionLostEvent;
        public bool intentionalShutdown;
        private NetworkRunner notifiedRunner;

        public void ReportConnectionLost(NetworkRunner source, string reason)
        {
            if (intentionalShutdown || source != runner || notifiedRunner == source) return;
            notifiedRunner = source;
            ConnectionLostEvent?.Invoke(reason);
        }

        public Action HostReadyEvent;
        public Action ClientReadyEvent;

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

        public void SetHostReady(bool value)
        {
            if (value)
            {
                HostReadyEvent?.Invoke();
            }
            isHostReady = value;
        }

        public void SetClientReady(bool value)
        {
            if (value)
            {
                ClientReadyEvent?.Invoke();
            }
            isClientReady = value;
        }

    }
}

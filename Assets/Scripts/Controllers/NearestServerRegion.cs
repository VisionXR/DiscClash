using Fusion;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class NearestServerRegion : MonoBehaviour, IConnectionCallbacks
{
    private LoadBalancingClient loadBalancingClient;

    void Start()
    {
        //// Initialize LoadBalancingClient
        //loadBalancingClient = new LoadBalancingClient();
        //loadBalancingClient.AddCallbackTarget(this);
        //loadBalancingClient.AppId = PhotonAppSettings.Instance.AppSettings.AppIdFusion; // Set your Photon App ID here
        //loadBalancingClient.AppVersion = PhotonAppSettings.Instance.AppSettings.AppVersion; // Set your app version here

        //// Connect to the Photon Master Server to get regions
        //loadBalancingClient.ConnectToNameServer();
    }

    public void OnConnected()
    {
        Debug.Log("Connected to Photon NameServer.");
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server.");
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError($"Disconnected from Photon: {cause}");
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log("Region list received.");

        // Ping the regions to find the best one
        regionHandler.PingMinimumOfRegions((RegionHandler handler) =>
        {
            if (handler.BestRegion != null)
            {
                string bestRegionCode = handler.BestRegion.Code;
                Debug.Log("Best region: " + bestRegionCode);

                // Set the best region for Fusion
               // SetFusionRegion(bestRegionCode);
            }
            else
            {
                Debug.LogError("No best region found. Please check your internet connection and Photon configuration.");
            }
        }, null);
    }

    
    public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }

    public void OnCustomAuthenticationFailed(string debugMessage) { }

    public void OnRegionPingCompleted(RegionHandler regionHandler) { }
}

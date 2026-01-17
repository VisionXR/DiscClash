using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkState : MonoBehaviour, INetworkRunnerCallbacks
{

    public NetworkOutputSO networkOutputData;
    private List<AvailableRooms> rooms;
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log(" Connected to server ");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log(" Failed to Connected to server ");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
       
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
       
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

      //  networkData.DisconnectedFromServer(reason.ToString());
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log(" Host left the game ");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
       
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
       
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
       
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
       
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
       
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
       
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
       
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
       
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
      

        // Print custom properties for the first 10 sessions
        int sessionCount = Mathf.Min(sessionList.Count, 2);

        rooms = new List<AvailableRooms>();


        for (int i = 0; i < sessionCount; i++)
        {
            var sessionItem = sessionList[i];
            AvailableRooms roomsItem = new AvailableRooms();

            roomsItem.roomName = sessionItem.Name;

            if (sessionItem.Properties.TryGetValue("gamemode", out var gameModeProperty) && gameModeProperty.IsInt)
            {
                var gameMode = (int)gameModeProperty.PropertyValue;
                roomsItem.gameMode = (com.VisionXR.HelperClasses.MultiPlayerGameMode)gameMode;

            }
            else
            {
                Debug.Log("Gamemode property not found or not an integer.");
            }

            if (sessionItem.Properties.TryGetValue("game", out var gameProperty) && gameProperty.IsInt)
            {
                var game = (int)gameProperty.PropertyValue;
                roomsItem.game = (Game)game;

            }
            else
            {
                Debug.Log("Game property not found or not an integer.");
            }

            if (sessionItem.Properties.TryGetValue("board", out var board) && gameProperty.IsInt)
            {
                var boardId = (int)board.PropertyValue;
                roomsItem.MyBoard = boardId;

            }
            else
            {
                Debug.Log("No board id found .");
            }

            if (sessionItem.Properties.TryGetValue("difficulty", out var aiDifficulty) && gameProperty.IsInt)
            {
                var difficluty = (int)aiDifficulty.PropertyValue;
                roomsItem.aiDifficulty = (AIDifficulty)difficluty;

            }
            else
            {
                Debug.Log("No board id found .");
            }

            rooms.Add(roomsItem);
        }

        networkOutputData.SetAvailableRooms(rooms);
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log(" ShutDown by me "+ shutdownReason);
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }
}

using com.VisionXR.GameElements;        
using com.VisionXR.ModelClasses;
using Fusion;
using Photon.Voice.Unity;
using System;
using System.Collections;
using System.Text;
using UnityEngine;

namespace com.VisionXR.HelperClasses
{
    public class PlayerNetworkData : NetworkBehaviour
    {
        [Header("Scriptable Objects")]
        public NetworkInputSO networkInputData;
        
      
        [Header("local Objects")]
        public Player player;
        public ReceivePlayerData receivePlayerData;
        public Recorder myRecorder;
        // Pre-allocate this buffer once (make sure size matches Capacity)
        private byte[] _avatarBuffer = new byte[1024];



        [Header("Network Variables")]     
        [Networked, Capacity(100), OnChangedRender(nameof(OnDestroyCoinsReceived))] public string destroyCoinsData { get; set; }
        [Networked, Capacity(600), OnChangedRender(nameof(OnPlayerPropertiesReceived))] public string playerPropertiesData { get; set; }
        [Networked, OnChangedRender(nameof(OnStrikerDataReceived))][HideInInspector] public StrikerSnapshot StrikerData { get; set; }    
        [Networked, OnChangedRender(nameof(OnSnapShotReceived))][HideInInspector] public GameSnapshot gameSnapshot { get; set; }     
        [Networked, OnChangedRender(nameof(OnGameDataReceived))][HideInInspector] public NetworkGameData networkGameData { get; set; }

 
       
           
                
        public override void Spawned()
        {
            base.Spawned();

            if (!HasStateAuthority)
            {
                OnPlayerPropertiesReceived();
            }
        }


        #region setters
        public void SetPlayerData(PlayerProperties p)
        {
            playerPropertiesData = JsonUtility.ToJson(p);
        }

        public void SetStrikerData(Vector3 pos, Vector3 rot)
        {

            StrikerSnapshot updated = StrikerData;
            updated.Position = pos;
            updated.Rotation = rot;
            StrikerData = updated;

        }

        public void SetSnapShotdata(GameSnapshot newgameSnapshot)
        {
            gameSnapshot = newgameSnapshot;
        }
        public void SetGameData(CurrentGameData data)
        {
            NetworkGameData updated = networkGameData;

            updated.TotalCoins = data.TotalCoins;
            updated.TotalWhites = data.TotalWhites;
            updated.TotalBlacks = data.TotalBlacks;
            updated.TotalReds = data.TotalReds;

            updated.P1Whites = data.P1Whites;
            updated.P1Blacks = data.P1Blacks;
            updated.P1Red = data.P1Red;

            updated.P2Whites = data.P2Whites;
            updated.P2Blacks = data.P2Blacks;
            updated.P2Red = data.P2Red;

            updated.P3Whites = data.P3Whites;
            updated.P3Blacks = data.P3Blacks;
            updated.P3Red = data.P3Red;

            updated.P4Whites = data.P4Whites;
            updated.P4Blacks = data.P4Blacks;
            updated.P4Red = data.P4Red;

            updated.isRedCovered = data.isRedCovered;
            updated.ShouldICoverCoin = data.ShouldICoverCoin;
            updated.isGameCompleted = data.isGameCompleted;

            networkGameData = updated;
        }


        public void SetDestroyCoins(string data)
        {
             
            destroyCoinsData = data;    
        }


        #endregion

        #region Receivers
        public void OnPlayerPropertiesReceived()
        {

            // Adding player to data
            // local or remote player

            PlayerProperties playerProperties = JsonUtility.FromJson<PlayerProperties>(playerPropertiesData);

            if (HasStateAuthority)
            {
                playerProperties.myPlayerControl = PlayerControl.Local;

                if (playerProperties.myPlayerRole == PlayerRole.Human)
                {
                    myRecorder.RecordingEnabled = true;
                }
            }
            else
            {
                playerProperties.myPlayerControl = PlayerControl.Remote;
            }

            player.SetProperties(playerProperties);

            // recorder and speaker


        }

        private void OnStrikerDataReceived()
        {
            if (!HasStateAuthority)
            {
                receivePlayerData.ReceiveStrikerData(StrikerData.Position, StrikerData.Rotation);
            }
        }
        private void OnSnapShotReceived()
        {         
            if (!HasStateAuthority)
            {
                receivePlayerData.ReceiveSnapShot(gameSnapshot); 
            }
        }

        private void OnDestroyCoinsReceived()
        {
            if (!string.IsNullOrEmpty(destroyCoinsData))
            {               
                networkInputData.DestroyCoinsFellInThisTurn(destroyCoinsData);
            }
        }

        public void OnGameDataReceived()
        {
            CurrentGameData gameData = new CurrentGameData();

            gameData.TotalCoins = networkGameData.TotalCoins;
            gameData.TotalWhites = networkGameData.TotalWhites;
            gameData.TotalBlacks = networkGameData.TotalBlacks;
            gameData.TotalReds = networkGameData.TotalReds;

            gameData.P1Whites = networkGameData.P1Whites;
            gameData.P1Blacks = networkGameData.P1Blacks;
            gameData.P1Red = networkGameData.P1Red;

            gameData.P2Whites = networkGameData.P2Whites;
            gameData.P2Blacks = networkGameData.P2Blacks;
            gameData.P2Red = networkGameData.P2Red;

            gameData.P3Whites = networkGameData.P3Whites;
            gameData.P3Blacks = networkGameData.P3Blacks;
            gameData.P3Red = networkGameData.P3Red;

            gameData.P4Whites = networkGameData.P4Whites;
            gameData.P4Blacks = networkGameData.P4Blacks;
            gameData.P4Red = networkGameData.P4Red;

            gameData.isRedCovered = networkGameData.isRedCovered;
            gameData.ShouldICoverCoin = networkGameData.ShouldICoverCoin;
            gameData.isGameCompleted = networkGameData.isGameCompleted;

            networkInputData.SetCurrentGameData(gameData);
            
        }
       
        #endregion

        #region RPCS

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_StartGame(int id)
        {
            networkInputData.StartGame(id);
        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_PutFine(PlayerCoin coin)
        {
            networkInputData.PutFine(coin);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_PlayerReady(int id)
        {        
            networkInputData.PlayerReadyEvent?.Invoke(id); 
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_PlayerStrikeStarted(float force, Vector3 dir)
        {
            if (!HasStateAuthority)
            {
                receivePlayerData.PlayerStrikeStarted(force, dir);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_PlayerStrikeForceStarted()
        {
            if (!HasStateAuthority)
            {
                receivePlayerData.PlayerStrikeForceStarted();
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_PlayerStrikeEnded()
        {
            if (!HasStateAuthority)
            {
                receivePlayerData.PlayerStrikeEnded();
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        public void RPC_SendGameData(string data)
        {
            networkInputData.SetGameResult(JsonUtility.FromJson<GameResult>(data));                
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies, Channel = RpcChannel.Reliable)]
        public void RPC_SendAIData(string data)
        {
            if (!HasStateAuthority)
            {
                receivePlayerData.ReceiveAIData(data);
            }

        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies, Channel = RpcChannel.Reliable)]
        public void RPC_ChangeStriker(int playerId, int strikerId)
        {
            if (!HasStateAuthority)
            {
                receivePlayerData.ChangeStrikerReceived(playerId,strikerId);
            }

        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies, Channel = RpcChannel.Unreliable)]
        public void RPC_SendCoinRotation(float val)
        {
            if (!HasStateAuthority)
            {
                receivePlayerData.ReceiveCoinRotationData(val);
            }

        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies, Channel = RpcChannel.Reliable)]
        public void RPC_SendAvatarData(NetworkAvatarData data)
        {
           

        }

        #endregion

    }
}
    
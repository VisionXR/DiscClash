using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace com.VisionXR.GameElements
{
    public class SendPlayerData : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public PlayersDataSO playerData;
        public GameDataSO gameData;
        public CoinDataSO coinData;
        public TestDataSO testData;

        [Header("local Objects")]
        public Player player;
        
        public PlayerNetworkData networkData;

        // Local Variables
        public int currentFrameNumber;
        public bool canISendSnapShot;
        private Coroutine sendStrikerDataRoutine;
       


        public IEnumerator Start()
        {
            yield return new WaitForSeconds(5);
            if (player.myPlayerControl == PlayerControl.Local && player.myPlayerRole == PlayerRole.Human)
            {
                StartCoroutine(SendAvatarData());
            }
        }


        private void OnEnable()
        {
            gameData.TurnChangedEvent += OnTurnChanged;
            player.strikeStartedEvent += PlayerStrikeStarted;
            player.strikeEndedEvent += PlayerStrikeEnded;
            player.strikeForceStartedEvent += PlayerStrikeForceStarted;
            player.aIMovementEvent += AIMoved;
            player.AllCoinsRotatedEvent += CoinsRotated;
        }

        private void OnDisable()
        {
            gameData.TurnChangedEvent -= OnTurnChanged;
            player.strikeStartedEvent -= PlayerStrikeStarted;   
            player.strikeEndedEvent -= PlayerStrikeEnded;
            player.strikeForceStartedEvent -= PlayerStrikeForceStarted;
            player.aIMovementEvent -= AIMoved;
            player.AllCoinsRotatedEvent -= CoinsRotated;
            StopAllCoroutines();
        }

        private void CoinsRotated(float val)
        {
           networkData.RPC_SendCoinRotation(val);
        }

        private void AIMoved(string data)
        {
            networkData.RPC_SendAIData(data);
        }

        private void PlayerStrikeForceStarted()
        {
            networkData.RPC_PlayerStrikeForceStarted();
        }


        private void PlayerStrikeStarted(float force, Vector3 dir)
        {
            currentFrameNumber = 1;
            canISendSnapShot = true;

            if (sendStrikerDataRoutine != null)
            {
                StopCoroutine(sendStrikerDataRoutine);
                sendStrikerDataRoutine = null;
            }

            networkData.RPC_PlayerStrikeStarted(force, dir);
            
        }
            
        private void PlayerStrikeEnded()
        {
            currentFrameNumber = 1;
            canISendSnapShot = false;
            networkData.RPC_PlayerStrikeEnded();            
        }

        private void OnTurnChanged(int currentTurnId)
        {
            if (player.myId == currentTurnId)
            {
                if (player.myPlayerControl == PlayerControl.Local && player.myPlayerRole == PlayerRole.Human)
                {
                    if (sendStrikerDataRoutine == null)
                    {
                        sendStrikerDataRoutine = StartCoroutine(SendStrikerData());
                    }
                }
            }
        }

        private IEnumerator SendStrikerData()
        {
            while (true)
            {
                networkData.SetStrikerData(player.strikerRigidBody.position, player.strikerRigidBody.transform.eulerAngles);
                yield return new WaitForSeconds(playerData.SendRate * Time.fixedDeltaTime);
            }
        }

        private void FixedUpdate()
        {
            // sending
            SendData();
        }

        public void SendData()
        {
            if (canISendSnapShot)
            {
                if (currentFrameNumber % playerData.SendRate == 0)
                {
                    // Send snapshot as JSON
                
                    networkData.SetSnapShotdata(GetGameSnapshotData());    
                }
                currentFrameNumber++;
            }

        }

        public GameSnapshot GetGameSnapshotData()
        {
            GameSnapshot snapshot = new GameSnapshot
            {
                FrameNumber = currentFrameNumber
            };

            // Set striker data
            Rigidbody strikerRb = player.strikerRigidBody;
            snapshot.Striker = new CoinSnapshot
            {
                Position = strikerRb.position,
                Rotation = strikerRb.rotation.eulerAngles,
                Velocity = strikerRb.linearVelocity,
                
            };

            // Set coin data
            List<Rigidbody> coinRbs = coinData.AvailableCoinsInGame;
            for (int i = 0; i < coinRbs.Count && i < snapshot.Coins.Length; i++)
            {
                Rigidbody rb = coinRbs[i];
                snapshot.Coins.Set(i, new CoinSnapshot
                {
                    Position = rb.position,
                    Rotation = rb.rotation.eulerAngles,
                    Velocity = rb.linearVelocity,
                    

                }); ;
            }

            return snapshot;
        }

        public IEnumerator SendAvatarData()
        {
            while (true)
            {
                yield return new WaitForSeconds(4.0f * Time.fixedDeltaTime);
             
            }
        }

    }

}
                                                                        
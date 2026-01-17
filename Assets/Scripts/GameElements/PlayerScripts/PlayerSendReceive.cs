using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.VisionXR.GameElements
{
    public class PlayerSendReceive : MonoBehaviour
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
        private Coroutine sendStrikerDataRoutine;
        public int currentFrameNumber;
        public bool canISendSnapShot;


        private void OnEnable()
        {
            gameData.TurnChangedEvent += OnTurnChanged;
            player.strikeStartedEvent += PlayerStrikeStarted;
            playerData.PlayerStrikeFinishedEvent += PlayerStrikeFinished;
        }

        private void OnDisable()
        {
            gameData.TurnChangedEvent -= OnTurnChanged;
            player.strikeStartedEvent -= PlayerStrikeStarted;
            playerData.PlayerStrikeFinishedEvent -= PlayerStrikeFinished;
        }

        private void PlayerStrikeStarted(float force,Vector3 dir)
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

        private void PlayerStrikeFinished(int id)
        {

            currentFrameNumber = 1;
            canISendSnapShot = false;

            if (id == player.myId)
            {
                networkData.RPC_PlayerStrikeEnded();
            }
        }

        private void OnTurnChanged(int currentTurnId)
        {                   
            if (player.myId == currentTurnId)
            {
                if (player.myPlayerControl == PlayerControl.Local)
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
                   // networkData.SetSnapShotdata(GetSnapShotString());  
                }

                currentFrameNumber++;
            }

        }
        
        public string GetSnapShotString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            // Add frame number
            sb.Append(currentFrameNumber);
            sb.Append("&");

            // Encode striker data: pos&rot&vel
            Rigidbody strikerRb = player.strikerRigidBody;
            sb.Append(DataConverter.EncodeTransform(strikerRb.position));
            sb.Append("&");
            sb.Append(DataConverter.EncodeTransform(strikerRb.transform.eulerAngles));
            sb.Append("&");
            sb.Append(DataConverter.EncodeTransform(strikerRb.linearVelocity));
            sb.Append("&");

            // Encode all coin data: pos&rot&vel#pos&rot&vel#...
            List<Rigidbody> coinRbs = coinData.AvailableCoinsInGame;
            for (int i = 0; i < coinRbs.Count; i++)
            {
                Rigidbody rb = coinRbs[i];
                sb.Append(DataConverter.EncodeTransform(rb.position));
                sb.Append("&");
                sb.Append(DataConverter.EncodeTransform(rb.transform.eulerAngles));
                sb.Append("&");
                sb.Append(DataConverter.EncodeTransform(rb.linearVelocity));

                if (i < coinRbs.Count - 1)
                {
                    sb.Append("#");
                }
            }

            return sb.ToString();
        }

       


    }


}
                
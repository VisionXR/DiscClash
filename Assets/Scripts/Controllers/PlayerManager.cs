using UnityEngine;
using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;
using System;
using com.VisionXR.GameElements;
using NUnit.Framework;
using System.Collections.Generic;

namespace com.VisionXR.Controllers
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public PlayersDataSO playersData;
        public UIOutputDataSO uiOutputData;  
        public UIInputDataSO uiInputData;   
        public MyPlayerSettings playerSettings;
        

        [Header("GameObjects")]
        public GameObject playerObject;


        [Header("Actions")]
        public Action<bool> PlayerWriteStatusEvent;
        public Action<string> PlayerReadStatusEvent;
        private void OnEnable()
        {
            playersData.CreatePlayerEvent += CreatePlayer;
            playersData.DestroyPlayerEvent += DestroyPlayer;

            playersData.CreateSinglePlayersEvent += CreatePlayersForSinglePlayer;
            playersData.DestroyAllPlayersEvent += DestroyAllPlayers;

    
        }

        private void OnDisable()
        {
            playersData.CreatePlayerEvent -= CreatePlayer;
            playersData.DestroyPlayerEvent -= DestroyPlayer;

            playersData.CreateSinglePlayersEvent -= CreatePlayersForSinglePlayer;
            playersData.DestroyAllPlayersEvent -= DestroyAllPlayers;
            
        }

  
        public void CreatePlayer(PlayerProperties properties)
        {
            GameObject newPlayer = Instantiate(playerObject, transform.position, transform.rotation);
            if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
            {
                if(uiOutputData.challenge == Challenge.BlackAndWhite)
                {
                    if(properties.myId == 1)
                    {
                        properties.myCoin = PlayerCoin.White;
                    }
                    else
                    {
                        properties.myCoin = PlayerCoin.Black;
                    }
                }
                else if(uiOutputData.challenge == Challenge.FreeStyle)
                {
                    if (properties.myId == 1)
                    {
                        properties.myCoin = PlayerCoin.All;
                    }
                    else
                    {
                        properties.myCoin = PlayerCoin.All;
                    }
                }
            
            }
            else if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PAIvsAI)
            {
                if (uiOutputData.challenge == Challenge.BlackAndWhite)
                {
                    if (properties.myTeam == Team.TeamA)
                    {
                        properties.myCoin = PlayerCoin.White;
                    }
                    else
                    {
                        properties.myCoin = PlayerCoin.Black;
                    }
                }
                else if (uiOutputData.challenge == Challenge.FreeStyle)
                {
                    if (properties.myTeam == Team.TeamA)
                    {
                        properties.myCoin = PlayerCoin.All;
                    }
                    else
                    {
                        properties.myCoin = PlayerCoin.All;
                    }
                }
            }
            newPlayer.GetComponent<Player>().SetProperties(properties);
        }

        public void CreatePlayersForSinglePlayer()
        {
            if(uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)        
            {

                    PlayerProperties p1 = new PlayerProperties();
                    p1.myId = 1;
                    p1.myOculusID = playerSettings.MyId ;
                    p1.myName = playerSettings.MyName;
                    p1.myStrikerID = playerSettings.MyStrikerId;
                    
               
                    p1.myPlayerControl = PlayerControl.Local;
                    p1.myPlayerRole = PlayerRole.Human;
                    p1.myTeam = Team.TeamA;
                    p1.myAiDifficulty = uiOutputData.aIDifficulty;

                    PlayerProperties p2 = new PlayerProperties();
                    p2.myId = 2;
                    p2.myOculusID = playerSettings.MyId;
                    p2.myName = "AI2";
                    p2.myStrikerID = 0;

              
                    p2.myPlayerControl = PlayerControl.Local;
                    p2.myPlayerRole = PlayerRole.AI;
                    p2.myTeam = Team.TeamB;
                    p2.myAiDifficulty = uiOutputData.aIDifficulty;

                    CreatePlayer(p1);
                    CreatePlayer(p2);

            }
            else if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PAIvsAI)
            {

                    PlayerProperties p1 = new PlayerProperties();
                    p1.myId = 1;
                    p1.myOculusID = playerSettings.MyId;
                    p1.myName = playerSettings.MyName;
                    p1.myStrikerID = playerSettings.MyStrikerId;
                  
                    p1.myPlayerControl = PlayerControl.Local;
                    p1.myPlayerRole = PlayerRole.Human;
                    p1.myTeam = Team.TeamA;
                    p1.myAiDifficulty = uiOutputData.aIDifficulty;

                    PlayerProperties p2 = new PlayerProperties();
                    p2.myId = 2;
                    p2.myOculusID = playerSettings.MyId;
                    p2.myName = "AI2";
                    p2.myStrikerID = 0;
                
                    p2.myPlayerControl = PlayerControl.Local;
                    p2.myPlayerRole = PlayerRole.AI;
                    p2.myTeam = Team.TeamA;
                    p2.myAiDifficulty = uiOutputData.aIDifficulty;

                    PlayerProperties p3 = new PlayerProperties();
                    p3.myId = 3;
                    p3.myOculusID = playerSettings.MyId;
                    p3.myName = "AI3";
                    p3.myStrikerID = 0;

                   
                    p3.myPlayerControl = PlayerControl.Local;
                    p3.myPlayerRole = PlayerRole.AI;
                    p3.myTeam = Team.TeamB;
                    p3.myAiDifficulty = uiOutputData.aIDifficulty;

                    PlayerProperties p4 = new PlayerProperties();
                    p4.myId = 4;
                    p4.myOculusID = playerSettings.MyId;
                    p4.myName = "AI4";
                    p4.myStrikerID = 0;
                 
                    p4.myPlayerControl = PlayerControl.Local;
                    p4.myPlayerRole = PlayerRole.AI;
                    p4.myTeam = Team.TeamB;
                    p4.myAiDifficulty = uiOutputData.aIDifficulty;

                    CreatePlayer(p1);
                    CreatePlayer(p2);
                    CreatePlayer(p3);
                    CreatePlayer(p4);
                 
            }

        }


        public void DestroyPlayer(int id)
        {
            foreach(Player p in playersData.CurrentPlayers)
            {
                if(p.myId == id)
                {
                    Destroy(p.gameObject);
                    break;
                }
            }
        }

        public void DestroyAllPlayers()
        {
            // Iterate in reverse to safely remove during iteration
            for (int i = playersData.CurrentPlayers.Count - 1; i >= 0; i--)
            {
                Player p = playersData.CurrentPlayers[i];
                if (p != null && p.gameObject != null)
                {
                    Destroy(p.gameObject);
                }
            }

            playersData.CurrentPlayers.Clear();
        }

    }
}




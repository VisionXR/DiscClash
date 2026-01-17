using System;
using UnityEngine;
using com.VisionXR.GameElements;
using System.Collections.Generic;
using com.VisionXR.HelperClasses;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "PlayersDataSO", menuName = "ScriptableObjects/PlayersDataSO", order = 1)]
    public class PlayersDataSO : ScriptableObject
    {
        // variables
        public int SendRate = 7;
        public int DelayRate = 4;
        public float strikerK = 1.457f;
        public float strikerangularK = 1.457f;
        public float coinK = 1.391f;
        public float coinangularK = 1.391f;
        public float k = 1.25f;
        public float strikerKNew = 1.457f;
        public float coinKNew = 1.391f;
        public List<Player> CurrentPlayers = new List<Player>();

        // Events
        public Action<PlayerProperties> CreatePlayerEvent;
        public Action CreateSinglePlayersEvent;
        public Action<int> DestroyPlayerEvent;
        public Action DestroyAllPlayersEvent;


        public Action<int, float> PlayerStrikeStartedEvent;
        public Action<int> PlayerStrikeFinishedEvent;

        public Action<Player> PlayerJoinedEvent;
        public Action<Player> PlayerLeftEvent;


        public Action PlayerImageLoadedEvent;

        // Methods

        private void OnEnable()
        {
            CurrentPlayers.Clear();
        }

        public void CreatePlayer(PlayerProperties playerProperties)
        {
            CreatePlayerEvent?.Invoke(playerProperties);
        }

        public void CreateSinglePlayers()
        {
            CreateSinglePlayersEvent?.Invoke();
        }
        public void DestroyPlayer(int id)
        {
             DestroyPlayerEvent?.Invoke(id);
        }

        public void DestroyAllPlayers()
        {
            DestroyAllPlayersEvent?.Invoke();
        }


        public Player GetPlayer(int id)
        {
            foreach (Player p in CurrentPlayers)
            {
                if (p.myId == id)
                {
                    return p;
                }
            }

            return null;
        }

        public Player GetMainPlayer()
        {
            foreach (Player p in CurrentPlayers)
            {
                         
                if (p.myPlayerRole == PlayerRole.Human && p.myPlayerControl == PlayerControl.Local)
                {
                     return p;
                }
                
            }

            return null;
        }

        public Player GetOtherPlayer()
        {

            foreach (Player p in CurrentPlayers)
            {
                
                    if (p.myPlayerRole == PlayerRole.Human && p.myPlayerControl == PlayerControl.Remote)
                    {
                        return p;
                    }
                
            }
            return null;
        }
        public Player GetPlayer(PlayerCoin coin)
        {
            foreach (Player p in CurrentPlayers)
            {
                if (p.myCoin == coin)
                {
                    return p;
                }
            }

            return null;
        }

        public int NoOfPlayers()
        {
            return CurrentPlayers.Count;
        }

        public void AddPlayer(Player p)
        {           
            CurrentPlayers.Add(p);
            PlayerJoinedEvent?.Invoke(p);
        }

        public void RemovePlayer(Player p)
        {
            if (CurrentPlayers.Contains(p))
            {             
                PlayerLeftEvent?.Invoke(p);
                CurrentPlayers.Remove(p);
                CurrentPlayers.RemoveAll(go => go == null);
            }
        }


        public void PlayerImageLoaded()
        {
            PlayerImageLoadedEvent?.Invoke();
        }
    }
}

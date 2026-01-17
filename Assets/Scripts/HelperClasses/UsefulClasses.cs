using System.Collections.Generic;
using System;
using UnityEngine;
using Fusion;

namespace com.VisionXR.HelperClasses
{
    [Serializable]
    public class DestroyCoinsList
    {
        public List<string> coins = new List<string>();
    }


    [Serializable]
    public class NetworkPlayer
    {
         public int myId;
         public ulong myOculusID;
         public int myStrikerId;
         public string myName;
         public PlayerCoin myCoin;
         public Team myTeam;
         public PlayerRole myPlayerRole;
         public AIDifficulty aIDifficulty;
         public string imageURL;
         
    }


    [Serializable]
    public class CoinData
    {
       
        public string Position;
        public string Rotation;
  
    }

    [Serializable]
    public class TransformData
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }
    [Serializable]
    public class StrikerData
    {
       
        public string Position;
        public string Rotation;
     
    }

    [Serializable]
    public class SnapShotData
    {
        public int frameNumber;
        public List<coinSnapShotData> allCoinsSnapShotData;
        public coinSnapShotData strikerSnapShotData;
    }

    [Serializable]
    public class coinSnapShotData
    {
        
        public string Position;
        public string Rotation;
        public string Velocity;

    }

        [Serializable]
    public class AudioData
    {       
        public AudioType audioType;
        public float volume = 1;

    }

    [Serializable]
    public class CurrentGameData
    {
      
        public int TotalCoins;
        public int TotalWhites;
        public int TotalBlacks;
        public int TotalReds;
        public int P1Whites;
        public int P1Blacks;
        public int P1Red;
        public int P2Whites;
        public int P2Blacks;
        public int P2Red;
        public int P3Whites;
        public int P3Blacks;
        public int P3Red;
        public int P4Whites;
        public int P4Blacks;
        public int P4Red;
        public bool isRedCovered;
        public bool ShouldICoverCoin;
        public bool isGameCompleted;
        public int currentTurnId;
      
    }

    [Serializable]
    public class StartGame
    {
        public string time;
        public int id;
    }


    [Serializable]
    public class GameResult
    {
        public int currentTurnId;
        public int winningPlayerId;
        public Team winningTeam;
        public bool isVictory;
    }

    [SerializeField]
    public class AIBotAnimationDetails
    {
        public string time;
        public int myId;
        public int eventId;
        public string coinPosition;
        public string strikerPosition;
        public string strikerRotation;
    }
 

    [Serializable]
    public class PlayerData
    {
        public DominantHand dominantHand;
        public int StrikerId;
        public int BoardId;
        public bool isPassThroughOn;
        public ServerRegion region;

    }
    [Serializable]
    public class AvatarData
    {
        public byte[] avatarInfo;

    }
    [Serializable]
    public class NotificationMessage
    {
        public ServerRegion region;
        public MultiPlayerGameMode multiPlayerGameMode;
        public Game game;
        public int MyBoard;
        public AIDifficulty difficulty;
        public string roomName;
        public string playerName;
    }

    [Serializable]
    public class Destination
    {
        public ServerRegion region;
        public GameType gameType;
        public Game game;
        public MultiPlayerGameMode multiPlayerGameMode;
        public SinglePlayerGameMode singlePlayerGameMode;
        public string lobbyName;
        public string roomName;
        public bool isJoinable;

     

        public ServerRegion GetRegion()
        {
            return Enum.TryParse(lobbyName, true, out ServerRegion serverRegion) ? serverRegion : ServerRegion.any;
        }

    }


    [Serializable]
    public class LinkData
    {
        public GameType gameType;
        public Game game;
        public MultiPlayerGameMode multiPlayerGameMode;
        public SinglePlayerGameMode singlePlayerGameMode;
    }


    [Serializable]
    public class AssetData
    {
        public string skuName;
        public bool isPurchased = false;
        public string Price;
    }

    [Serializable]
    public class ProfileImage
    {
        public Sprite image;
    }

        [Serializable]
    public class Friend
    {
        public ulong FriendID;
        public string FriendName;
       
    }

    [Serializable]
    public class AvailableRooms
    {
        public string  roomName;
        public MultiPlayerGameMode gameMode;
        public Game game;
        public int MyBoard;
        public AIDifficulty aiDifficulty;
    }

    [Serializable]
    public class PlayerProperties
    {
        public int myId;
        public int myStrikerID = 1;
        public ulong myOculusID;
        public string imageURL;
        public string myName;

        public PlayerControl myPlayerControl;
        public PlayerRole myPlayerRole;
        public PlayerCoin myCoin;
        public Team myTeam;
        public AIDifficulty myAiDifficulty;
    }

}

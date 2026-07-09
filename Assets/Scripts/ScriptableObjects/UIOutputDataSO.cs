using com.VisionXR.HelperClasses;
using System;
using UnityEngine;
using UnityEngine.Rendering;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "UIOutputDataSO", menuName = "ScriptableObjects/UIOutputDataSO", order = 1)]
    public class UIOutputDataSO : ScriptableObject
    {
        // variables
        public GameType gameType;
        public GameMode gameMode;
        public Challenge challenge;
        public AIDifficulty aIDifficulty;
        public PlayerCoin playerCoin;
        public RoomType roomType;
        public ServerRegion region;
        public int NoOfPlayers = 2;
        public RoomJoinType roomJoinType;

        public int MyBoardId;
        public int MyCoinsId;
        public int MyStrikerId;

        // coin Images
        public Sprite WhiteCoin;
        public Sprite BlackCoin;
        public Sprite RedCoin;
        public Sprite BlackAndWhiteCoin;


        // Board And Coins Events
        public Action<int> SetMyCoinsIdEvent;
        public Action<int> SetMyBoardEvent;
        public Action CoinsSetEvent;


        // Methods

        public void SetRoomJoinType(RoomJoinType roomJoinType)
        {
            this.roomJoinType = roomJoinType;
        }
        public void SetPlayerCount(int total)
        {
            NoOfPlayers = total;
        }

        public void SetGameType(GameType gameType)
        {
            this.gameType = gameType;
        }


        public void SetChallenge(Challenge challenge)
        {
            this.challenge = challenge;
        }

        public void SetGameMode(GameMode gameMode)
        {
            this.gameMode = gameMode;
        }

        public void SetRoomType(RoomType roomType)
        {
            this.roomType = roomType;
        }

        public void SetAIDifficulty(AIDifficulty aIDifficulty)
        {
            this.aIDifficulty = aIDifficulty;
        }
        public void SetPlayerCoin(PlayerCoin playerCoin)
        {
            this.playerCoin = playerCoin;
        }

        public void SetCoinImages(Sprite white, Sprite black, Sprite Red, Sprite blackAndwhite)
        {
            WhiteCoin = white;
            BlackCoin = black;
            RedCoin = Red;
            BlackAndWhiteCoin = blackAndwhite;
            CoinsSetEvent?.Invoke();
        }

        public void SetMyBoardId(int boardId)
        {
            MyBoardId = boardId;
            SetMyBoardEvent?.Invoke(boardId);
        }

        public void SetMyStrikerId(int strikerId)
        {
            MyStrikerId = strikerId;
        }

        public void SetMyCoinsId(int coinsId)
        {
            MyCoinsId = coinsId;             
        }

    }
}

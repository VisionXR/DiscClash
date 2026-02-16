using com.VisionXR.HelperClasses;
using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "UIOutputDataSO", menuName = "ScriptableObjects/UIOutputDataSO", order = 1)]
    public class UIOutputDataSO : ScriptableObject
    {
        // variables
        public GameType gameType;
        public Game game;
        public MultiPlayerGameMode multiPlayerGameMode;
        public SinglePlayerGameMode singlePlayerGameMode;
        public AIDifficulty aIDifficulty;
        public PlayerCoin playerCoin;
        public RoomType roomType;
        public ServerRegion region;
        public int NoOfPlayers = 2;

        public int MyBoard;
        public int MyCoinsId;

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
        public void SetPlayerCount(int total)
        {
            NoOfPlayers = total;
        }

        public void SetGameType(GameType gameType)
        {
            this.gameType = gameType;
        }
        public void SetGame(Game game)
        {
            this.game = game;
        }

        public void SetRoomType(RoomType roomType)
        {
            this.roomType = roomType;
        }
        public void SetGameMode(MultiPlayerGameMode gameMode)
        {
            multiPlayerGameMode = gameMode;
        }
        public void SetSingleGameMode(SinglePlayerGameMode singleGameMode)
        {
            singlePlayerGameMode = singleGameMode;
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

    }
}

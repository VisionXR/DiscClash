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
        public bool isPlaying = false;
        public ServerRegion region;
        public int MyCoinsId = 0;
        public int MyBoard = 0;
        public int NoOfPlayers = 2;

        public Sprite WhiteCoin;
        public Sprite BlackCoin;
        public Sprite RedCoin;
        public Sprite BlackAndWhiteCoin;
        // Events


        public Action StartSinglePlayerGameEvent;
        public Action StartMultiPlayerGameEvent;
        public Action StartTutorialEvent;
        public Action StartTrickShotsEvent;
      
        public Action<Destination> StartFTUEEvent;
        public Action EndTutorialEvent;
        public Action StopTrickShotsEvent;

        public Action<int> PlayerReadyEvent;
        public Action<GameResult> ShowGameResultEvent;

        public Action ExitGameEvent;
        public Action HomeEvent;
        public Action PlayAgainEvent;

        // Mic And Speaker Events
        public Action TurnOnMicEvent;
        public Action TurnOffMicEvent;
        public Action TurnOnSpeakerEvent;
        public Action TurnOffSpeakerEvent;

        // Board And Coins Events
        public Action<int> SetMyCoinsIdEvent;
        public Action<int> SetMyBoardEvent;
        public Action CoinsSetEvent;

        // Methods

        public void SetCoinImages(Sprite white, Sprite black, Sprite Red, Sprite blackAndwhite)
        {
            WhiteCoin = white;
            BlackCoin = black;
            RedCoin = Red;
            BlackAndWhiteCoin = blackAndwhite;
            CoinsSetEvent?.Invoke();
        }

        public void SetPlayerCount(int total)
        {
            NoOfPlayers = total;
        }

        public void SetMyCoinsId(int id)
        {
            MyCoinsId = id;
            SetMyCoinsIdEvent?.Invoke(id);
        }

        public void SetMyBoard(int id)
        {

            MyBoard = id;
            SetMyBoardEvent?.Invoke(id);
        }
        public void StartSinglePlayerGame()
        {
            StartSinglePlayerGameEvent?.Invoke();
        }

        public void StartTrickShots()
        {
            StartTrickShotsEvent?.Invoke();
        }
        public void StartMultiPlayerGame()
        {
            StartMultiPlayerGameEvent?.Invoke();
        }

        public void StartTutorial()
        {
            StartTutorialEvent?.Invoke();
        }

        public void StartFTUE(Destination destination)
        {
            StartFTUEEvent?.Invoke(destination);
        }

        public void EndTutorial()
        {
            EndTutorialEvent?.Invoke();
        }

        public void GameCompleted(GameResult gameResult)
        {
            ShowGameResultEvent?.Invoke(gameResult);
        }

        public void ExitGame()
        {
            ExitGameEvent?.Invoke();
        }

        public void GoToHome()
        {
            HomeEvent?.Invoke();
        }

        public void PlayAgain()
        {
            PlayAgainEvent?.Invoke();
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


        public void PlayerReady(int id)
        {
            PlayerReadyEvent?.Invoke(id);
        }

        public void TurnOnMic()
        {
            TurnOnMicEvent?.Invoke();
        }
        public void TurnOffMic()
        {
            TurnOffMicEvent?.Invoke();
        }

        public void TurnOnSpeaker()
        {
            TurnOnSpeakerEvent?.Invoke();
        }

        public void TurnOffSpeaker()
        {
            TurnOffSpeakerEvent?.Invoke();
        }

        public void SetIsPlaying(bool val)
        {
            isPlaying = val;
        }

    
    }
}

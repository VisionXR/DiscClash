using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.VisionXR.Controllers
{
    public class MultiplayerManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public NetworkInputSO networkInputData;
        public NetworkOutputSO networkOutputData;
        public PlayersDataSO playersData;
        public CoinDataSO coinData;
        public StrikerDataSO strikerData;
        public GameDataSO gameData;
        public UIOutputDataSO uiOutputData;
        public InputDataSO inputData;
        public MyPlayerSettings playerSettings;
        public BoardPropertiesSO boardProperties;
        public LeaderBoardSO leaderBoard;
        public AchievementsDataSO achievementsData;
        

        [Header("Scripts")]
        public BlackAndWhiteLogic blackAndWhiteLogic;
        public FreeStyleLogic freeStyleLogic;
        public FineLogic fineLogic;
        public DataManager dataManager;
        public MultiPlayerConnectionDisconnection connectionDisconnection;
        private bool isFirstTurn = false;

        [Header("Local")]
        public ParticleSystem winPs1;
        public ParticleSystem winPs2;
        private Coroutine turnTimeRoutine;

        private void OnEnable()
        {
            uiOutputData.SetIsPlaying(true);

            uiOutputData.ExitGameEvent += OnExitGame;
            uiOutputData.HomeEvent += OnExitGame;
           
            playersData.PlayerStrikeStartedEvent += StrikeStarted;
            playersData.PlayerStrikeFinishedEvent += StrikeFinished;

            fineLogic.PutFineEvent += PutFine;

            networkInputData.StartGameEvent += StartGame;

            networkInputData.CurrentGameDataReceivedEvent += GameDataReceived;
            networkInputData.DestroyCoinsFellInThisTurnEvent += DestroyCoinsFellInThisTurn;
            networkInputData.GameResultReceivedEvent += GameResultReceived;
            networkInputData.PutFineEvent += ReceiveFine;

            gameData.TurnChangedEvent += TurnChanged;

            strikerData.ChangeStrikerEvent += ChangeStriker;

        }



        private void OnDisable()
        {
      
            uiOutputData.ExitGameEvent -= OnExitGame;
            uiOutputData.HomeEvent -= OnExitGame;
           

            playersData.PlayerStrikeStartedEvent -= StrikeStarted;
            playersData.PlayerStrikeFinishedEvent -= StrikeFinished;

            fineLogic.PutFineEvent -= PutFine;

            networkInputData.StartGameEvent -= StartGame;

            networkInputData.CurrentGameDataReceivedEvent -= GameDataReceived;
            networkInputData.GameResultReceivedEvent -= GameResultReceived;
            networkInputData.DestroyCoinsFellInThisTurnEvent -= DestroyCoinsFellInThisTurn;
            networkInputData.PutFineEvent -= ReceiveFine;


            gameData.TurnChangedEvent -= TurnChanged;
            strikerData.ChangeStrikerEvent -= ChangeStriker;
        }

        private void ChangeStriker(int playerId, int strikerId)
        {
            dataManager.SendStrikerChange(playerId, strikerId);
        }

        private void TurnChanged(int id)
        {
            if (turnTimeRoutine == null)
            {
                turnTimeRoutine = StartCoroutine(TurnTimeRoutine(id));
            }
            else
            {
                StopCoroutine(turnTimeRoutine);
                turnTimeRoutine = StartCoroutine(TurnTimeRoutine(id));
            }
        }

        // Add this coroutine to fix CS0103: The name 'TurnTimeRoutine' does not exist in the current context
        private IEnumerator TurnTimeRoutine(int id)
        {
            // Example: Wait for a fixed turn time (e.g., 30 seconds)
            float turnTime = 45f;
            float elapsed = 0f;
            while (elapsed < turnTime)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
          
            turnTimeRoutine = null;

            if(networkOutputData.IsHost())
            {
                GameResult result = new GameResult();
                result.isVictory = false;
                result.currentTurnId = NextTurn();
                dataManager.SendGameResult(result);
            }
        }


        public void StartGame(int id)
        {
            gameData.ResetData();
            coinData.ResetData();
            coinData.ResetCount();
            strikerData.ResetFoul();
            coinData.CreateAllCoins();

            if(networkOutputData.IsHost())
            {
                GameResult result = new GameResult();
                result.currentTurnId = id;
                dataManager.SendGameResult(result);

                if (!Application.isEditor)
                {
                    achievementsData.MultiPlayerGameStart();
                }

                if (id == 1)
                {
                    Player p = playersData.GetMainPlayer();
                    p.GetComponent<PlayerInput>().StartRotation();
                    coinData.ShowRotationCanvasEvent?.Invoke();
                    isFirstTurn = true;
                }
            }
        }
    
        private void StrikeStarted(int id, float f)
        {         
            inputData.DeactivateInput();
            if (isFirstTurn)
            {
                Player p = playersData.GetMainPlayer();
                p.GetComponent<PlayerInput>().StopRotation();
                isFirstTurn = false;
            }

            if (turnTimeRoutine != null)
            {
                StopCoroutine(turnTimeRoutine);
                turnTimeRoutine = null;
            }
        }
        private void StrikeFinished(int id)
        {         
            StartCoroutine(WaitAndProcessData());
        }

        private void DestroyCoinsFellInThisTurn(string data)
        {
            coinData.DestroyCoinsFellInThisTurnEvent(DecodeList(data));
        }

        private IEnumerator WaitAndProcessData()
        {
            
            List<string> coinsFell = coinData.GetCoinsFellInThisTurn();
            dataManager.SendDestroyCoinsInThisTurn(EncodeList(coinsFell));
            yield return new WaitForSeconds(0.5f);
            ProcessPlayerData(playersData.GetPlayer(gameData.currentTurnId), coinData.Whites, coinData.Blacks, coinData.Red, strikerData.isFoul);
        }

        public string EncodeList(List<string> list)
        {
            return string.Join("|", list);
        }

        public List<string> DecodeList(string encoded)
        {
            if (string.IsNullOrEmpty(encoded)) return new List<string>();
            return new List<string>(encoded.Split('|'));
        }


        private void PutFine(PlayerCoin coin)
        {
            dataManager.SendFine(coin);
        }


        private void GameDataReceived(CurrentGameData data)
        {
            gameData.SetCurrentGameData(data);
        }

        private void GameResultReceived(GameResult result)
        {
            coinData.ResetData();
            strikerData.ResetFoul();
            if(result.isVictory)
            {
                HandleVictory(result);
                
            }

            else
            {
                gameData.ChangeTurn(result.currentTurnId);
            }
        }
        private void ReceiveFine(PlayerCoin coin)
        {
            coinData.CreateCoin(coin);
        }

        public void ProcessPlayerData(Player p, int Whites, int Blacks, int Red, bool isFoul)
        {
            IncrementScore(p, Whites, Blacks, Red, isFoul);
           
            UpdateGameData(Whites, Blacks, Red);

            bool ShouldIContinueTurn = DeterminePlayerTurn(p, Whites, Blacks, Red, isFoul);

            fineLogic.CheckFine(p, Whites, Blacks, Red, isFoul);

            GameResult gameResult = CheckGameResult(p);

            int turnId = 1;
            if (ShouldIContinueTurn)
            {
                turnId = gameData.currentTurnId;

            }
            else
            {
                turnId = NextTurn();

            }

            gameResult.currentTurnId = turnId;

            StartCoroutine(SendGameResultToOthers(gameResult)); 

        }

        private IEnumerator SendGameResultToOthers(GameResult result)
        {
            dataManager.SendGameData(gameData.GetCurrentGameData());
            yield return new WaitForSeconds(0.5f);
            dataManager.SendGameResult(result);
        }
        private bool DeterminePlayerTurn(Player p, int Whites, int Blacks, int Red, bool isFoul)
        {
            if (uiOutputData.game == Game.BlackAndWhite)
            {
                return blackAndWhiteLogic.ShouldPlayerContinueTurn(p, Whites, Blacks, Red, isFoul);
            }
            else
            {
                return freeStyleLogic.ShouldPlayerContinueTurn(p, Whites, Blacks, Red, isFoul);
            }
        }
        public void IncrementScore(Player p, int Whites, int Blacks, int Red, bool isFoul)
        {
            if (p.myId == 1)
            {
                gameData.P1Whites += Whites;
                gameData.P1Blacks += Blacks;
                gameData.P1Red += Red;
            }
            else if (p.myId == 2)
            {
                gameData.P2Whites += Whites;
                gameData.P2Blacks += Blacks;
                gameData.P2Red += Red;
            }
            else if (p.myId == 3)
            {
                gameData.P3Whites += Whites;
                gameData.P3Blacks += Blacks;
                gameData.P3Red += Red;
            }
            else if (p.myId == 4)
            {
                gameData.P4Whites += Whites;
                gameData.P4Blacks += Blacks;
                gameData.P4Red += Red;
            }
        }
        private void UpdateGameData(int Whites, int Blacks, int Red)
        {
            gameData.TotalWhites -= Whites;
            gameData.TotalBlacks -= Blacks;
            gameData.TotalReds -= Red;
        }
        private GameResult CheckGameResult(Player p)
        {
            if (uiOutputData.game == Game.BlackAndWhite)
            {
                return blackAndWhiteLogic.CheckWinningCondition(p);
            }
            else if (uiOutputData.game == Game.FreeStyle)
            {
                return freeStyleLogic.CheckWinningCondition(p);
            }
            else
            {
                return new GameResult { isVictory = false }; // Default GameResult, adjust as needed
            }

        }
     
        /// <summary>
        /// Determines the next player's turn, handling cases where the current player has left.
        /// </summary>
        /// <returns>The myId of the next valid player.</returns>
        public int NextTurn()
        {
            int id = gameData.currentTurnId;
          
            if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2) // Two-player mode
            {
                if(id == 1)
                {
                    id = 2;
                }
                else
                {
                    id = 1;
                }
            }
            else  // Four-player mode
            {
                // Loop through player IDs 1, 3, 2, 4
                if (id == 1) id = 3;
                else if (id == 3) id = 2;
                else if (id == 2) id = 4;
                else if (id == 4) id = 1;
            }
           
            return id;
        }

        private void HandleVictory(GameResult gameResult)
        {
            // Update LeaderBoard

            Player mainPlayer = playersData.GetMainPlayer();
            if (mainPlayer.myTeam == gameResult.winningTeam)
            {
                if (!Application.isEditor)
                {
                    leaderBoard.WriteToLeaderBoard(1, "MultiPlayer");
                    leaderBoard.WriteToLeaderBoard(1, "TotalGames");
                    achievementsData.MultiPlayerGameWOn();
                }
                AudioManager.instance.PlayWinningSound();
              
                winPs1.Play();
                winPs2.Play();
            }
            else
            {
                AudioManager.instance.PlayLosingSound();
            }


            EndGame(gameResult);
        }


        public void EndGame(GameResult result)
        {
            inputData.DeactivateInput();
            coinData.DestroyAllCoins();
            foreach(Player p in playersData.CurrentPlayers)
            {
                p.myStriker.SetActive(false);
            }
            uiOutputData.GameCompleted(result);
            connectionDisconnection.EndGame();
        }

        private void OnExitGame()   
        {           
            inputData.DeactivateInput();
            coinData.DestroyAllCoins();
            networkInputData.LeaveRoom();
            uiOutputData.SetIsPlaying(false);
        }

    }
}
        
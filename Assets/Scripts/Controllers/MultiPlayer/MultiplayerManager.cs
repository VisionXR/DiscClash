using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
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
        public UIInputDataSO uiInputData;
        public InputDataSO inputData;
        public LeaderBoardSO leaderBoardData;
        public UIDataSO uiData;
        public ADDataSO adData;


        [Header("Scripts")]
        public MobileInputManager mobileInputManager;
        public InputCanvasView inputCanvasView;
        public BlackAndWhiteLogic blackAndWhiteLogic;
        public FreeStyleLogic freeStyleLogic;
        public FineLogic fineLogic;
        public DataManager dataManager;
        public MultiPlayerConnectionDisconnection connectionDisconnection;
        public ScoreManager scoreManager;
        private bool isFirstTurn = false;

        [Header("Local")]
        public ParticleSystem winPs1;
        public ParticleSystem winPs2;
        private DateTime matchStartTime;
        private Coroutine turnTimeRoutine;

        private void OnEnable()
        {
            gameData.SetFirstTurnId(-1);

            uiInputData.ExitGameEvent += OnExitGame;
            uiInputData.HomeEvent += OnExitGame;
           
            playersData.PlayerStrikeStartedEvent += StrikeStarted;
            playersData.PlayerStrikeFinishedEvent += StrikeFinished;

            fineLogic.PutFineEvent += PutFine;

            networkInputData.StartGameEvent += StartGame;
            uiInputData.PlayAgainEvent += PlayAgain;

            networkInputData.CurrentGameDataReceivedEvent += GameDataReceived;
            networkInputData.DestroyCoinsFellInThisTurnEvent += DestroyCoinsFellInThisTurn;
            networkInputData.GameResultReceivedEvent += GameResultReceived;
            networkInputData.PutFineEvent += ReceiveFine;
            networkInputData.StartNewBoardEvent += StartNextBoard;

            gameData.TurnChangedEvent += TurnChanged;


            uiInputData.PauseGameEvent += PauseGame;
            uiInputData.ResumeGameEvent += ResumeGame;

        }

        private void OnDisable()
        {

            uiInputData.ExitGameEvent -= OnExitGame;
            uiInputData.HomeEvent -= OnExitGame;         

            playersData.PlayerStrikeStartedEvent -= StrikeStarted;
            playersData.PlayerStrikeFinishedEvent -= StrikeFinished;

            fineLogic.PutFineEvent -= PutFine;

            networkInputData.StartGameEvent -= StartGame;
            uiInputData.PlayAgainEvent -= PlayAgain;

            networkInputData.CurrentGameDataReceivedEvent -= GameDataReceived;
            networkInputData.GameResultReceivedEvent -= GameResultReceived;
            networkInputData.DestroyCoinsFellInThisTurnEvent -= DestroyCoinsFellInThisTurn;
            networkInputData.PutFineEvent -= ReceiveFine;
            networkInputData.StartNewBoardEvent -= StartNextBoard;

            gameData.TurnChangedEvent -= TurnChanged;
   
            uiInputData.PauseGameEvent -= PauseGame;
            uiInputData.ResumeGameEvent -= ResumeGame;
        }
        private void ResumeGame()
        {
            if (inputData.isInputEnabled)
            {
                inputCanvasView.gameObject.SetActive(true);
                inputCanvasView.TurnOn();
            }
        }

        private void PauseGame()
        {
            if (inputData.isInputEnabled)
            {
                inputCanvasView.TurnOff();
            }
        }
        public void PlayAgain()
        {
            dataManager.PlayAgain();
        }

        private void TurnChanged(int id)
        {

        }

        //// Add this coroutine to fix CS0103: The name 'TurnTimeRoutine' does not exist in the current context
        //private IEnumerator TurnTimeRoutine(int id)
        //{
        //    // Example: Wait for a fixed turn time (e.g., 30 seconds)
        //    float turnTime = 45f;
        //    float elapsed = 0f;
        //    while (elapsed < turnTime)
        //    {
        //        elapsed += Time.deltaTime;
        //        yield return null;
        //    }
          
        //    turnTimeRoutine = null;

        //    if(networkOutputData.IsHost())
        //    {
        //        GameResult result = new GameResult();
        //        result.isVictory = false;
        //        result.currentTurnId = NextTurn();
        //        dataManager.SendGameResult(result);
        //    }
        //}


        public void StartGame(int turnId,int coinsId)
        {
            uiData.uiManager.ChangeState("MultiPlayerStartGame", true);
          
            uiOutputData.SetMyCoinsId(coinsId);

            gameData.ResetData();
            coinData.ResetData();
            coinData.ResetCount();
            strikerData.ResetFoul();

            gameData.SetFirstTurnId(turnId);

            coinData.CreateAllCoins(uiOutputData.MyCoinsId);

            connectionDisconnection.StartGame();

            uiInputData.StartGame();

            if (uiOutputData.challenge == Challenge.BWTournament || uiOutputData.challenge == Challenge.FSTournament)
            {
                Debug.Log(" In multiplayer tournament start ");

                gameData.tournamentData.ResetTournamentData();

            }

            StartCoroutine(WaitAndStart(turnId));

            matchStartTime = DateTime.Now;

            FireBaseAnalyticsManager.Instance.LogGameStart(
               Enum.GetName(typeof(GameType), uiOutputData.gameType),
               Enum.GetName(typeof(Challenge), uiOutputData.challenge)

               );

        }

        public void StartNextBoard()
        {
            int id = 1;
            if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
            {
                if (gameData.firstTurnId == 1)
                {
                    id = 2;
                }
            }
            else
            {
                if (gameData.firstTurnId == 1)
                {
                    id = 3;
                }
                else if (gameData.firstTurnId == 3)
                {
                    id = 2;
                }
                else if (gameData.firstTurnId == 2)
                {
                    id = 4;
                }
                else if (gameData.firstTurnId == 4)
                {
                    id = 1;
                }
            }

            gameData.tournamentData.SetBoardNo(); // Increment Board Number

            if (uiOutputData.challenge == Challenge.BWTournament) // Switch coins for players
            {
                foreach (Player p in playersData.CurrentPlayers)
                {
                    if (p.myCoin == PlayerCoin.White)
                    {
                        p.myCoin = PlayerCoin.Black;
                    }
                    else
                    {
                        p.myCoin = PlayerCoin.White;
                    }
                }
            }

            StartCoroutine(WaitAndStart(id));
        }

        private IEnumerator WaitAndStart(int turnId)
        {
            yield return new WaitForSeconds(0.5f);
            gameData.ChangeTurn(turnId);

            Player p = playersData.GetMainPlayer();

            if (p.myId == turnId)
            {

                mobileInputManager.SetFirstTurn(true);
                coinData.ShowCoinRotationCanvas(turnId);
                isFirstTurn = true;

            }
        }
    
        private void StrikeStarted(int id, float f)
        {
          
            inputData.DisableInput();

            if (isFirstTurn)
            {
                mobileInputManager.SetFirstTurn(false);
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
            scoreManager.UpdateScore();
        }

        private void GameResultReceived(GameResult result)
        {
            coinData.ResetData();
            strikerData.ResetFoul();

            if(result.isVictory)
            {
                if (uiOutputData.challenge == Challenge.BWTournament || uiOutputData.challenge == Challenge.FSTournament)
                {
                    StartCoroutine(HandleTournamentVictory(result));
                }
                else
                {
                    StartCoroutine(HandleVictory(result));
                }

            }

            else
            {
                gameData.ChangeTurn(result.currentTurnId);
            }
        }
        private void ReceiveFine(PlayerCoin coin)
        {
            coinData.CreateCoin(coin,uiOutputData.MyCoinsId);
        }

        public void ProcessPlayerData(Player p, int Whites, int Blacks, int Red, bool isFoul)
        {
            IncrementScore(p, Whites, Blacks, Red, isFoul);
           
            UpdateGameData(Whites, Blacks, Red);

            bool ShouldIContinueTurn = DeterminePlayerTurn(p, Whites, Blacks, Red, isFoul);

            fineLogic.CheckFine(p, Whites, Blacks, Red, isFoul);

            GameResult gameResult = CheckGameResult(p);

            scoreManager.UpdateScore();
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
            if (uiOutputData.challenge == Challenge.BlackAndWhite || uiOutputData.challenge == Challenge.BWTournament)
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
            if (uiOutputData.challenge == Challenge.BlackAndWhite || uiOutputData.challenge == Challenge.BWTournament)
            {
                return blackAndWhiteLogic.CheckWinningCondition(p);
            }
            else if (uiOutputData.challenge == Challenge.FreeStyle || uiOutputData.challenge == Challenge.FSTournament)
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

        private IEnumerator HandleVictory(GameResult gameResult)
        {
            yield return new WaitForSeconds(0.5f);

            float matchDuration = (float)(DateTime.Now - matchStartTime).TotalSeconds;
            FireBaseAnalyticsManager.Instance.LogGameComplete(
              Enum.GetName(typeof(GameType), uiOutputData.gameType),
                Enum.GetName(typeof(Challenge), uiOutputData.challenge),
              matchDuration
              );

            // Update LeaderBoard
            uiData.uiManager.ChangeState("MultiPlayerStartGame", false);
            uiInputData.GameCompleted(gameResult);

            Player mainPlayer = playersData.GetMainPlayer();
            if (mainPlayer.myTeam == gameResult.winningTeam)
            {     
                AudioManager.instance.PlayWinningSound();            
                winPs1.Play();
                winPs2.Play();
                uiInputData.GameWon();
                if (uiOutputData.challenge == Challenge.BlackAndWhite)
                {
                    leaderBoardData.WriteToLeaderBoard(gameData.GetBWMatchPoints(mainPlayer), "MultiPlayer");

                }
                else if (uiOutputData.challenge == Challenge.FreeStyle)
                {
                    leaderBoardData.WriteToLeaderBoard(gameData.GetFSMatchPoints(mainPlayer), "MultiPlayer");
                }
            }
            else
            {
                AudioManager.instance.PlayLosingSound();
        
            }
            EndGame();
            adData.ShowInterstitialAd();
        }

        private IEnumerator HandleTournamentVictory(GameResult gameResult)
        {
            yield return new WaitForSeconds(1);

            Player mainPlayer = playersData.GetMainPlayer();

            Player p = playersData.GetPlayer(gameResult.winningPlayerId);
            int mpPoints = 0;

            if (uiOutputData.challenge == Challenge.BWTournament)
            {
                mpPoints = gameData.GetBWMatchPoints(p);
            }
            else if (uiOutputData.challenge == Challenge.FSTournament)
            {
                mpPoints = gameData.GetFSMatchPoints(p);
            }


            gameData.GetBWMatchPoints(p);

            if (p.myId == 1)
            {
                gameData.tournamentData.SetScores(mpPoints.ToString(), "0");
                gameData.tournamentData.CalculateScores();
            }
            else
            {
                gameData.tournamentData.SetScores("0", mpPoints.ToString());
                gameData.tournamentData.CalculateScores();
            }

            if (mainPlayer.myTeam == gameResult.winningTeam)
            {
                AudioManager.instance.PlayWinningSound();

                winPs1.Play();
                winPs2.Play();
            }
            else
            {
                Debug.Log("Tournament Board Lost!");
                AudioManager.instance.PlayLosingSound();
            }

            if (gameData.tournamentData.currentBoardNo == 7 || gameData.tournamentData.P1TotalScore >= 25 || gameData.tournamentData.P2TotalScore >= 25)
            {
                if (gameData.tournamentData.currentBoardNo == 7)
                {
                    if (gameData.tournamentData.P1TotalScore >= gameData.tournamentData.P2TotalScore)
                    {
                        gameResult.winningPlayerId = 1;
                        Player wp = playersData.GetPlayer(1);
                        gameResult.winningTeam = wp.myTeam;
                    }
                    else
                    {
                        gameResult.winningPlayerId = 2;
                        Player wp = playersData.GetPlayer(2);
                        gameResult.winningTeam = wp.myTeam;
                    }
                }
                else
                {
                    if (gameData.tournamentData.P1TotalScore >= 25)
                    {
                        gameResult.winningPlayerId = 1;
                        Player wp = playersData.GetPlayer(1);
                        gameResult.winningTeam = wp.myTeam;
                    }

                    else if (gameData.tournamentData.P2TotalScore >= 25)
                    {

                        gameResult.winningPlayerId = 2;
                        Player wp = playersData.GetPlayer(2);
                        gameResult.winningTeam = wp.myTeam;

                    }
                }

                uiInputData.GameCompleted(gameResult);
            }
            else
            {
                uiInputData.TournamentBoardCompleted(gameResult);
            }

            EndGame();
            adData.ShowInterstitialAd();
        }


        public void EndGame()
        {
            inputCanvasView.gameObject.SetActive(false);
            inputData.DisableInput();
            coinData.DestroyAllCoins();
            foreach(Player p in playersData.CurrentPlayers)
            {
                p.myStriker.SetActive(false);
            }
         
            connectionDisconnection.EndGame();
        }

      

        private void OnExitGame()   
        {
            float matchDuration = (float)(DateTime.Now - matchStartTime).TotalSeconds;
            FireBaseAnalyticsManager.Instance.LogGameExit(
             Enum.GetName(typeof(GameType), uiOutputData.gameType),
             Enum.GetName(typeof(Challenge), uiOutputData.challenge),
             matchDuration
             );

            mobileInputManager.SetFirstTurn(false);
            inputCanvasView.gameObject.SetActive(false);
            inputData.DisableInput();
            coinData.DestroyAllCoins();
            networkInputData.LeaveRoom();
            
        }

    }
}
        
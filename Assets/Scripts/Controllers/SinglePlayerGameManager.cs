using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
using System;
using System.Collections;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class SinglePlayerGameManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public PlayersDataSO playersData;
        public CoinDataSO coinData;
        public StrikerDataSO strikerData;
        public GameDataSO gameData;
        public InputDataSO inputData;
        public LeaderBoardSO leaderBoardData;
        public ADDataSO adData;


        [Header("Scripts")]
        public MobileInputManager mobileInputManager;
        public InputCanvasView inputCanvasView;
        public ScoreManager scoreManager;
        public BlackAndWhiteLogic blackAndWhiteLogic;
        public FreeStyleLogic freeStyleLogic;
        public FineLogic fineLogic;
       

        [Header("Local")]
        public ParticleSystem winPs1;
        public ParticleSystem winPs2;

        // local variables
        // T10/T07: Accept one result and one next-board operation for each tournament board.
        private bool awaitingNextBoard;
        private bool tournamentResultPending;
        private DateTime matchStartTime;
        private bool isFirstTurn = false;

        private void OnEnable()
        {
            
            uiInputData.HomeEvent += ExitGame;
            uiInputData.ExitGameEvent += ExitGame;
            uiInputData.PlayAgainEvent += StartGame;

            uiInputData.NextTournamentBoardEvent += StartNextBoard;

            playersData.PlayerStrikeStartedEvent += StrikeStarted;
            playersData.PlayerStrikeFinishedEvent += StrikeFinished;

            fineLogic.PutFineEvent += PutFine;

            uiInputData.PauseGameEvent += PauseGame;
            uiInputData.ResumeGameEvent += ResumeGame;

            playersData.CreateSinglePlayers();
        }

        private void OnDisable()
        {
            // T10: No delayed board startup/result may survive leaving the game.
            StopAllCoroutines();
            awaitingNextBoard = false;
            tournamentResultPending = false;
          
            uiInputData.HomeEvent -= ExitGame;
            uiInputData.ExitGameEvent -= ExitGame;

            uiInputData.PlayAgainEvent -= StartGame;

            uiInputData.NextTournamentBoardEvent -= StartNextBoard;

            playersData.PlayerStrikeStartedEvent -= StrikeStarted;
            playersData.PlayerStrikeFinishedEvent -= StrikeFinished;

            fineLogic.PutFineEvent -= PutFine;

            uiInputData.PauseGameEvent -= PauseGame;
            uiInputData.ResumeGameEvent -= ResumeGame;

        }

        private void ResumeGame()
        {
            if(inputData.isInputEnabled)
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

        public void StartGame()
        {
            // T10: A fresh game starts a fresh board lifecycle.
            awaitingNextBoard = false;
            tournamentResultPending = false;
            int id = 1;
            if(uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
            {
                if(gameData.firstTurnId == 1)
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

            if (uiOutputData.challenge == Challenge.BWTournament || uiOutputData.challenge == Challenge.FSTournament)
            {
                Debug.Log(" In tournament start ");

                gameData.tournamentData.ResetTournamentData();
               
            }

            StartCoroutine(WaitAndStart(id));
        }

        public void StartNextBoard()
        {
            // T10: Ignore repeated clicks/RPCs before creating coins or changing player colours.
            if (!awaitingNextBoard || !gameData.tournamentData.SetBoardNo()) return;
            awaitingNextBoard = false;
            tournamentResultPending = false;
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

        private IEnumerator WaitAndStart(int id)
        {
            yield return new WaitForSeconds(0.5f);
            coinData.ResetData();
            coinData.CreateAllCoins(uiOutputData.MyCoinsId);
            gameData.SetFirstTurnId(id);
            // T08: Keep board setup events, but count only the first board as a new game.
            uiInputData.StartGame(gameData.tournamentData.currentBoardNo == 0
                || (uiOutputData.challenge != Challenge.BWTournament && uiOutputData.challenge != Challenge.FSTournament));
            int firstTurn = id;
            if (firstTurn == 1)
            {
                mobileInputManager.SetFirstTurn(true);
                coinData.ShowCoinRotationCanvas(firstTurn);
                isFirstTurn = true;
            }

            matchStartTime = DateTime.Now;

            FireBaseAnalyticsManager.Instance.LogGameStart(
                Enum.GetName(typeof(GameType), uiOutputData.gameType),
                Enum.GetName(typeof(Challenge), uiOutputData.challenge)
                );


            StartCoroutine(WaitForSeconds(0.1f, firstTurn));
        }

        private void StrikeStarted(int id, float f)
        {
         
            inputData.DisableInput();           
            if(isFirstTurn)
            {
                mobileInputManager.SetFirstTurn(false);
                isFirstTurn = false;
            }     
        }

        private void StrikeFinished(int id)
        {
        
            ProcessPlayerData(playersData.GetPlayer(gameData.currentTurnId), coinData.Whites, coinData.Blacks, coinData.Red, strikerData.isFoul);
            coinData.DestroyCoinsFellInthisTurn(coinData.GetCoinsFellInThisTurn());
            coinData.ResetData();
            strikerData.ResetFoul();

        }

        private void PutFine(PlayerCoin coin)
        {
            coinData.CreateCoin(coin,uiOutputData.MyCoinsId);
        }

        public void ProcessPlayerData(Player p, int Whites, int Blacks, int Red, bool isFoul)
        {
            
            IncrementScore(p, Whites, Blacks, Red, isFoul);

            bool ShouldIContinueTurn = DeterminePlayerTurn(p, Whites, Blacks, Red, isFoul);

            UpdateGameData(Whites, Blacks, Red);

            fineLogic.CheckFine(p, Whites, Blacks, Red, isFoul);

            scoreManager.UpdateScore();

            GameResult gameResult = CheckGameResult(p);

            if (gameResult.isVictory)
            {
                if(uiOutputData.challenge == Challenge.BWTournament || uiOutputData.challenge == Challenge.FSTournament)
                {
                    StartCoroutine(HandleTournamentVictory(gameResult));
                }
                else
                {
                    StartCoroutine(HandleVictory(gameResult));
                }
            }
            else
            {
                HandleTurnChange(ShouldIContinueTurn);
            }
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
        private void HandleTurnChange(bool ShouldIContinueTurn)
        {
            StartCoroutine(WaitAndChangeTurn(ShouldIContinueTurn));
        }
        private IEnumerator WaitAndChangeTurn(bool ShouldIContinueTurn)
        {
            yield return new WaitForSeconds(0.1f);

            int id = 1;

            if (ShouldIContinueTurn)
            {
                id = gameData.currentTurnId;
               
            }
            else
            {       
                id = NextTurn();
               
            }
            
            gameData.ChangeTurn(id);
            Player p = playersData.GetMainPlayer() ;
     

        }
        public int NextTurn()
        {
            int id = gameData.currentTurnId;

            if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI) // Two-player mode
            {
                if (id == 1)
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

            yield return new WaitForSeconds(1);
            float matchDuration = (float)(DateTime.Now - matchStartTime).TotalSeconds;
            FireBaseAnalyticsManager.Instance.LogGameComplete(
              Enum.GetName(typeof(GameType), uiOutputData.gameType),
                Enum.GetName(typeof(Challenge), uiOutputData.challenge),
              matchDuration
              );

            Player mainPlayer = playersData.GetMainPlayer();
            if (mainPlayer.myTeam == gameResult.winningTeam)
            {
              
                AudioManager.instance.PlayWinningSound();
               
                winPs1.Play();
                winPs2.Play();
                uiInputData.GameWon();

                if (uiOutputData.challenge == Challenge.BlackAndWhite)
                {
                    leaderBoardData.WriteToLeaderBoard(gameData.GetBWMatchPoints(mainPlayer), "SinglePlayer");

                }
                else if(uiOutputData.challenge == Challenge.FreeStyle)
                {
                    leaderBoardData.WriteToLeaderBoard(gameData.GetFSMatchPoints(mainPlayer), "SinglePlayer");
                }
            }
            else
            {
                AudioManager.instance.PlayLosingSound();              
            }

            uiInputData.GameCompleted(gameResult);
            EndGame();
            adData.ShowInterstitialAd();
        }

        private IEnumerator HandleTournamentVictory(GameResult gameResult)
        {
            // T10/T07: Duplicate completion messages must not score or reward the board twice.
            if (tournamentResultPending) yield break;
            tournamentResultPending = true;
            yield return new WaitForSeconds(1);

            Player mainPlayer = playersData.GetMainPlayer();
            Player winner = playersData.GetPlayer(gameResult.winningPlayerId);
            int points = uiOutputData.challenge == Challenge.BWTournament
                ? gameData.GetBWMatchPoints(winner) : gameData.GetFSMatchPoints(winner);

            // T01: Attribute board points to the winning side, including Team A's Player 2.
            bool firstSide = gameData.IsFirstTournamentSide(winner);
            gameData.tournamentData.SetScores(firstSide ? points.ToString() : "0", firstSide ? "0" : points.ToString());
            gameData.tournamentData.CalculateScores();
            bool finished = gameData.tournamentData.currentBoardNo == 7
                || gameData.tournamentData.P1TotalScore >= 25 || gameData.tournamentData.P2TotalScore >= 25;

            // T02/T09: Resolve the overall winner before choosing final sound, particles and rewards.
            if (finished)
            {
                gameResult.winningPlayerId = gameData.GetTournamentWinnerId();
                gameResult.winningTeam = playersData.GetPlayer(gameResult.winningPlayerId).myTeam;
            }
            bool localWinner = mainPlayer.myTeam == gameResult.winningTeam;
            if (localWinner)
            {
                AudioManager.instance.PlayWinningSound();
                winPs1.Play();
                winPs2.Play();
            }
            else AudioManager.instance.PlayLosingSound();

            if (finished)
            {
                // T07: Count one tournament win and submit the winning side's total once.
                if (localWinner)
                {
                    uiInputData.GameWon();
                    int total = gameResult.winningTeam == Team.TeamA
                        ? gameData.tournamentData.P1TotalScore : gameData.tournamentData.P2TotalScore;
                    leaderBoardData.WriteToLeaderBoard(total, "SinglePlayer");
                }
                uiInputData.GameCompleted(gameResult);
            }
            else
            {
                // T10: Only a completed, non-final board permits one Next operation.
                awaitingNextBoard = true;
                uiInputData.TournamentBoardCompleted(gameResult);
            }
            EndGame();
            adData.ShowInterstitialAd();
        }

        public void EndGame()
        {
            coinData.DestroyAllCoins();
            inputCanvasView.gameObject.SetActive(false);
            inputData.DisableInput();
            gameData.ResetData();
            foreach (Player p in playersData.CurrentPlayers)
            {
                p.myStriker.SetActive(false);
            }
        }
        private void ExitGame()
        {

            float matchDuration = (float)(DateTime.Now - matchStartTime).TotalSeconds;
            FireBaseAnalyticsManager.Instance.LogGameExit(
             Enum.GetName(typeof(GameType), uiOutputData.gameType),
             Enum.GetName(typeof(Challenge), uiOutputData.challenge),
             matchDuration
             );


            inputCanvasView.gameObject.SetActive(false);
            inputData.DisableInput();
            coinData.DestroyAllCoins();
            playersData.DestroyAllPlayers();
            mobileInputManager.SetFirstTurn(false);
            gameObject.SetActive(false);
            

        }


        private IEnumerator WaitForSeconds(float v, int turnid)
        {
            yield return new WaitForSeconds(v);
            strikerData.ResetFoul();
            gameData.ChangeTurn(turnid);
        }

    }
}
    
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
        private bool isFirstTurn = false;

        private void OnEnable()
        {
            
            uiInputData.HomeEvent += ExitGame;
            uiInputData.ExitGameEvent += ExitGame;
            uiInputData.PlayAgainEvent += StartGame;

            playersData.PlayerStrikeStartedEvent += StrikeStarted;
            playersData.PlayerStrikeFinishedEvent += StrikeFinished;

            fineLogic.PutFineEvent += PutFine;

            uiInputData.PauseGameEvent += PauseGame;
            uiInputData.ResumeGameEvent += ResumeGame;

            playersData.CreateSinglePlayers();
        }

        private void OnDisable()
        {
          
            uiInputData.HomeEvent -= ExitGame;
            uiInputData.ExitGameEvent -= ExitGame;

            uiInputData.PlayAgainEvent -= StartGame;

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

        public void StartGame(int id)
        {
            StartCoroutine(WaitAndStart(id));
        }

        private IEnumerator WaitAndStart(int id)
        {
            yield return new WaitForSeconds(1);
            coinData.ResetData();
            coinData.CreateAllCoins(uiOutputData.MyCoinsId);
            gameData.SetFirstTurnId(id);
            uiInputData.StartGame();
            int firstTurn = id;
            if (firstTurn == 1)
            {
                mobileInputManager.SetFirstTurn(true);
                coinData.ShowCoinRotationCanvas(firstTurn);
                isFirstTurn = true;
            }
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
          
            GameResult gameResult = CheckGameResult(p);

            scoreManager.UpdateScore();


            if (gameResult.isVictory)
            {
                HandleVictory(gameResult);
            }
            else
            {
                HandleTurnChange(ShouldIContinueTurn);
            }
        }
        private bool DeterminePlayerTurn(Player p, int Whites, int Blacks, int Red, bool isFoul)
        {
            if (uiOutputData.challenge == Challenge.BlackAndWhite)
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
            if (uiOutputData.challenge == Challenge.BlackAndWhite)
            {
                return blackAndWhiteLogic.CheckWinningCondition(p);
            }
            else if (uiOutputData.challenge == Challenge.FreeStyle)
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

        private void HandleVictory(GameResult gameResult)
        {
          

            Player mainPlayer = playersData.GetMainPlayer();
            if (mainPlayer.myTeam == gameResult.winningTeam)
            {
              
                AudioManager.instance.PlayWinningSound();
               
                winPs1.Play();
                winPs2.Play();

                uiInputData.GameWon();
                CalculatePoints();              
            }
            else
            {
                AudioManager.instance.PlayLosingSound();              
            }

            uiInputData.GameCompleted(gameResult);
            EndGame();
            adData.ShowInterstitialAd();
        }

        private void CalculatePoints()
        {
            Player mainPlayer = playersData.GetMainPlayer();
            int leaderboardPoints = 0;
            if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
            {
               
               if( mainPlayer.myId == 1)
                {
                    if (gameData.P1Score > gameData.P2Score)
                    {
                        leaderboardPoints = gameData.P1Score-gameData.P2Score;
                    }
                    else
                    {
                        leaderboardPoints = 1;
                    }
                }
                else
                {
                    if (gameData.P2Score > gameData.P1Score)
                    {
                        leaderboardPoints = gameData.P2Score - gameData.P1Score;
                    }
                    else
                    {
                        leaderboardPoints = 1;
                    }
                }
            }
            else
            {
                if (mainPlayer.myTeam == Team.TeamA)
                {
                    if (gameData.TeamAScore > gameData.TeamBScore)
                    {
                        leaderboardPoints = gameData.TeamAScore - gameData.TeamBScore;
                    }
                    else
                    {
                        leaderboardPoints = 1;
                    }
                }
                else
                {
                    if (gameData.TeamBScore > gameData.TeamAScore)
                    {
                        leaderboardPoints = gameData.TeamBScore - gameData.TeamAScore;
                    }
                    else
                    {
                        leaderboardPoints = 1;
                    }
                }
            }

            Debug.Log("Points Earned: " + leaderboardPoints);
            leaderBoardData.WriteToLeaderBoard(leaderboardPoints, "SinglePlayer");
            // Here you can add code to update the player's points in a leaderboard or player profile
        }

        public void EndGame()
        {
            coinData.DestroyAllCoins();
            inputCanvasView.gameObject.SetActive(false);
            inputData.DisableInput();

            foreach (Player p in playersData.CurrentPlayers)
            {
                p.myStriker.SetActive(false);
            }
        }
        private void ExitGame()
        {
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
    
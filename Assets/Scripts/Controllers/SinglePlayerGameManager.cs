using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class SinglePlayerGameManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public PlayersDataSO playersData;
        public CoinDataSO coinData;
        public StrikerDataSO strikerData;
        public GameDataSO gameData;
        public InputDataSO inputData;
        public LeaderBoardSO leaderBoard;
        public AchievementsDataSO achievementsData;


        [Header("Scripts")]
        public GameObject inputPanel2Players;
        public GameObject inputPanel4Players;
        public BlackAndWhiteLogic blackAndWhiteLogic;
        public FreeStyleLogic freeStyleLogic;
        public FineLogic fineLogic;
        private bool isFirstTurn = false;

        [Header("Local")]
        public ParticleSystem winPs1;
        public ParticleSystem winPs2;

        private void OnEnable()
        {

            uiOutputData.SetIsPlaying(true);

            uiOutputData.HomeEvent += ExitGame;
            uiOutputData.ExitGameEvent += ExitGame;
            uiOutputData.PlayAgainEvent += PlayAgain;

            playersData.PlayerStrikeStartedEvent += StrikeStarted;
            playersData.PlayerStrikeFinishedEvent += StrikeFinished ;

            fineLogic.PutFineEvent += PutFine;

            gameData.TurnChangedEvent += TurnChanged;


            StartCoroutine(StartGame());
        }

        private void OnDisable()
        {

            uiOutputData.HomeEvent -= ExitGame;
            uiOutputData.ExitGameEvent -= ExitGame;
            uiOutputData.PlayAgainEvent -= PlayAgain;

            playersData.PlayerStrikeStartedEvent -= StrikeStarted;
            playersData.PlayerStrikeFinishedEvent -= StrikeFinished;

            fineLogic.PutFineEvent -= PutFine;

            gameData.TurnChangedEvent -= TurnChanged;


        }

        private void TurnChanged(int id)
        {
            Player p = playersData.GetPlayer(id);
            if (p.myPlayerRole == PlayerRole.Human && p.myPlayerControl == PlayerControl.Local)
            {
                inputPanel2Players.SetActive(true);
                inputPanel4Players.SetActive(true);
            }
        }

        private void PlayAgain()
        {
            coinData.ResetData();
            strikerData.ResetFoul();

            coinData.CreateAllCoins();


            int firstTurn = 1;

            if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
            {
                if (uiOutputData.playerCoin == PlayerCoin.White)
                {
                    firstTurn = 1;

                }
                else
                {
                    firstTurn = 2;
                }
            }
            else if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PAIvsAI)
            {
                if (uiOutputData.playerCoin == PlayerCoin.White)
                {
                    firstTurn = 1;
                }
                else
                {
                    firstTurn = 3;
                }

            }

            gameData.ChangeTurn(firstTurn);


            if (firstTurn == 1)
            {
                Player p = playersData.GetMainPlayer();
                p.GetComponent<PlayerInput>().StartRotation();
                isFirstTurn = true;

            }

        }
        private IEnumerator StartGame()
        {

            coinData.ResetData();
           
            playersData.CreateSinglePlayers();
            yield return new WaitForSeconds(0.1f);
            coinData.CreateAllCoins();
           
            int firstTurn = 1;

            if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
            {
                if (uiOutputData.playerCoin == PlayerCoin.White)
                {
                    firstTurn = 1;

                }
                else
                {
                    firstTurn = 2;
                }
            }
            else if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PAIvsAI)
            {
                if (uiOutputData.playerCoin == PlayerCoin.White)
                {
                    firstTurn = 1;
                }
                else
                {
                    firstTurn = 3;
                }

            }

         
            if (firstTurn == 1)
            {
                Player p = playersData.GetMainPlayer();
                p.GetComponent<PlayerInput>().StartRotation();
                coinData.ShowRotationCanvasEvent?.Invoke();
                isFirstTurn = true;
               
            }

            StartCoroutine(WaitForSeconds(0.1f, firstTurn));
        }

        private IEnumerator WaitForSeconds(float v,int turnid)
        {
            yield return new WaitForSeconds(v);
            strikerData.ResetFoul();
            gameData.ChangeTurn(turnid);
        }

        private void StrikeStarted(int id, float f)
        {
            inputPanel2Players.SetActive(false);
            inputPanel4Players.SetActive(false);
            inputData.DeactivateInput();           
            if(isFirstTurn)
            {
                Player p = playersData.GetMainPlayer();
                p.GetComponent<PlayerInput>().StopRotation();
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
            coinData.CreateCoin(coin);
        }

        public void ProcessPlayerData(Player p, int Whites, int Blacks, int Red, bool isFoul)
        {
            

            IncrementScore(p, Whites, Blacks, Red, isFoul);

            bool ShouldIContinueTurn = DeterminePlayerTurn(p, Whites, Blacks, Red, isFoul);

            UpdateGameData(Whites, Blacks, Red);

            fineLogic.CheckFine(p, Whites, Blacks, Red, isFoul);
          
            GameResult gameResult = CheckGameResult(p);


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

            if (uiOutputData.singlePlayerGameMode== SinglePlayerGameMode.PvsAI) // Two-player mode
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
                leaderBoard.WriteToLeaderBoard(1, "SinglePlayer");
                leaderBoard.WriteToLeaderBoard(1, "TotalGames");
                AudioManager.instance.PlayWinningSound();
                achievementsData.SinglePlayerWon();
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
            coinData.DestroyAllCoins();

            inputData.DeactivateInput();

            uiOutputData.GameCompleted(result);


            foreach (Player p in playersData.CurrentPlayers)
            {
                p.myStriker.SetActive(false);
            }

        

        }

        private void ExitGame()
        {
            inputData.DeactivateInput();
            coinData.DestroyAllCoins();
            playersData.DestroyAllPlayers();
            uiOutputData.SetIsPlaying(false);
            gameObject.SetActive(false);
            

        }
    
    }
}
    
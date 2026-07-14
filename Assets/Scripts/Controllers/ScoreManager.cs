using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
using System;
using UnityEngine;


namespace com.VisionXR.Controllers
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public PlayersDataSO playersData;
        public GameDataSO gameData;


        [Header("Input Panels")]
        public InputCanvasView inputCanvasView;



        [Header("Score Panels")]
        public ScorePanel2Player ScorePanel2Players;
        public ScorePanel4Player ScorePanel4Players;

        private void OnEnable()
        {
            uiInputData.StartGameEvent += StartGame;
            gameData.TurnChangedEvent += TurnChanged;
            playersData.PlayerStrikeStartedEvent += StrikeStarted;
        }

        private void OnDisable()
        {
            uiInputData.StartGameEvent -= StartGame;
            gameData.TurnChangedEvent -= TurnChanged;
            playersData.PlayerStrikeStartedEvent -= StrikeStarted;
        }



        private void StartGame()
        {
            if(uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI || uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
            {
                ScorePanel2Players.gameObject.SetActive(true);
                ScorePanel2Players.ShowImages();
            }
            else
            {
                ScorePanel4Players.gameObject.SetActive(true);
                ScorePanel4Players.ShowImages();
            }
        }
        private void StrikeStarted(int arg1, float arg2)
        {
            inputCanvasView.TurnOff();
        }

        private void TurnChanged(int id)
        {
            Player p = playersData.GetPlayer(id);
            if (p.myPlayerRole == PlayerRole.Human && p.myPlayerControl == PlayerControl.Local)
            {
               inputCanvasView.gameObject.SetActive(true);
                inputCanvasView.TurnOn();
            }
        }

        public void UpdateScore()
        {
            if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI || uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
            {
                if(uiOutputData.challenge == Challenge.BlackAndWhite)
                {
                    ShowBlackAndWhite2PlayerScore();
                }
                else
                {
                    ShowFreeStyle2PlayerScore();
                }
            }
            else
            {
                if (uiOutputData.challenge == Challenge.BlackAndWhite)
                {
                    ShowBlackAndWhite4PlayerScore();
                }
                else
                {
                    ShowFreeStyle4PlayerScore();
                }
            }
        }

        private void ShowBlackAndWhite2PlayerScore()
        {

            Player player1 = playersData.GetPlayer(1);
            if (player1 != null)
            {
                if (player1.myCoin == PlayerCoin.White)
                {
                    gameData.P1Score = gameData.P1Whites + gameData.P2Whites + gameData.P1Red * 3;
                    ScorePanel2Players.leftPlayer.SetScore(gameData.P1Score);

                }
                else
                {

                    gameData.P1Score = gameData.P1Blacks + gameData.P2Blacks + gameData.P1Red * 3;
                    ScorePanel2Players.leftPlayer.SetScore(gameData.P1Score);
                }
            }


            Player player2 = playersData.GetPlayer(2);

            if (player2 != null)
            {
                if (player2.myCoin == PlayerCoin.White)
                {

                    gameData.P2Score = gameData.P1Whites + gameData.P2Whites + gameData.P2Red * 3;
                    ScorePanel2Players.rightPlayer.SetScore(gameData.P2Score);
                }
                else
                {

                    gameData.P2Score = gameData.P1Blacks + gameData.P2Blacks + gameData.P2Red * 3;
                    ScorePanel2Players.rightPlayer.SetScore(gameData.P2Score);
                }
            }


        }
        private void ShowFreeStyle2PlayerScore()
        {
            Player player1 = playersData.GetPlayer(1);

            if (player1 != null)
            {

                gameData.P1Score = gameData.P1Whites + gameData.P1Blacks + gameData.P1Red * 3;
                ScorePanel2Players.leftPlayer.SetScore(gameData.P1Score);
            }


            Player player2 = playersData.GetPlayer(2);

            if (player2 != null)
            {

                gameData.P2Score = gameData.P2Whites + gameData.P2Blacks + gameData.P2Red * 3;
                ScorePanel2Players.rightPlayer.SetScore(gameData.P1Score);
            }


        }

        private void ShowBlackAndWhite4PlayerScore()
        {
            Player p1 = playersData.GetPlayer(1);


            Player p2 = playersData.GetPlayer(2);


            Player p3 = playersData.GetPlayer(3);


            Player p4 = playersData.GetPlayer(4);


            if (p1 != null && p2 != null)
            {
              
                gameData.TeamAScore = (gameData.P1Whites + gameData.P2Whites + gameData.P3Whites + gameData.P4Whites + gameData.P1Red * 3 + gameData.P2Red * 3);
                ScorePanel4Players.leftPlayer1.SetScore(gameData.TeamAScore);
                ScorePanel4Players.leftPlayer2.SetScore(gameData.TeamAScore);
            }

            if (p3 != null && p4 != null)
            {
             
                gameData.TeamBScore = (gameData.P1Blacks + gameData.P2Blacks + gameData.P3Blacks + gameData.P4Blacks + gameData.P3Red * 3 + gameData.P4Red * 3);
                ScorePanel4Players.rightPlayer1.SetScore(gameData.TeamBScore);
                ScorePanel4Players.rightPlayer2.SetScore(gameData.TeamBScore);
            }

        }

        private void ShowFreeStyle4PlayerScore()
        {
            Player p1 = playersData.GetPlayer(1);


            Player p2 = playersData.GetPlayer(2);


            Player p3 = playersData.GetPlayer(3);


            Player p4 = playersData.GetPlayer(4);


            if (p1 != null && p2 != null)
            {
             
                gameData.TeamAScore = (gameData.P1Whites + gameData.P2Whites + gameData.P1Blacks + gameData.P2Blacks + gameData.P1Red * 3 + gameData.P2Red * 3);
                ScorePanel4Players.leftPlayer1.SetScore(gameData.TeamAScore);      
                ScorePanel4Players.leftPlayer2.SetScore(gameData.TeamAScore);
            }

            if (p3 != null && p4 != null)
            {
              
                gameData.TeamBScore = (gameData.P3Whites + gameData.P4Whites + gameData.P3Blacks + gameData.P4Blacks + gameData.P3Red * 3 + gameData.P4Red * 3);
                ScorePanel4Players.rightPlayer1.SetScore(gameData.TeamAScore);
                ScorePanel4Players.rightPlayer2.SetScore(gameData.TeamAScore);
            }

        }
    }
}

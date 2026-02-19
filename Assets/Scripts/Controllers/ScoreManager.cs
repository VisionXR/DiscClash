using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
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
        public GameObject inputPanel2Players;
        public GameObject inputPanel4Players;

        [Header("Score Panels")]
        public GameObject ScorePanel2Players;
        public GameObject ScorePanel4Players;

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
            if(uiOutputData.gameMode == GameMode.PvsAI || uiOutputData.gameMode == GameMode.P1vsP2)
            {
                ScorePanel2Players.SetActive(true);
            }
            else
            {
                ScorePanel4Players.SetActive(true);
            }
        }
        private void StrikeStarted(int arg1, float arg2)
        {
            inputPanel2Players.SetActive(false);
            inputPanel4Players.SetActive(false);
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

        public void UpdateScore()
        {

        }

        private void ShowBlackAndWhite2PlayerScore()
        {

            Player player1 = playersData.GetPlayer(1);
            if (player1 != null)
            {
                if (player1.myCoin == PlayerCoin.White)
                {
                    gameData.P1Score = gameData.P1Whites + gameData.P2Whites + gameData.P1Red * 3;

                }
                else
                {

                    gameData.P1Score = gameData.P1Blacks + gameData.P2Blacks + gameData.P1Red * 3;
                }
            }


            Player player2 = playersData.GetPlayer(2);

            if (player2 != null)
            {
                if (player2.myCoin == PlayerCoin.White)
                {

                    gameData.P2Score = gameData.P1Whites + gameData.P2Whites + gameData.P2Red * 3;
                }
                else
                {

                    gameData.P2Score = gameData.P1Blacks + gameData.P2Blacks + gameData.P2Red * 3;
                }
            }


        }
        private void ShowFreeStyle2PlayerScore()
        {
            Player player1 = playersData.GetPlayer(1);

            if (player1 != null)
            {

                gameData.P1Score = gameData.P1Whites + gameData.P1Blacks + gameData.P1Red * 3;
            }


            Player player2 = playersData.GetPlayer(2);

            if (player2 != null)
            {

                gameData.P2Score = gameData.P2Whites + gameData.P2Blacks + gameData.P2Red * 3;
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

            }

            if (p3 != null && p4 != null)
            {
             
                gameData.TeamBScore = (gameData.P1Blacks + gameData.P2Blacks + gameData.P3Blacks + gameData.P4Blacks + gameData.P3Red * 3 + gameData.P4Red * 3);

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
            }

            if (p3 != null && p4 != null)
            {
              
                gameData.TeamBScore = (gameData.P3Whites + gameData.P4Whites + gameData.P3Blacks + gameData.P4Blacks + gameData.P3Red * 3 + gameData.P4Red * 3);

            }

        }
    }
}

using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{

    public class DisplayScoreFor2 : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        [SerializeField] private GameDataSO gameData;
        [SerializeField] private PlayersDataSO playersData;
        [SerializeField] private UIOutputDataSO uiOutputData;    
        

        [Header("Player1 Details")]
        [SerializeField] private TMP_Text Player1Score;
        [SerializeField] private Image player1Timer;
        [SerializeField] private TMP_Text Player1Title;
        [SerializeField] private TMP_Text Player1QueenScore;
        [SerializeField] private TMP_Text Player1TotalScore;
        [SerializeField] private Image player1CoinImage;
        [SerializeField] private Image player1TurnIdicator;

        [Space(5)]
        [Header("Player2 Details")]
        [SerializeField] private TMP_Text Player2Score;
        [SerializeField] private Image player2Timer;
        [SerializeField] private TMP_Text Player2Title;
        [SerializeField] private TMP_Text Player2QueenScore;
        [SerializeField] private TMP_Text Player2TotalScore;
        [SerializeField] private Image player2CoinImage;
        [SerializeField] private Image player2TurnIdicator;

        [Space(5)]
        [Header("Other Details")]
        [SerializeField] private Sprite whiteCoin;
        [SerializeField] private Sprite blackCoin;
        [SerializeField] private Sprite blackwhiteCoin;
        [SerializeField] private Color activeColor;
        [SerializeField] private Color IdleColor;

        private Coroutine TimerRoutine;

        private void OnEnable()
        {
            gameData.TurnChangedEvent += ShowScore;
         
        }

        private void OnDisable()
        {
            gameData.TurnChangedEvent -= ShowScore;
           

            ResetScore();
            if (TimerRoutine != null)
            {
                StopCoroutine(TimerRoutine);
                TimerRoutine = null;
            }
        }

     
        private void ShowScore(int newPlayerID)
        {

            // setting title
            if (uiOutputData.game == Game.BlackAndWhite) 
            {
                Player1Title.text = "Black And White";
                Player2Title.text = "Black And White";
                ShowBlackAndWhiteScore(newPlayerID);
            }
            else
            {
                Player1Title.text = "Free Style";
                Player2Title.text = "Free Style";
                ShowFreeStyleScore(newPlayerID);
            }

            if(TimerRoutine == null)
            {
                TimerRoutine = StartCoroutine(ShowTurnTime(newPlayerID));
            }
            else
            {
                StopCoroutine(TimerRoutine);
                TimerRoutine = StartCoroutine(ShowTurnTime(newPlayerID));
            }

        }

        private IEnumerator ShowTurnTime(int id)
        {
            // Set both timers to 0 initially
            player1Timer.fillAmount = 0;
            player2Timer.fillAmount = 0;

            // The total duration of the turn (in seconds)
            float turnDuration = gameData.TurnTime;
            float elapsedTime = 0f;

            // Reference to the active timer
            Image activeTimer = null;

            // Set the correct timer fill based on the player's turn (id)
            if (id == 1)
            {
                player1Timer.fillAmount = 1; // Start full
                activeTimer = player1Timer;  // Set player 1's timer as active
            }
            else if (id == 2)
            {
                player2Timer.fillAmount = 1; // Start full
                activeTimer = player2Timer;  // Set player 2's timer as active
            }

            activeTimer.fillAmount = 1;

            // Gradually reduce the fillAmount to 0 over 'turnDuration' seconds
            while (elapsedTime < turnDuration)
            {
                elapsedTime += Time.deltaTime; // Time passed since last frame

                // Calculate the new fill amount based on elapsed time
                activeTimer.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / turnDuration);

                // Wait until the next frame
                yield return new WaitForEndOfFrame();
            }

            // Ensure the fill amount is set to 0 at the end of the timer
            activeTimer.fillAmount = 0f;

            
        }

        private void OnPlayerStrikeStarted()
        {
            if (TimerRoutine != null)
            {
                StopCoroutine(TimerRoutine);
                TimerRoutine = null;
            }
        }



        private void ResetScore()
        {
            Player1Score.text = "0";
            Player1QueenScore.text = "0";
            Player1TotalScore.text = "0";
            Player2Score.text = "0";
            Player2QueenScore.text = "0";
            Player2TotalScore.text = "0";
            player1TurnIdicator.color = IdleColor;
            player2TurnIdicator.color = IdleColor;

        }

        private void ShowBlackAndWhiteScore(int newPlayerID)
        {


            player1CoinImage.sprite = whiteCoin;
            Player1Score.text = (gameData.P1Whites + gameData.P2Whites).ToString();
            Player1QueenScore.text = (gameData.P1Red * 3).ToString();
            Player1TotalScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P1Red * 3).ToString();


            player2CoinImage.sprite = blackCoin;
            Player2Score.text = (gameData.P1Blacks + gameData.P2Blacks).ToString();
            Player2QueenScore.text = (gameData.P2Red * 3).ToString();
            Player2TotalScore.text = (gameData.P1Blacks + gameData.P2Blacks + gameData.P2Red * 3).ToString();


            if (newPlayerID == 1)
            {
                player1TurnIdicator.color = activeColor;
                player2TurnIdicator.color = IdleColor;
            }
            else
            {
                player1TurnIdicator.color = IdleColor;
                player2TurnIdicator.color = activeColor;
            }

        }

        private void ShowFreeStyleScore(int newPlayerID)    
        {

            player1CoinImage.sprite = blackwhiteCoin;
            Player1Score.text = (gameData.P1Whites + gameData.P1Blacks).ToString();
            Player1QueenScore.text = (gameData.P1Red * 3).ToString();
            Player1TotalScore.text = (gameData.P1Whites + gameData.P1Blacks + gameData.P1Red * 3).ToString();


            player2CoinImage.sprite = blackwhiteCoin;
            Player2Score.text = (gameData.P2Whites + gameData.P2Blacks).ToString();
            Player2QueenScore.text = (gameData.P2Red * 3).ToString();
            Player2TotalScore.text = (gameData.P2Whites + gameData.P2Blacks + gameData.P2Red * 3).ToString();


            // change indicator

            if (newPlayerID == 1)
            {
                player1TurnIdicator.color = activeColor;
                player2TurnIdicator.color = IdleColor;
            }
            else
            {
                player1TurnIdicator.color = IdleColor;
                player2TurnIdicator.color = activeColor;
            }


        }
    }
}

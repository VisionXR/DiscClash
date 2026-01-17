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
    public class DisplayScoreFor4 : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        [SerializeField] private GameDataSO gameData;
        [SerializeField] private UIOutputDataSO uiOutputData;  
        
 
        [Header("TeamA Details")]
        [SerializeField] private TMP_Text TeamAScore;
        [SerializeField] private TMP_Text TeamATitle;
        [SerializeField] private TMP_Text TeamAQueenScore;
        [SerializeField] private TMP_Text TeamATotalScore;
        [SerializeField] private Image TeamACoinImage;
        [SerializeField] private Image player1TurnIdicator;
        [SerializeField] private Image player1Timer;
        [SerializeField] private Image player2TurnIdicator;
        [SerializeField] private Image player2Timer;
        [Space(5)]
        [Header("TeamB Details")]
        [SerializeField] private TMP_Text TeamBScore;
        [SerializeField] private TMP_Text TeamBTitle;
        [SerializeField] private TMP_Text TeamBQueenScore;
        [SerializeField] private TMP_Text TeamBTotalScore;
        [SerializeField] private Image TeamBCoinImage;
        [SerializeField] private Image player3TurnIdicator;
        [SerializeField] private Image player3Timer;
        [SerializeField] private Image player4TurnIdicator;
        [SerializeField] private Image player4Timer;

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
                TeamATitle.text = "Black And White";
                TeamBTitle.text = "Black And White";
                ShowBlackAndWhiteScore(newPlayerID);
            }
            else
            {
                TeamATitle.text = "Free Style";
                TeamBTitle.text = "Free Style";
                ShowFreeStyleScore(newPlayerID);
            }

            if (TimerRoutine == null)
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
            player3Timer.fillAmount = 0;
            player4Timer.fillAmount = 0;

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
            else if (id == 3)
            {
                player3Timer.fillAmount = 1; // Start full
                activeTimer = player3Timer;  // Set player 3's timer as active
            }
            else if (id == 4)
            {
                player4Timer.fillAmount = 1; // Start full
                activeTimer = player4Timer;  // Set player 4's timer as active
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

            if (TimerRoutine != null)
            {
                StopCoroutine(TimerRoutine);
                TimerRoutine = null;
            }
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
            TeamAScore.text = "0";
            TeamAQueenScore.text = "0";
            TeamATotalScore.text = "0";

            TeamBScore.text = "0";
            TeamBQueenScore.text = "0";
            TeamBTotalScore.text = "0";

            player1TurnIdicator.color = IdleColor;
            player2TurnIdicator.color = IdleColor;
            player3TurnIdicator.color = IdleColor;
            player4TurnIdicator.color = IdleColor;


        }

        private void ShowBlackAndWhiteScore(int newPlayerID)
        {
            

                TeamACoinImage.sprite = whiteCoin;
                TeamAScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P3Whites + gameData.P4Whites).ToString();
                TeamAQueenScore.text = (gameData.P1Red * 3 + gameData.P2Red * 3).ToString();
                TeamATotalScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P3Whites + gameData.P4Whites + gameData.P1Red * 3 + gameData.P2Red * 3).ToString();

                TeamBCoinImage.sprite = blackCoin;
                TeamBScore.text = (gameData.P1Blacks + gameData.P2Blacks + gameData.P3Blacks + gameData.P4Blacks).ToString();
                TeamBQueenScore.text = (gameData.P3Red * 3 + gameData.P4Red * 3).ToString();
                TeamBTotalScore.text = (gameData.P1Blacks + gameData.P2Blacks + gameData.P3Blacks + gameData.P4Blacks + gameData.P3Red * 3 + gameData.P4Red * 3).ToString();

            

                if (newPlayerID == 1)
                {
                    player1TurnIdicator.color = activeColor;
                    player2TurnIdicator.color = IdleColor;
                    player3TurnIdicator.color = IdleColor;
                    player4TurnIdicator.color = IdleColor;
                }
                else if (newPlayerID == 2)
                {
                    player1TurnIdicator.color = IdleColor;
                    player2TurnIdicator.color = activeColor;
                    player3TurnIdicator.color = IdleColor;
                    player4TurnIdicator.color = IdleColor;
                }
                else if (newPlayerID == 3)
                {
                    player1TurnIdicator.color = IdleColor;
                    player2TurnIdicator.color = IdleColor;
                    player3TurnIdicator.color = activeColor;
                    player4TurnIdicator.color = IdleColor;
                }
                else if (newPlayerID == 4)
                {
                    player1TurnIdicator.color = IdleColor;
                    player2TurnIdicator.color = IdleColor;
                    player3TurnIdicator.color = IdleColor;
                    player4TurnIdicator.color = activeColor;
                }
            
        }

        private void ShowFreeStyleScore(int newPlayerID)
        {


                TeamACoinImage.sprite = blackwhiteCoin;
                TeamAScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P1Blacks + gameData.P2Blacks).ToString();
                TeamAQueenScore.text = (gameData.P1Red * 3 + gameData.P2Red * 3).ToString();
                TeamATotalScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P1Blacks + gameData.P2Blacks + gameData.P1Red * 3 + gameData.P2Red * 3).ToString();

                TeamBCoinImage.sprite = blackwhiteCoin;
                TeamBScore.text = (gameData.P3Whites + gameData.P4Whites + gameData.P3Blacks + gameData.P4Blacks).ToString();
                TeamBQueenScore.text = (gameData.P3Red * 3 + gameData.P4Red * 3).ToString();
                TeamBTotalScore.text = (gameData.P3Whites + gameData.P4Whites + gameData.P3Blacks + gameData.P4Blacks + gameData.P3Red * 3 + gameData.P4Red * 3).ToString();


                if ( newPlayerID == 1)
                {
                    player1TurnIdicator.color = activeColor;
                    player2TurnIdicator.color = IdleColor;
                    player3TurnIdicator.color = IdleColor;
                    player4TurnIdicator.color = IdleColor;
                }
                else if (newPlayerID == 2)
                {
                    player1TurnIdicator.color = IdleColor;
                    player2TurnIdicator.color = activeColor;
                    player3TurnIdicator.color = IdleColor;
                    player4TurnIdicator.color = IdleColor;
                }
                else if ( newPlayerID == 3)
                {
                    player1TurnIdicator.color = IdleColor;
                    player2TurnIdicator.color = IdleColor;
                    player3TurnIdicator.color = activeColor;
                    player4TurnIdicator.color = IdleColor;
                }
                else if (newPlayerID == 4)
                {
                    player1TurnIdicator.color = IdleColor;
                    player2TurnIdicator.color = IdleColor;
                    player3TurnIdicator.color = IdleColor;
                    player4TurnIdicator.color = activeColor;
                }
            
        }
    }
}

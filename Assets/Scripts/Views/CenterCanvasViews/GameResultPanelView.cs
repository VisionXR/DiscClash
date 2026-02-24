using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class GameResultPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects ")]
        public UIOutputDataSO uiOutputData;  
        public UIInputDataSO uiInputData;
        public GameDataSO gameData;
        public PlayersDataSO playersData;
        public GameResultData gameResultData;

        [Header("4 Player Objects ")]
        public GameObject FourPlayersObject;
        public TMP_Text winningTeam;

        public TMP_Text player1Name;
        public TMP_Text player2Name;
        public TMP_Text player3Name;
        public TMP_Text player4Name;


        [Header("2 Player Objects ")]
        public GameObject TwoPlayersObject;

        public TMP_Text winningPlayer;

        public TMP_Text player1Nam;
        public TMP_Text player2Nam;

        [Header("Winning Objects ")]
        public TMP_Text playerCoinsText;
        public TMP_Text winningCoinsText;

        public void ShowResult(GameResult result)
        {
            ResetData();

            winningCoinsText.text = result.coinsWon.ToString();
           

            if (uiOutputData.gameType == GameType.VsCPU)
            {
                if (uiOutputData.gameMode == GameMode.PvsAI)
                {
                    TwoPlayersObject.SetActive(true);
                    SetTwoPlayerData(result);
                }
                else
                {
                    FourPlayersObject.SetActive(true);
                    SetFourPlayerData(result);
                }
            }
            else if ((uiOutputData.gameType == GameType.OnlineMultiPlayer || uiOutputData.gameType == GameType.PlayWithFriends))
            {
                if (uiOutputData.gameMode == GameMode.P1vsP2)
                {
                    TwoPlayersObject.SetActive(true);
                    SetTwoPlayerData(result);
                }
                else
                {
                    FourPlayersObject.SetActive(true);
                    SetFourPlayerData(result);
                }
            }

            if (result.isMainPlayer)
            {
                AudioManager.instance.PlayCoinCollectionSound();
                StartCoroutine(AnimateWinningCoins(3f));
            }
        }

        private void SetTwoPlayerData(GameResult result)
        {
            winningPlayer.text = playersData.GetPlayer(result.winningPlayerId).myName + " Won ";
            player1Nam.text = playersData.GetPlayer(1).myName;
            player2Nam.text = playersData.GetPlayer(2).myName;
        }

        private void SetFourPlayerData(GameResult result)
        {
            winningTeam.text = playersData.GetPlayer(result.winningPlayerId).myTeam + " Won ";
            player1Name.text = playersData.GetPlayer(1).myName;
            player2Name.text = playersData.GetPlayer(2).myName;
            player3Name.text = playersData.GetPlayer(3).myName;
            player4Name.text = playersData.GetPlayer(4).myName;

        }


        private void ResetData()
        {
            TwoPlayersObject.SetActive(false);
            FourPlayersObject.SetActive(false);
        }

        public void OnHomeButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiInputData.GoToHome();

            gameObject.SetActive(false);
          
        }

        public void OnPlayAgainButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiInputData.PlayAgain();
            gameObject.SetActive(false);
          
        }

        /// <summary>
        /// Animate transfer of winning coins into the player's coins UI.
        /// Example: playerCoinsText = "1000", winningCoinsText = "100" => after animation:
        /// winningCoinsText = "0", playerCoinsText = "1100".
        /// The transfer is spread over 'duration' seconds with an ease-out curve.
        /// </summary>
        private IEnumerator AnimateWinningCoins(float duration)
        {
            if (playerCoinsText == null || winningCoinsText == null)
                yield break;

            // Parse current UI values (fallback to 0)
            int playerStart = 0;
            int winStart = 0;

            int.TryParse(playerCoinsText.text, out playerStart);
            int.TryParse(winningCoinsText.text, out winStart);

            // Nothing to transfer
            if (winStart <= 0)
            {
                // Ensure UI is consistent
                winningCoinsText.text = "0";
                playerCoinsText.text = playerStart.ToString();
                yield break;
            }

            float elapsed = 0f;
            int playerTarget = playerStart + winStart;

            while (elapsed < duration)
            {
                float t = Mathf.Clamp01(elapsed / duration);
                // ease-out (quadratic)
                float ease = 1f - Mathf.Pow(1f - t, 2f);

                // current remaining win coins (ease from winStart -> 0)
                int currentWin = Mathf.RoundToInt(Mathf.Lerp(winStart, 0f, ease));
                currentWin = Mathf.Max(0, currentWin);

                // transferred so far
                int transferred = winStart - currentWin;

                // update player displayed coins
                int currentPlayer = playerStart + transferred;

                winningCoinsText.text = currentWin.ToString();
                playerCoinsText.text = currentPlayer.ToString();

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Finalize exact values
            winningCoinsText.text = "0";
            playerCoinsText.text = playerTarget.ToString();
        }

    }
}

using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
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


        public void ShowResult(GameResult result)
        {
            ResetData();

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

    }
}

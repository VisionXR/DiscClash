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

        public Image TeamACoin;
        public Image TeamARedCoin;
        public Image TeamBCoin;
        public Image TeamBRedCoin;


        public TMP_Text TeamAScore;
        public TMP_Text TeamARedScore;
        public TMP_Text TeamATotalScore;

        public TMP_Text TeamBScore;
        public TMP_Text TeamBRedScore;
        public TMP_Text TeamBTotalScore;

        [Header("2 Player Objects ")]
        public GameObject TwoPlayersObject;

        public TMP_Text winningPlayer;

        public TMP_Text player1Nam;
        public TMP_Text player2Nam;


        public Image player1Coin;
        public Image player1RedCoin;
        public Image player2Coin;
        public Image player2RedCoin;


        public TMP_Text player1Score;
        public TMP_Text player1RedScore;
        public TMP_Text player1TotalScore;

        public TMP_Text player2Score;
        public TMP_Text player2RedScore;
        public TMP_Text player2TotalScore;

        [Header("Other Details")]
        public GameObject CenterCanvas;



        public void ShowResult(GameResult result)
        {
            ResetData();

            if (uiOutputData.gameType == GameType.SinglePlayer)
            {
                if(uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
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
            else if(uiOutputData.gameType == GameType.MultiPlayer)
            {
                if(uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
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

            if(uiOutputData.game == Game.BlackAndWhite)
            {
                ShowBlackAndWhite2PlayerScore();
            }
            else if(uiOutputData.game == Game.FreeStyle)
            {
                ShowFreeStyle2PlayerScore();
            }
        }

        private void SetFourPlayerData(GameResult result)
        {
            winningTeam.text = playersData.GetPlayer(result.winningPlayerId).myTeam + " Won ";
            player1Name.text = playersData.GetPlayer(1).myName;
            player2Name.text = playersData.GetPlayer(2).myName;
            player3Name.text = playersData.GetPlayer(3).myName;
            player4Name.text = playersData.GetPlayer(4).myName;

            if (uiOutputData.game == Game.BlackAndWhite)
            {
                ShowBlackAndWhite4PlayerScore();
            }
            else if (uiOutputData.game == Game.FreeStyle)
            {
                ShowFreeStyle4PlayerScore();
            }

        }

        private void ShowBlackAndWhite2PlayerScore()
        {

            Player player1 = playersData.GetPlayer(1);
            if (player1 != null)
            {
                if (player1.myCoin == PlayerCoin.White)
                {
                    player1Coin.sprite = uiOutputData.WhiteCoin;
                    player2Coin.sprite = uiOutputData.BlackCoin;
                    player1RedCoin.sprite = uiOutputData.RedCoin;
                    player2RedCoin.sprite = uiOutputData.RedCoin;

                    player1Score.text = (gameData.P1Whites + gameData.P2Whites).ToString();
                    player1RedScore.text = (gameData.P1Red * 3).ToString();
                    player1TotalScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P1Red * 3).ToString();

                }
                else
                {
                    player2Coin.sprite = uiOutputData.WhiteCoin;
                    player1Coin.sprite = uiOutputData.BlackCoin;
                    player1RedCoin.sprite = uiOutputData.RedCoin;
                    player2RedCoin.sprite = uiOutputData.RedCoin;
                    player1Score.text = (gameData.P1Blacks + gameData.P2Blacks).ToString();
                    player1TotalScore.text = (gameData.P1Blacks + gameData.P2Blacks + gameData.P1Red * 3).ToString();
                    player1RedScore.text = (gameData.P1Red * 3).ToString();
                }
            }


            Player player2 = playersData.GetPlayer(2);

            if (player2 != null)
            {
                if (player2.myCoin == PlayerCoin.White)
                {
                    player2Score.text = (gameData.P1Whites + gameData.P2Whites).ToString();
                    player2RedScore.text = (gameData.P2Red * 3).ToString();
                    player2TotalScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P2Red * 3).ToString();
                }
                else
                {

                    player2Score.text = (gameData.P1Blacks + gameData.P2Blacks).ToString();
                    player2RedScore.text = (gameData.P2Red * 3).ToString();
                    player2TotalScore.text = (gameData.P1Blacks + gameData.P2Blacks + gameData.P2Red * 3).ToString();
                }
            }

           
        }
        private void ShowFreeStyle2PlayerScore()
        {
            Player player1 = playersData.GetPlayer(1);

            if (player1 != null)
            {
                player1Coin.sprite = uiOutputData.BlackAndWhiteCoin;
                player1RedCoin.sprite = uiOutputData.RedCoin;
                player1Score.text = (gameData.P1Whites + gameData.P1Blacks).ToString();
                player1RedScore.text = (gameData.P1Red * 3).ToString();
                player1TotalScore.text = (gameData.P1Whites + gameData.P1Blacks + gameData.P1Red * 3).ToString();
            }


            Player player2 = playersData.GetPlayer(2);

            if (player2 != null)
            {
                player2Coin.sprite = uiOutputData.BlackAndWhiteCoin;
                player2RedCoin.sprite = uiOutputData.RedCoin;
                player2Score.text = (gameData.P2Whites + gameData.P2Blacks).ToString();
                player2RedScore.text = (gameData.P2Red * 3).ToString();
                player2TotalScore.text = (gameData.P2Whites + gameData.P2Blacks + gameData.P2Red * 3).ToString();
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
                TeamACoin.sprite = uiOutputData.WhiteCoin;
                TeamARedCoin.sprite = uiOutputData.RedCoin;
                TeamAScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P3Whites + gameData.P4Whites).ToString();
                TeamARedScore.text = (gameData.P1Red * 3 + gameData.P2Red * 3).ToString();
                TeamATotalScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P3Whites + gameData.P4Whites + gameData.P1Red * 3 + gameData.P2Red * 3).ToString();

            }

            if (p3 != null && p4 != null)
            {
                TeamBCoin.sprite = uiOutputData.BlackCoin;
                TeamBRedCoin.sprite = uiOutputData.RedCoin;   
                TeamBScore.text = (gameData.P1Blacks + gameData.P2Blacks + gameData.P3Blacks + gameData.P4Blacks).ToString();
                TeamBRedScore.text = (gameData.P3Red * 3 + gameData.P4Red * 3).ToString();
                TeamBTotalScore.text = (gameData.P1Blacks + gameData.P2Blacks + gameData.P3Blacks + gameData.P4Blacks + gameData.P3Red * 3 + gameData.P4Red * 3).ToString();

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
                TeamACoin.sprite = uiOutputData.BlackAndWhiteCoin;
                TeamARedCoin.sprite = uiOutputData.RedCoin;
                TeamAScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P1Blacks + gameData.P2Blacks).ToString();
                TeamARedScore.text = (gameData.P1Red * 3 + gameData.P2Red * 3).ToString();
                TeamATotalScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P1Blacks + gameData.P2Blacks + gameData.P1Red * 3 + gameData.P2Red * 3).ToString();
            }

            if (p3 != null && p4 != null)
            {
                TeamBCoin.sprite = uiOutputData.BlackAndWhiteCoin;
                TeamBRedCoin.sprite = uiOutputData.RedCoin;
                TeamBScore.text = (gameData.P3Whites + gameData.P4Whites + gameData.P3Blacks + gameData.P4Blacks).ToString();
                TeamBRedScore.text = (gameData.P3Red * 3 + gameData.P4Red * 3).ToString();
                TeamBTotalScore.text = (gameData.P3Whites + gameData.P4Whites + gameData.P3Blacks + gameData.P4Blacks + gameData.P3Red * 3 + gameData.P4Red * 3).ToString();

            }
          
        }

        private void ResetData()
        {
            TwoPlayersObject.SetActive(false);
            FourPlayersObject.SetActive(false);
        }

        public void OnHomeButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.GoToHome();

            gameObject.SetActive(false);
            CenterCanvas.SetActive(false);
        }

        public void OnPlayAgainButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();        
            uiOutputData.PlayAgain();   
            gameObject.SetActive(false);
            CenterCanvas.SetActive(false);
        }

    }
}

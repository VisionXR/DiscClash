using TMPro;    
using UnityEngine;
using UnityEngine.UI;
using com.VisionXR.ModelClasses;
using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;

namespace com.VisionXR.Views
{
    public class SPvs1AIScorePanelView : MonoBehaviour
    {
        [Header("Scriptable Objecst")]
        [SerializeField] private GameDataSO gameData;
        [SerializeField] private UIOutputDataSO uiOutputData;
        [SerializeField] private PlayersDataSO playersData;

        [Header("Player1 Details")]
        [SerializeField] private TMP_Text player1Title;
        [SerializeField] private TMP_Text player1Name;
        [SerializeField] private Image player1Image;
        [SerializeField] private Image player1TurnIdicator;
        [SerializeField] private TMP_Text player1Score;
        [SerializeField] private TMP_Text player1QueenScore;
        [SerializeField] private TMP_Text player1TotalScore;   
        [SerializeField] private Image player1CoinImage;

        [Header("Player2 Details")]
        [SerializeField] private TMP_Text player2Title;
        [SerializeField] private TMP_Text player2Name;
        [SerializeField] private Image player2Image;
        [SerializeField] private Image player2TurnIdicator;
        [SerializeField] private TMP_Text player2Score;
        [SerializeField] private TMP_Text player2QueenScore;
        [SerializeField] private TMP_Text player2TotalScore;
        [SerializeField] private Image player2CoinImage;

        [Header("Other Details")]
        [SerializeField] private Sprite whiteCoin;
        [SerializeField] private Sprite blackCoin;
        [SerializeField] private Sprite blackwhiteCoin;
        [SerializeField] private Color activeColor;
        [SerializeField] private Color IdleColor;

        private void OnEnable()
        {
            gameData.TurnChangedEvent += ShowScore;
            uiOutputData.StartSinglePlayerGameEvent += SetTitle;
          
        }

        private void OnDisable()
        {
            gameData.TurnChangedEvent -= ShowScore;
            uiOutputData.StartSinglePlayerGameEvent -= SetTitle;
        
        }


        public void SetTitle()
        {
            ResetScore();

            if (uiOutputData.game == Game.BlackAndWhite)
            {
                player1Title.text = "Black And White";
                player2Title.text = "Black And White";
               
            }
            else
            {
                player1Title.text = "Free Style";
                player2Title.text = "Free Style";
            
            }
        }

        private void ShowImageForPlayer(Player p)
        {
            if (p.myId == 1)
            {
                if (p.GetMyImage() != null)
                {
                    player1Image.sprite = p.GetMyImage();
                }

            }
            else if (p.myId == 2)
            {
                if (p.GetMyImage() != null)
                {
                    player2Image.sprite = p.GetMyImage();
                }

            }
        }
        private void ResetScore()
        {

            player1Score.text = "0";
            player1QueenScore.text = "0";
            player1TotalScore.text = "0";
            player2Score.text = "0";
            player2QueenScore.text = "0";
            player2TotalScore.text = "0";
            player1Name.text = "";
            player2Name.text = "";
            player1TurnIdicator.color = IdleColor;
            player2TurnIdicator.color = IdleColor;
        }
        private void ShowScore(int newPlayerID = 1)
        {
            // setting title
            if (uiOutputData.game == Game.BlackAndWhite)
            {
                ShowBlackAndWhiteScore(newPlayerID);
            }
            else
            {
                ShowFreeStyleScore(newPlayerID);
            }

        }

        private void ShowBlackAndWhiteScore(int newPlayerID)
        {


            Player player1 = playersData.GetPlayer(1);
            SetPlayerData(player1.myId, player1.myName, player1.GetMyImage());
            if (player1.myCoin == PlayerCoin.White)
            {
                player1CoinImage.sprite = whiteCoin;
                player2CoinImage.sprite = blackCoin;
                player1Score.text = (gameData.P1Whites + gameData.P2Whites).ToString();
                player1QueenScore.text = (gameData.P1Red * 3).ToString();
                player1TotalScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P1Red * 3).ToString();

            }
            else
            {
                player2CoinImage.sprite = whiteCoin;
                player1CoinImage.sprite = blackCoin;
                player1Score.text = (gameData.P1Blacks + gameData.P2Blacks).ToString();
                player1TotalScore.text = (gameData.P1Blacks + gameData.P2Blacks + gameData.P1Red * 3).ToString();
                player1QueenScore.text = (gameData.P1Red * 3).ToString();
            }


            Player player2 = playersData.GetPlayer(2);
            SetPlayerData(player2.myId, player2.myName, player2.GetMyImage());

            if (player2.myCoin == PlayerCoin.White)
            {
                player2Score.text = (gameData.P1Whites + gameData.P2Whites).ToString();
                player2QueenScore.text = (gameData.P2Red * 3).ToString();
                player2TotalScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P2Red * 3).ToString();
            }
            else
            {

                player2Score.text = (gameData.P1Blacks + gameData.P2Blacks).ToString();
                player2QueenScore.text = (gameData.P2Red * 3).ToString();
                player2TotalScore.text = (gameData.P1Blacks + gameData.P2Blacks + gameData.P2Red * 3).ToString();
            }

            // change indicator

            if (player1.myId == newPlayerID)
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
            player2CoinImage.sprite = blackwhiteCoin;

            Player player1 = playersData.GetPlayer(1);
            player1Name.text = player1.myName;
            ShowImageForPlayer(player1);


            player1Score.text = (gameData.P1Whites + gameData.P1Blacks).ToString();
            player1QueenScore.text = (gameData.P1Red * 3).ToString();
            player1TotalScore.text = (gameData.P1Whites + gameData.P1Blacks + gameData.P1Red * 3).ToString();


            Player player2 = playersData.GetPlayer(2);
            player2Name.text = player2.myName;
            ShowImageForPlayer(player2);

            player2Score.text = (gameData.P2Whites + gameData.P2Blacks).ToString();
            player2QueenScore.text = (gameData.P2Red * 3).ToString();
            player2TotalScore.text = (gameData.P2Whites + gameData.P2Blacks + gameData.P2Red * 3).ToString();


            // change indicator

            if (player1.myId == newPlayerID)
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


        private void SetPlayerData(int playerId, string name, Sprite image)
        {
            TMP_Text nameText = playerId == 1 ? player1Name : player2Name;
            Image playerImage = playerId == 1 ? player1Image : player2Image;          
            nameText.text = name;
            playerImage.sprite = image;

        }
    }
}

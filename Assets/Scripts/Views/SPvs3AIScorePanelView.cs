using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class SPvs3AIScorePanelView : MonoBehaviour
    {
        [Header(" scriptable Objects")]
        [SerializeField] private GameDataSO gameData;
        [SerializeField] private UIOutputDataSO uiOutputData;
        [SerializeField] private PlayersDataSO playersData;

        [Header("TeamA Details")]
        [SerializeField] private TMP_Text TeamATitle;
        [SerializeField] private TMP_Text player1Name;
        [SerializeField] private TMP_Text player2Name;
        [SerializeField] private Image player1Image;
        [SerializeField] private Image player2Image;
        [SerializeField] private Image player1TurnIndicator;
        [SerializeField] private Image player2TurnIndicator;
        [SerializeField] private TMP_Text teamAScore;
        [SerializeField] private TMP_Text teamAQueenScore;
        [SerializeField] private TMP_Text teamATotalScore;
        [SerializeField] private Image teamACoinImage;

        [Header("TeamB Details")]
        [SerializeField] private TMP_Text TeamBTitle;
        [SerializeField] private TMP_Text player3Name;
        [SerializeField] private TMP_Text player4Name;
        [SerializeField] private Image player3Image;
        [SerializeField] private Image player4Image;
        [SerializeField] private Image player3TurnIndicator;
        [SerializeField] private Image player4TurnIndicator;
        [SerializeField] private TMP_Text teamBScore;
        [SerializeField] private TMP_Text teamBQueenScore;
        [SerializeField] private TMP_Text teamBTotalScore;
        [SerializeField] private Image teamBCoinImage;

        [Header("Other Varaibles")]
        [SerializeField] private Sprite whiteCoin;
        [SerializeField] private Sprite blackCoin;
        [SerializeField] private Sprite blackwhiteCoin;
        [SerializeField] private Color activeColor;
        [SerializeField] private Color IdleColor;


        private void OnEnable()
        {
            gameData.TurnChangedEvent += ShowScore;
            ResetScore();
        }

        private void OnDisable()
        {
            gameData.TurnChangedEvent -= ShowScore;
            ResetScore();
        }

        private void ResetScore()
        {
            teamAScore.text = "0";
            teamAQueenScore.text = "0";
            teamATotalScore.text = "0";
            teamBScore.text = "0";
            teamBQueenScore.text = "0";
            teamBTotalScore.text = "0";
            player1TurnIndicator.color = IdleColor;
            player2TurnIndicator.color = IdleColor;
            player3TurnIndicator.color = IdleColor;
            player4TurnIndicator.color = IdleColor;
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
            else if (p.myId == 3)
            {
                if (p.GetMyImage() != null)
                {
                    player3Image.sprite = p.GetMyImage();
                }

            }
            else if (p.myId == 4)
            {
                if (p.GetMyImage() != null)
                {
                    player4Image.sprite = p.GetMyImage();
                }

            }
        }
        private void ShowScore(int newPlayerID = 1)
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



        }

        private void ShowBlackAndWhiteScore(int newPlayerID)
        {
            Player p1 = playersData.GetPlayer(1);
            player1Name.text = p1.myName;
            ShowImageForPlayer(p1);

            Player p2 = playersData.GetPlayer(2);
            player2Name.text = p2.myName;
            ShowImageForPlayer(p2);

            Player p3 = playersData.GetPlayer(3);
            player3Name.text = p3.myName;
            ShowImageForPlayer(p3);

            Player p4 = playersData.GetPlayer(4);
            player4Name.text = p4.myName;
            ShowImageForPlayer(p4);

            if (p1.myCoin == PlayerCoin.White)
            {

                teamACoinImage.sprite = whiteCoin;
                teamBCoinImage.sprite = blackCoin;

                teamAScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P3Whites + gameData.P4Whites).ToString();
                teamAQueenScore.text = (gameData.P1Red * 3 + gameData.P2Red * 3).ToString();
                teamATotalScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P3Whites + gameData.P4Whites + gameData.P1Red * 3 + gameData.P2Red * 3).ToString();

                teamBScore.text = (gameData.P1Blacks + gameData.P2Blacks + gameData.P3Blacks + gameData.P4Blacks).ToString();
                teamBQueenScore.text = (gameData.P3Red * 3 + gameData.P4Red * 3).ToString();
                teamBTotalScore.text = (gameData.P1Blacks + gameData.P2Blacks + gameData.P3Blacks + gameData.P4Blacks + gameData.P3Red * 3 + gameData.P4Red * 3).ToString();
            }
            else
            {
                teamACoinImage.sprite = blackCoin;
                teamBCoinImage.sprite = whiteCoin;

                teamBScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P3Whites + gameData.P4Whites).ToString();
                teamBQueenScore.text = (gameData.P3Red * 3 + gameData.P4Red * 3).ToString();
                teamBTotalScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P3Whites + gameData.P4Whites + gameData.P3Red * 3 + gameData.P4Red * 3).ToString();

                teamAScore.text = (gameData.P1Blacks + gameData.P2Blacks + gameData.P3Blacks + gameData.P4Blacks).ToString();
                teamAQueenScore.text = (gameData.P1Red * 3 + gameData.P2Red * 3).ToString();
                teamATotalScore.text = (gameData.P1Blacks + gameData.P2Blacks + gameData.P3Blacks + gameData.P4Blacks + gameData.P1Red * 3 + gameData.P2Red * 3).ToString();
            }

            // change indicator

            if (p1.myId == newPlayerID)
            {
                player1TurnIndicator.color = activeColor;
                player2TurnIndicator.color = IdleColor;
                player3TurnIndicator.color = IdleColor;
                player4TurnIndicator.color = IdleColor;
            }
            else if (p2.myId == newPlayerID)
            {
                player1TurnIndicator.color = IdleColor;
                player2TurnIndicator.color = activeColor;
                player3TurnIndicator.color = IdleColor;
                player4TurnIndicator.color = IdleColor;
            }
            else if (p3.myId == newPlayerID)
            {
                player1TurnIndicator.color = IdleColor;
                player2TurnIndicator.color = IdleColor;
                player3TurnIndicator.color = activeColor;
                player4TurnIndicator.color = IdleColor;
            }
            else if (p4.myId == newPlayerID)
            {
                player1TurnIndicator.color = IdleColor;
                player2TurnIndicator.color = IdleColor;
                player3TurnIndicator.color = IdleColor;
                player4TurnIndicator.color = activeColor;
            }
        }

        private void ShowFreeStyleScore(int newPlayerID)
        {
            Player p1 = playersData.GetPlayer(1);
            player1Name.text = p1.myName;
            ShowImageForPlayer(p1);

            Player p2 = playersData.GetPlayer(2);
            player2Name.text = p2.myName;
            ShowImageForPlayer(p2);

            Player p3 = playersData.GetPlayer(3);
            player3Name.text = p3.myName;
            ShowImageForPlayer(p3);

            Player p4 = playersData.GetPlayer(4);
            player4Name.text = p4.myName;
            ShowImageForPlayer(p4);


            teamACoinImage.sprite = blackwhiteCoin;
            teamBCoinImage.sprite = blackwhiteCoin;

            teamAScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P1Blacks + gameData.P2Blacks).ToString();
            teamAQueenScore.text = (gameData.P1Red * 3 + gameData.P2Red * 3).ToString();
            teamATotalScore.text = (gameData.P1Whites + gameData.P2Whites + gameData.P1Blacks + gameData.P2Blacks + gameData.P1Red * 3 + gameData.P2Red * 3).ToString();

            teamBScore.text = (gameData.P3Whites + gameData.P4Whites + gameData.P3Blacks + gameData.P4Blacks).ToString();
            teamBQueenScore.text = (gameData.P3Red * 3 + gameData.P4Red * 3).ToString();
            teamBTotalScore.text = (gameData.P3Whites + gameData.P4Whites + gameData.P3Blacks + gameData.P4Blacks + gameData.P3Red * 3 + gameData.P4Red * 3).ToString();

            // change indicator

            if (p1.myId == newPlayerID)
            {
                player1TurnIndicator.color = activeColor;
                player2TurnIndicator.color = IdleColor;
                player3TurnIndicator.color = IdleColor;
                player4TurnIndicator.color = IdleColor;
            }
            else if (p2.myId == newPlayerID)
            {
                player1TurnIndicator.color = IdleColor;
                player2TurnIndicator.color = activeColor;
                player3TurnIndicator.color = IdleColor;
                player4TurnIndicator.color = IdleColor;
            }
            else if (p3.myId == newPlayerID)
            {
                player1TurnIndicator.color = IdleColor;
                player2TurnIndicator.color = IdleColor;
                player3TurnIndicator.color = activeColor;
                player4TurnIndicator.color = IdleColor;
            }
            else if (p4.myId == newPlayerID)
            {
                player1TurnIndicator.color = IdleColor;
                player2TurnIndicator.color = IdleColor;
                player3TurnIndicator.color = IdleColor;
                player4TurnIndicator.color = activeColor;
            }
        }
    }
}

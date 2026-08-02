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
        public UIDataSO uiData;


        [Header("UI Elements")]
        public TMP_Text gameNameText;
        public GameObject P1Winner;
        public GameObject P2Winner;
        public Image P1Image;
        public Image P2Image;
        public TMP_Text player1Nam;
        public TMP_Text player2Nam;
        public string leaderBoardState;

        public void ShowResult(GameResult result)
        {
           

            if (uiOutputData.gameType == GameType.VsCPU)
            {
                if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
                {
                    gameNameText.text = "Game : P vs AI";
                    SetTwoPlayerData(result);
                }
                else if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PAIvsAI)
                {
                    gameNameText.text = "Game : PAI vs AI";
                    SetFourPlayerData(result);
                }

            }
            else if ( uiOutputData.gameType == GameType.PlayWithFriends)
            {
                if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
                {
                    gameNameText.text = "Game : P1 vs P2";
                    SetTwoPlayerData(result);
                }
                else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1AIvsP2AI )
                {
                    gameNameText.text = "Game : P1AI vs P2AI";                     
                    SetFourPlayerData(result);
                }
                else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1P2vsAI)
                {
                    gameNameText.text = "Game : P1P2 vs AI";                     
                    SetFourPlayerData(result);
                }
            }


        }

        private void SetTwoPlayerData(GameResult result)
        {
            if(result.winningPlayerId == 1)
            {
                P1Winner.SetActive(true);
                P2Winner.SetActive(false);
            }
            else if(result.winningPlayerId == 2)
            {
                P1Winner.SetActive(false);
                P2Winner.SetActive(true);
            }

            P1Image.sprite = playersData.GetPlayer(1).GetMyImage();
            P2Image.sprite = playersData.GetPlayer(2).GetMyImage();
            player1Nam.text = playersData.GetPlayer(1).myName;
            player2Nam.text = playersData.GetPlayer(2).myName;
        }

        private void SetFourPlayerData(GameResult result)
        {
            if (result.winningTeam == Team.TeamA)
            {
                P1Winner.SetActive(true);
                P2Winner.SetActive(false);
            }
            else if (result.winningTeam == Team.TeamB)
            {
                P1Winner.SetActive(false);
                P2Winner.SetActive(true);
            }

            if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1AIvsP2AI)
            {
                P1Image.sprite = playersData.GetPlayer(1).GetMyImage();
                player1Nam.text = playersData.GetPlayer(1).myName;
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1P2vsAI)
            {
                Player p = playersData.GetMainPlayer();
                if (p != null)
                {
                    P1Image.sprite = playersData.GetPlayer(p.myId).GetMyImage();
                    player1Nam.text = playersData.GetPlayer(p.myId).myName;
                }
            }


            if (uiOutputData.gameType == GameType.VsCPU)
            {
                P2Image.sprite = playersData.GetPlayer(2).GetMyImage();
                player2Nam.text = playersData.GetPlayer(2).myName;
            }
            else if (uiOutputData.gameType == GameType.PlayWithFriends)
            {
               
                    P2Image.sprite = playersData.GetPlayer(3).GetMyImage();
                    player2Nam.text = playersData.GetPlayer(3).myName;
                
            }

        }

        public void OnHomeButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiInputData.GoToHome();
            uiData.uiManager.ChangeState("SinglePlayer", false);
            uiData.uiManager.ChangeState("MultiPlayer", false);
            uiData.uiManager.ChangeState("Tutorial", false);
            uiData.uiManager.GoToState(StateName.HomeState);
            uiData.uiManager.ResetAllBools();
        }

        public void LeaderBoardBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(leaderBoardState, true);
        }

        public void OnPlayAgainButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState("GameCompleted", false);

            int id = 1;
            if(gameData.firstTurnId == 1)
            {
                id = 2;
            }
            uiInputData.PlayAgain(id);
          
        }

       

    }
}

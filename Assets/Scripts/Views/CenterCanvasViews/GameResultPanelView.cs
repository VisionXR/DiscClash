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
        public GameObject P1Winner;
        public GameObject P2Winner;
        public Image P1Image;
        public Image P2Image;
        public TMP_Text player1Nam;
        public TMP_Text player2Nam;


        public void ShowResult(GameResult result)
        {
           

            if (uiOutputData.gameType == GameType.VsCPU)
            {
                if (uiOutputData.gameMode == GameMode.PvsAI)
                {
                    
                    SetTwoPlayerData(result);
                }

            }
            else if ((uiOutputData.gameType == GameType.OnlineMultiPlayer || uiOutputData.gameType == GameType.PlayWithFriends))
            {
                if (uiOutputData.gameMode == GameMode.P1vsP2)
                {
                  
                    SetTwoPlayerData(result);
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

        public void OnPlayAgainButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState("GameCompleted", false);
            uiInputData.PlayAgain();
          
        }

       

    }
}

using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class TournamentBoardComplete : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public GameDataSO gameData;
        public UIDataSO uiData;
        public NetworkOutputSO networkOutput;
        public NetworkInputSO networkInput;

        [Header("States")]
        public TMP_Text boardNoText;
        public DataManager dataManager;
        public string tournamentBoardResultState;

        [Header("UI Elements")]
        public GameObject NextBtn;
        public GameObject HomeBtn;
        public GameObject ClientText;

        // T10: Latch immediately, before the outgoing panel animation can accept another click.
        private bool nextRequested;

        private void OnEnable()
        {
            nextRequested = false;
            networkInput.StartNewBoardEvent += StartNextBoard;

            boardNoText.text = "Board " + (gameData.tournamentData.currentBoardNo + 1).ToString() + " Completed";

            if (uiOutputData.gameType == GameType.VsCPU)
            {
                NextBtn.SetActive(true);
                HomeBtn.SetActive(true);
                ClientText.SetActive(false);
            }
            else if (uiOutputData.gameType == GameType.PlayWithFriends)
            {
                if(networkOutput.isHost)
                {
                    NextBtn.SetActive(true);
                    HomeBtn.SetActive(true);
                    ClientText.SetActive(false);
                }
                else
                {
                    NextBtn.SetActive(false);
                    // T06: A waiting client must always have an exit option.
                    HomeBtn.SetActive(true);
                    ClientText.SetActive(true);
                }
            }
        }

        private void OnDisable()
        {
            networkInput.StartNewBoardEvent -= StartNextBoard;

        }


        public void NextBoardBtnClicked()
        {
            // T10: Only the CPU player or online host may request one next board per result screen.
            if (nextRequested || (uiOutputData.gameType == GameType.PlayWithFriends && !networkOutput.isHost)) return;
            nextRequested = true;
            NextBtn.SetActive(false);
            AudioManager.instance.PlayButtonClickSound();

            if (uiOutputData.gameType == GameType.VsCPU)
            {
                uiInputData.PlayNextBoard();
                uiData.uiManager.ChangeState(tournamentBoardResultState, false);
            }
            else if (uiOutputData.gameType == GameType.PlayWithFriends)
            {
                dataManager.SendTournamentBoardComplete();
                NextBtn.SetActive(false);
                HomeBtn.SetActive(false);
            }
        }

        public void StartNextBoard()
        {
            uiInputData.PlayNextBoard();
            uiData.uiManager.ChangeState(tournamentBoardResultState, false);
        }

        public void HomeBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            gameData.ResetData();
            uiInputData.GoToHome();
            uiData.uiManager.ChangeState("SinglePlayer", false);
            uiData.uiManager.ChangeState("MultiPlayer", false);
            uiData.uiManager.ChangeState("Tutorial", false);
            uiData.uiManager.GoToState(StateName.HomeState);
            uiData.uiManager.ResetAllBools();
        }
    }
}

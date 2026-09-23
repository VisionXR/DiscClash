using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
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
        public string tournamentBoardResultState;

        [Header("UI Elements")]
        public GameObject NextBtn;
        public GameObject HomeBtn;
        public GameObject ClientText;

        private void OnEnable()
        {
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
                    HomeBtn.SetActive(false);
                    ClientText.SetActive(true);
                }
            }
        }


        public void NextBoardBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            if (uiOutputData.gameType == GameType.VsCPU)
            {
                uiInputData.PlayNextBoard();
                uiData.uiManager.ChangeState(tournamentBoardResultState, false);
            }
            else if (uiOutputData.gameType == GameType.PlayWithFriends)
            {
                
            }
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

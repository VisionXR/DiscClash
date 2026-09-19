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


        public void NextBoardBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiInputData.PlayNextBoard();
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

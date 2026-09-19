using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class TournamentScoreView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;


        [Header("Buttons")]
        public PanelOnOff tournamentPanel;
        public GameObject LeftBtn;
        public GameObject RightBtn;


        private void OnEnable()
        {
            if (uiOutputData.challenge == Challenge.BWTournament || uiOutputData.challenge == Challenge.FSTournament)
            {
                LeftBtn.SetActive(true);
                RightBtn.SetActive(true);
            }
            else
            {
                LeftBtn.SetActive(false);
                RightBtn.SetActive(false);
            }
        }


        public void TournamentBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            if (tournamentPanel.gameObject.activeInHierarchy)
            {
                tournamentPanel.TurnOffPanel();
            }
            else
            {
                tournamentPanel.TurnOnPanel();
            }
        }
    }
}

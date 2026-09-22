using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class TournamentPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public GameDataSO gameData;
        public UIDataSO uiData;
        public UIOutputDataSO uiOutputData;

        [Header("Name List")]
        public TMP_Text P1Name;
        public TMP_Text P2Name;

        [Header("Score List")]
        public TMP_Text P1TotalScore;
        public TMP_Text P2TotalScore;
        public TournamentScoreList scoreList;


        private void OnEnable()
        {
            for (int i = 0; i < gameData.tournamentData.P1Scores.Count; i++)
            {
                scoreList.P1Scores[i].text = gameData.tournamentData.P1Scores[i].ToString();
                scoreList.P2Scores[i].text = gameData.tournamentData.P2Scores[i].ToString();
            }

            P1TotalScore.text = gameData.tournamentData.P1TotalScore.ToString();
            P2TotalScore.text = gameData.tournamentData.P2TotalScore.ToString();


            if (uiOutputData.gameType == GameType.VsCPU && uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
            {
                P1Name.text = "P1";
                P2Name.text = "P2";
            }
            else if (uiOutputData.gameType == GameType.PlayWithFriends && uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
            {
                P1Name.text = "P1";
                P2Name.text = "P2";
            }
            else
            {
                P1Name.text = "TeamA";
                P2Name.text = "TeamB";
            }
        }

    }

    [Serializable]
    public class TournamentScoreList
    {      
        public List<TMP_Text> P1Scores;
        public List<TMP_Text> P2Scores;
    }
}

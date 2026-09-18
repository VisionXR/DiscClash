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
        [Header("Score List")]
        public TMP_Text P1TotalScore;
        public TMP_Text P2TotalScore;
        public TournamentScoreList scoreList;
        
    }

    [Serializable]
    public class TournamentScoreList
    {
        public TMP_Text P1Name;
        public List<TMP_Text> P1Scores;

        public TMP_Text P2Name;
        public List<TMP_Text> P2Scores;
    }
}

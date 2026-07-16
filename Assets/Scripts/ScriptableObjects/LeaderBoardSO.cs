
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "LeaderBoardSO", menuName = "ScriptableObjects/LeaderBoardSO", order = 1)]
    public class LeaderBoardSO : ScriptableObject
    {
        // variables
        public List<LeaderBoardPoints> leaderBoardPoints;

        // Action
     
        public Action<string> GetTop10ScoresEvent;
        public Action<int,string> WriteToLeaderBoardEvent;
        public Action<List<string>, List<int>, List<int>> ShowLeaderBoardDataEvent;


        // Methods
        public void WriteToLeaderBoard(int points,string apiName)
        {
            WriteToLeaderBoardEvent?.Invoke(points,apiName);
        }

        public void GetTop10Scores(string apiName)
        {
            GetTop10ScoresEvent?.Invoke(apiName);
        }



        public void ShowLeaderBoardData(List<string> names,List<int> ranks,List<int> points)
        {
            ShowLeaderBoardDataEvent?.Invoke(names, ranks, points);
        }



   
    }


    [Serializable]
    public class  LeaderBoardPoints
    {
        public string apiName;
        public int wins;
        public int rank;
    }
}

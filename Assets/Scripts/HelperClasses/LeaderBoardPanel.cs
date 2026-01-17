using com.VisionXR.ModelClasses;

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace com.VisionXR.HelperClasses
{
    public class LeaderBoardPanel : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public MyPlayerSettings settings;
        public LeaderBoardSO leaderBoard;

        [Header(" Game Objects")]
        public GameObject MainPanel;
        public TMP_Text myRankText;
        public TMP_Dropdown ApiDD;


        [Header(" Local Objects")]
        public List<TMP_Text> Names;
        public List<TMP_Text> Ranks;
        public List<TMP_Text> Points;
        


        public string apiName = "MultiPlayer";
            
        private void OnEnable()
        {
         
        }

        private void OnDisable()
        {
            
        }

        public void ShowLeaderBoard(List<string> names, List<int> ranks, List<int> points)
        {
            myRankText.text = "My Rank : " + leaderBoard.GetRankByApiName(ApiDD.value);
            ClearAllText();
            for (int i = 0; i < names.Count; i++)
            {
                Names[i].text = names[i];
                Ranks[i].text = ranks[i].ToString();
                Points[i].text = points[i].ToString();
            }
        }

        private void ClearAllText()
        {
            for (int i = 0; i < Names.Count; i++)
            {
                Names[i].text = "";
                Ranks[i].text = "";
                Points[i].text = "";
            }
        }


        public void ApiDDChanged(int value)
        {
            AudioManager.instance.PlayButtonClickSound();
            if(value==0)
            {
                apiName = "MultiPlayer";
            }
            else if(value==1)
            {
                apiName = "SinglePlayer";
            }
            else if(value==2)
            {
                apiName = "TotalGames";
            }
            
        }

        public void StartAtDDChanged(int value)
        {
            AudioManager.instance.PlayButtonClickSound();
           
        }
    }
}

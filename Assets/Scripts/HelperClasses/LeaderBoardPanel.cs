using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class LeaderBoardPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public MyPlayerSettings settings;
        public LeaderBoardSO leaderBoard;
       
        public UIDataSO uiData;

        [Header("Tab Objects")]
        public GameObject leaderBoardItem; // Prefab representing a row entry
        public Transform leaderBoardItemParent; // Grid layout group or vertical panel parent
        public List<GameObject> SelectionImages;

        [Header("Panel Objects")]
        public string currentState;

        private int currentId = 0;

        private void OnEnable()
        {
            leaderBoard.ShowLeaderBoardDataEvent += ShowLeaderBoard;
            TabButtonClicked(0);
        }

        private void OnDisable()
        {
            leaderBoard.ShowLeaderBoardDataEvent -= ShowLeaderBoard;
        }

        public void TabButtonClicked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetTabs();
            SelectionImages[id].SetActive(true);

            currentId = id;
            string apiName = "SinglePlayer";
            // 1. Wipe current list clean
            ClearAllText();
            if(id == 1)
            {
                apiName = "MultiPlayer";
            }
            leaderBoard.GetTop10Scores(apiName); // Fetch new data based on the selected tab's API name
        }

        private void ResetTabs()
        {
            foreach (var img in SelectionImages)
            {
                img.SetActive(false);
            }
        }

        /// <summary>
        /// Clears old runtime row clones and instantiates a new leaderboard list
        /// </summary>
        public void ShowLeaderBoard(List<string> names, List<int> ranks, List<int> points)
        {
            Debug.Log("Leaderboard Count " + names.Count);

            // 2. Safety check bounds to make sure structural array sizes align cleanly
            if (names == null || ranks == null || points == null ||
                names.Count != ranks.Count || names.Count != points.Count)
            {
                Debug.LogError("Leaderboard population data collections have mismatched index counts!");
                return;
            }

            // 3. Loop through your retrieved rows and populate rows dynamically
            for (int i = 0; i < names.Count; i++)
            {
                // Instantiate runtime prefab container under target transform parent layout
                GameObject itemInstance = Instantiate(leaderBoardItem, leaderBoardItemParent);

                LeaderBoardItem item = itemInstance.GetComponent<LeaderBoardItem>();
                item.SetLeaderBoardData(names[i], ranks[i], points[i]);

            }
        }

        /// <summary>
        /// Clears the items under the parent container safely by iterating backwards
        /// </summary>
        private void ClearAllText()
        {
            // We loop backwards using a standard integer loop to avoid enumeration mutation crashes
            for (int i = leaderBoardItemParent.childCount - 1; i >= 0; i--)
            {
                Transform child = leaderBoardItemParent.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        public void RefreshBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ClearAllText();
            string apiName = "SinglePlayer";
            // 1. Wipe current list clean

            if (currentId == 1)
            {
                apiName = "MultiPlayer";
            }

            leaderBoard.GetTop10Scores(apiName); // Fetch new data based on the selected tab's API name
        }

        public void BackBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(currentState, false);
        }
    }
}
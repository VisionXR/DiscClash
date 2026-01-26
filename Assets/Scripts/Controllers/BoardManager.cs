using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace com.VisionXR.Controllers
{
    public class BoardManager : MonoBehaviour
    {
        [Header(" Scriptable Objects ")]
        public MyPlayerSettings myPlayerSettings;
        public BoardPropertiesSO boardProperties;
        public UIOutputDataSO uIOutputData;
       

        [Header(" Boards")]
        public GameObject BoardPos;
        private GameObject currentBoard;

        private void OnEnable()
        {
            myPlayerSettings.BoardChangedEvent += CreateNewBoardFromResources;
            uIOutputData.SetMyBoardEvent += CreateNewBoardFromResources;
            uIOutputData.StartTutorialEvent += StartTutorial;
            uIOutputData.StartTrickShotsEvent += StartTutorial; // Assuming StartTrickShots is similar to StartTutorial
            uIOutputData.StartFTUEEvent += StartFTUE;
            uIOutputData.EndTutorialEvent += EndTutorial;
            uIOutputData.StopTrickShotsEvent += EndTutorial;
            CreateNewBoardFromResources(myPlayerSettings.MyBoard);
        }

        private void OnDisable()
        {
            myPlayerSettings.BoardChangedEvent -= CreateNewBoardFromResources;
            uIOutputData.SetMyBoardEvent -= CreateNewBoardFromResources;
            uIOutputData.StartTutorialEvent -= StartTutorial;
            uIOutputData.StartTrickShotsEvent -= StartTutorial;
            uIOutputData.StartFTUEEvent -= StartFTUE;
            uIOutputData.EndTutorialEvent -= EndTutorial;
            uIOutputData.StopTrickShotsEvent -= EndTutorial;
        }

        private void StartFTUE(Destination destination)
        {
            if (currentBoard != null)
            {
                currentBoard.SetActive(false);
            }
        }

        private void StartTutorial()
        {
            if (currentBoard != null)
            {
                currentBoard.SetActive(false);
            }
        }

        private void EndTutorial()
        {
            if (currentBoard != null)
            {
                currentBoard.SetActive(true);
            }
        }

        public void CreateNewBoardFromResources(int i)
        {
            Debug.Log($"Creating new board with index: {i}");

            if (currentBoard != null)
            {
                Destroy(currentBoard);
            }

            // Build the resource path, e.g., "Boards/Board0"
            string resourcePath = $"NewBoards/Board{i}";
            GameObject boardPrefab = Resources.Load<GameObject>(resourcePath);

            if (boardPrefab != null)
            {
                currentBoard = Instantiate(boardPrefab, BoardPos.transform.position, BoardPos.transform.rotation);
            }
            else
            {
                Debug.LogError($"Board prefab not found at Resources/{resourcePath}");
            }
        }
    }
}

using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Controllers
{
    public class BoardManager : MonoBehaviour
    {
        [Header(" Scriptable Objects ")]
        public MyPlayerSettings myPlayerSettings;
        public BoardDataSO boardData;
        public UIOutputDataSO uIOutputData;
        public UIInputDataSO uiInputData;

        [Header(" Boards")]
        public GameObject BoardPos;
        public GameObject currentBoard;

        private void OnEnable()
        {
            myPlayerSettings.BoardChangedEvent += CreateNewBoardFromResources;
            uIOutputData.SetMyBoardEvent += CreateNewBoardFromResources;
            CreateNewBoardFromResources(0);
        }

        private void OnDisable()
        {
            myPlayerSettings.BoardChangedEvent -= CreateNewBoardFromResources;
            uIOutputData.SetMyBoardEvent -= CreateNewBoardFromResources;
          
        }

        public void StartTutorial()
        {
            if (currentBoard != null) currentBoard.SetActive(false);
        }

        public void EndTutorial()
        {
            if (currentBoard != null) currentBoard.SetActive(true);
        }

        public void CreateNewBoardFromResources(int i)
        {
            
            if (currentBoard != null)
            {               
                Destroy(currentBoard);
            }

            // Build the resource path, e.g., "Boards/Board0"
            string resourcePath = $"Boards/Board{i}";
            GameObject boardPrefab = Resources.Load<GameObject>(resourcePath);

            if (boardPrefab != null)
            {
                currentBoard = Instantiate(boardPrefab, BoardPos.transform);
            }
            else
            {
                Debug.LogError($"Board prefab not found at Resources/{resourcePath}");
            }
        }


    }
}

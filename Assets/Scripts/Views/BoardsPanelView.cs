using UnityEngine;
using UnityEngine.UI;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using com.VisionXR.HelperClasses;

namespace com.VisionXR.Views
{
    /// <summary>
    /// Handles the logic for selecting and displaying board images.
    /// </summary>
    public class BoardsPanelView : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public MyPlayerSettings MyPlayerSettings;
        public BoardDataSO boardData;
        public PurchaseDataSO purchaseData;
        public AchievementsDataSO achievementsData;


        [Header("Panel Objects")]
        public List<string> achievementBoardsNames;
        public List<GameObject> achievementBoardLocks;
        public GameObject purchasePanel;
        public GameObject achievementPanel;

        [Header("Board Images")]
        [SerializeField] private List<Image> boardSelectedImages;
        [SerializeField] private List<Image> boardLockImages;

        [Header("Scroll Views")]
        public GameObject defaultScrollView;
        public GameObject premiumScrollView;
        public GameObject achievementScrollView;

        [Header("Selection Images")]
        public Image defaultImage;
        public Image premiumImage;
        public Image achievementImage;


        private void OnEnable()
        {
            InitializeBoards();
            OpenLock();
            purchaseData.BoardAssetPurchasedEvent += OpenLock;
        }

        private void OnDisable()
        {
            purchaseData.BoardAssetPurchasedEvent -= OpenLock;
        }

        /// <summary>
        /// Handles the logic when a board is selected.
        /// Updates the board selection in the player settings.
        /// </summary>
        /// <param name="id">The selected board ID.</param>
        public void OnBoardSelected(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            if (boardLockImages[id].gameObject.activeInHierarchy)
            {
                purchasePanel.SetActive(true);
                purchasePanel.GetComponent<PurchasePanel>().Initialise(1);
            }
            else
            { 
                MyPlayerSettings.SetBoard(id);
                MyPlayerSettings.SaveSettings();
                InitializeBoards();
            }
        }

        public void AchievementBoardSelected(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            if (boardLockImages[id].gameObject.activeInHierarchy)
            {
                achievementPanel.SetActive(true);
            }
            else
            {
                MyPlayerSettings.SetBoard(id);
                MyPlayerSettings.SaveSettings();
                InitializeBoards();
            }
        }



        private void OpenLock()
        {
            foreach (AssetData data in purchaseData.BoardsData)
            {
                if (data.isPurchased)
                {
                    int id = purchaseData.BoardsData.IndexOf(data);


                    // Unlock striker images based on purchased id
                    if (id == 0)
                    {
                        UnlockBoards(0, 4); // Unlock 1,2,3,4 (indices 0-4)
                    }
                    else if (id == 1)
                    {
                        UnlockBoards(5, 9); // Unlock 5,6,7,8,9 (indices 5-9)
                    }
                    else if (id == 2)
                    {
                        UnlockBoards(10, 14); // Unlock 10,11,12,13,14 (indices 10-14)
                    }

                }
            }



            // Unlock achievement boards 
            foreach (string achievementName in achievementBoardsNames)
            {
                if (achievementsData.isAchievementUnlocked(achievementName))
                {
                    int index = achievementBoardsNames.IndexOf(achievementName);
                    if (index < achievementBoardLocks.Count)
                    {
                        achievementBoardLocks[index].SetActive(false);
                    }
                }
            }
        }

        private void UnlockBoards(int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex && i < boardLockImages.Count; i++)
            {
                boardLockImages[i].gameObject.SetActive(false);
            }

        }



        /// <summary>
        /// Initializes the board images by resetting and updating the selected board.
        /// </summary>
        private void InitializeBoards()
        {
            ResetImages();
            if (MyPlayerSettings.MyBoard >= 0 && MyPlayerSettings.MyBoard < boardSelectedImages.Count)
            {
                Image selectedImage = boardSelectedImages[MyPlayerSettings.MyBoard];
                selectedImage.color = Color.white;
                selectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            }
        }

        /// <summary>
        /// Resets the board images to their default state.
        /// </summary>
        private void ResetImages()
        {
            foreach (Image boardImage in boardSelectedImages)
            {
                boardImage.gameObject.GetComponent<UIGradient>().enabled = false;
                boardImage.color = AppProperties.instance.IdleColor;
            }
        }

        public void CloseButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            gameObject.SetActive(false);
        }

        public void DefaultClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetScrolls();
            defaultScrollView.SetActive(true);
            defaultImage.gameObject.GetComponent<UIGradient>().enabled = true;
            defaultImage.color = Color.white;
        }

        public void PremiumClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetScrolls();
            premiumScrollView.SetActive(true);
            premiumImage.gameObject.GetComponent<UIGradient>().enabled = true;
            premiumImage.color = Color.white;
        }

        public void AchievementClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetScrolls();
            achievementScrollView.SetActive(true);
            achievementImage.gameObject.GetComponent<UIGradient>().enabled = true;
            achievementImage.color = Color.white;
        }


        private void ResetScrolls()
        {
            defaultScrollView.SetActive(false);
            premiumScrollView.SetActive(false);
            achievementScrollView.SetActive(false);

            defaultImage.gameObject.GetComponent<UIGradient>().enabled = false;
            premiumImage.gameObject.GetComponent<UIGradient>().enabled = false;
            achievementImage.gameObject.GetComponent<UIGradient>().enabled = false;

            defaultImage.color = AppProperties.instance.IdleColor;
            premiumImage.color = AppProperties.instance.IdleColor;
            achievementImage.color = AppProperties.instance.IdleColor;
        }
    }
}

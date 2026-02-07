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
    public class CoinsPanelView : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public MyPlayerSettings MyPlayerSettings;
        public PurchaseDataSO purchaseData;
        public AchievementsDataSO achievementsData;


        [Header("Panel Objects")]
        public List<string> achievementCoinNames;
        public List<GameObject> achievementCoinLocks;
        public GameObject purchasePanel;
        public GameObject achievementPanel;

        [Header("Coin Images")]
        [SerializeField] private List<Image> coinSelectedImages;
        [SerializeField] private List<Image> coinLockImages;

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
            InitializeCoins();
            OpenLock();
            purchaseData.CoinAssetPurchasedEvent += OpenLock;
        }

        private void OnDisable()
        {
            purchaseData.CoinAssetPurchasedEvent -= OpenLock;
        }

        /// <summary>
        /// Handles the logic when a board is selected.
        /// Updates the board selection in the player settings.
        /// </summary>
        /// <param name="id">The selected board ID.</param>
        public void OnCoinsSelected(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            if (coinLockImages[id].gameObject.activeInHierarchy)
            {
                purchasePanel.SetActive(true);
                purchasePanel.GetComponent<PurchasePanel>().Initialise(2);
            }
            else
            {
                MyPlayerSettings.SetCoins(id);
                MyPlayerSettings.SaveSettings();
                InitializeCoins();
            }
        }

        public void AchievementCoinsSelected(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            if (coinLockImages[id].gameObject.activeInHierarchy)
            {
               achievementPanel.SetActive(true);
            }
            else
            {
                MyPlayerSettings.SetCoins(id);
                MyPlayerSettings.SaveSettings();
                InitializeCoins();
            }
        }



        private void OpenLock()
        {

            foreach (AssetData data in purchaseData.CoinsData)
            {
                if (data.isPurchased)
                {
                    int id = purchaseData.CoinsData.IndexOf(data);


                    // Unlock striker images based on purchased id
                    if (id == 0)
                    {
                        UnlockCoins(0, 4); // Unlock 1,2,3,4 (indices 0-4)
                    }
                    else if (id == 1)
                    {
                        UnlockCoins(5, 9); // Unlock 5,6,7,8,9 (indices 5-9)
                    }
                    else if (id == 2)
                    {
                        UnlockCoins(10, 14); // Unlock 10,11,12,13,14 (indices 10-14)
                    }
                
                }
            }

            foreach (string coinName in achievementCoinNames)
            {
                if (achievementsData.isAchievementUnlocked(coinName))
                {
                    int index = achievementCoinNames.IndexOf(coinName);
                    if (index >= 0 && index < achievementCoinLocks.Count)
                    {
                        achievementCoinLocks[index].SetActive(false);
                    }
                }
            }
        }

        private void UnlockCoins(int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex && i < coinLockImages.Count; i++)
            {
                coinLockImages[i].gameObject.SetActive(false);
            }

        }
        /// <summary>
        /// Initializes the board images by resetting and updating the selected board.
        /// </summary>
        private void InitializeCoins()
        {
            ResetImages();
            if (MyPlayerSettings.MyCoinsId >= 0 && MyPlayerSettings.MyCoinsId < coinSelectedImages.Count)
            {
                Image selectedImage = coinSelectedImages[MyPlayerSettings.MyCoinsId];
                selectedImage.color = Color.white;
                selectedImage.gameObject.GetComponent<UIGradient>().enabled = true;
            }
        }

        /// <summary>
        /// Resets the board images to their default state.
        /// </summary>
        private void ResetImages()
        {
            foreach (Image boardImage in coinSelectedImages)
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

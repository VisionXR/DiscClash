using UnityEngine;
using UnityEngine.UI;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using TMPro;
using com.VisionXR.HelperClasses;
using com.VisionXR.GameElements;

namespace com.VisionXR.Views
{
    /// <summary>
    /// Handles the logic for selecting and displaying striker images.
    /// </summary>
    public class StrikersPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public MyPlayerSettings MyPlayerSettings;
        public PurchaseDataSO purchaseData;
        public AchievementsDataSO achievementsData;
        public StrikerDataSO strikerData;
        public PlayersDataSO playersData;


        [Header("Panel Objects")]
        public List<string> achievementStrikerNames;
        public List<GameObject> achievementStrikerLocks;
        public GameObject purchasePanel;
        public GameObject achievementPanel;

        [Header("Scroll Views")]
        public GameObject defaultScrollView;
        public GameObject premiumScrollView;
        public GameObject achievementScrollView;

        [Header("Panel Images")]
        public Image defaultImage;
        public Image premiumImage;
        public Image achievementImage;

        [Header("Striker Properties")]

        [SerializeField] private List<Image> strikerSelectedImages;
        [SerializeField] private List<Image> strikerLockImages;
        [SerializeField] public List<StrikerProperties> strikerProperties;
        [SerializeField] private List<Slider> powerSliders;
        [SerializeField] private List<Slider> aimSliders;
        [SerializeField] private List<Slider> timeSliders;
        [SerializeField] private List<TMP_Text> strikerNames;

        private void OnEnable()
        {
            InitializeStrikers();
            OpenLock();
            purchaseData.StrikerAssetPurchasedEvent += OpenLock;
        }


        private void OnDisable()
        {
            purchaseData.StrikerAssetPurchasedEvent -= OpenLock;
        }

        /// <summary>
        /// Initializes the striker images by resetting and updating the selected striker.
        /// </summary>
        private void InitializeStrikers()
        {
            ResetImages();
            if (MyPlayerSettings.MyStrikerId >= 0 && MyPlayerSettings.MyStrikerId < strikerSelectedImages.Count)
            {
                Image selectedStriker = strikerSelectedImages[MyPlayerSettings.MyStrikerId];
                selectedStriker.color = Color.white;
                selectedStriker.gameObject.GetComponent<UIGradient>().enabled = true;
            }

            for (int i = 0; i < strikerProperties.Count; i++)
            {
                if (i < strikerNames.Count)
                {
                    strikerNames[i].text = strikerProperties[i].strikerName;
                }
                if (i < powerSliders.Count)
                {
                    powerSliders[i].value = strikerProperties[i].power;
                }
                if (i < aimSliders.Count)
                {
                    aimSliders[i].value = strikerProperties[i].aim;
                }
                if (i < timeSliders.Count)
                {
                    timeSliders[i].value = strikerProperties[i].time;
                }
            }
        }

        private void OpenLock()
        {
            foreach(AssetData data in purchaseData.StrikersData)
            {
                if (data.isPurchased)
                {
                    int id = purchaseData.StrikersData.IndexOf(data);


                    // Unlock striker images based on purchased id
                    if (id == 0)
                    {
                        UnlockStrikers(0, 4); // Unlock 1,2,3,4 (indices 0-4)
                    }
                    else if (id == 1)
                    {
                        UnlockStrikers(5, 9); // Unlock 5,6,7,8,9 (indices 5-9)
                    }
                    else if (id == 2)
                    {
                        UnlockStrikers(10, 14); // Unlock 10,11,12,13,14 (indices 10-14)
                    }
                    else if (id == 3)
                    {
                        UnlockStrikers(15, 19); // Unlock 15,16,17,18,19 (indices 15-19)
                    }
                }
            }


            // unlocking achievement strikers

            foreach (string achievementName in achievementStrikerNames)
            {
                if (achievementsData.isAchievementUnlocked(achievementName))
                {
                    int index = achievementStrikerNames.IndexOf(achievementName);
                    if (index < achievementStrikerLocks.Count)
                    {
                        achievementStrikerLocks[index].SetActive(false);
                    }
                }
            }
        }

        private void UnlockStrikers(int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex && i < strikerLockImages.Count; i++)
            {
                strikerLockImages[i].gameObject.SetActive(false);
            }
 
        }

        /// <summary>
        /// Handles the logic when a striker is selected.
        /// Updates the striker selection in the player settings.
        /// </summary>
        /// <param name="id">The selected striker ID.</param>
        public void OnStrikerSelected(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            if (strikerLockImages[id].gameObject.activeInHierarchy)
            {
                purchasePanel.SetActive(true);
                purchasePanel.GetComponent<PurchasePanel>().Initialise(0);
            }
            else
            {
                MyPlayerSettings.SetStriker(id);
                MyPlayerSettings.SaveSettings();
                InitializeStrikers();
            }      
        }

        public void OnStrikerChanged(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            if (strikerLockImages[id].gameObject.activeInHierarchy)
            {
                purchasePanel.SetActive(true);
                purchasePanel.GetComponent<PurchasePanel>().Initialise(0);
            }
            else
            {
                Debug.Log(" in here ");
                MyPlayerSettings.SetStriker(id);
                MyPlayerSettings.SaveSettings();
                InitializeStrikers();
                Player  p = playersData.GetMainPlayer();
                if (p != null)
                {
                    Debug.Log(" also here ");
                    strikerData.ChangeStriker(p.myId, id);
                }
                
            }
        }

        public void AchievementStrikerSelected(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            if (strikerLockImages[id].gameObject.activeInHierarchy)
            {
                achievementPanel.SetActive(true);
            }
            else
            {
                MyPlayerSettings.SetStriker(id);
                MyPlayerSettings.SaveSettings();
                InitializeStrikers();
            }
        }

        public void AchievementStrikerChanged(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            if (strikerLockImages[id].gameObject.activeInHierarchy)
            {
                achievementPanel.SetActive(true);
            }
            else
            {
                MyPlayerSettings.SetStriker(id);
                MyPlayerSettings.SaveSettings();
                InitializeStrikers();
                Player p = playersData.GetMainPlayer();
                if (p != null)
                {

                    strikerData.ChangeStriker(p.myId, id);
                }

            }
        }

        /// <summary>
        /// Resets the striker images to their default state.
        /// </summary>
        private void ResetImages()
        {
            foreach (Image strikerImage in strikerSelectedImages)
            {
                strikerImage.gameObject.GetComponent<UIGradient>().enabled = false;
                strikerImage.color = AppProperties.instance.IdleColor;
            }
        }

        public void OnCloseButtonClicked()
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

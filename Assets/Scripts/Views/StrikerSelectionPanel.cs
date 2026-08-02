using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class StrikerSelectionPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public UIDataSO uiData;
        public ADDataSO adData;
        public PurchaseDataSO purchaseData;


        [Header("Local Objects")]
        public PanelOnOff internetToastPanel;
        public List<Sprite> strikerSprites;
        public List<GameObject> allButtons;
        public List<GameObject> strikerSelectionImages;
        public List<GameObject> strikerLockImages;
        public List<GameObject> adButtons;

        [Header("Ad Panel ")]
        public PanelOnOff adDetailsPanel;
        public Image strikerImage;
        public GameObject errorText;
        public TMP_Text adNumberText;
        private int adNumberIndex = 0;
        private int currentStrikerIndex = 0;

        [Header("Index ")]
        public int selectionImageIndex = 1;
        public int lockImageIndex = 5;
        public int adButtonIndex= 6 ;

        [Header("States")]
        public string currentState;
        public string purchaseState;


        void Start()
        {
            // Loop through your buttons using a standard for-loop to easily track the index
            for (int i = 0; i < allButtons.Count; i++)
            {
                GameObject buttonObj = allButtons[i];

                // 1. Populate your lists (your existing logic)
                strikerSelectionImages.Add(buttonObj.transform.GetChild(selectionImageIndex).gameObject);
                strikerLockImages.Add(buttonObj.transform.GetChild(lockImageIndex).gameObject);

                GameObject adButtonObj = buttonObj.transform.GetChild(adButtonIndex).gameObject;
                adButtons.Add(adButtonObj);

                // 2. CRITICAL: Capture the current index in a local variable!
                // This creates a unique "copy" for each button's click event.
                int boardIndex = i;

                // 3. Get the Button component and attach the listener
                Button btnComponent = adButtonObj.GetComponent<Button>();
                if (btnComponent != null)
                {
                    // Clear any previous listeners to prevent double-firing if this runs multiple times
                    btnComponent.onClick.RemoveAllListeners();

                    // Register the event, passing the local 'boardIndex' copy
                    btnComponent.onClick.AddListener(() => AdButtonClicked(boardIndex));
                }
                else
                {
                    Debug.LogWarning($"Child 7 on button {i} is missing a Button component!");
                }
            }

            ResetBoardImages();
            if (strikerSelectionImages.Count > uiOutputData.MyStrikerId)
            {
                strikerSelectionImages[uiOutputData.MyBoardId].SetActive(true);
                OpenLock();
            }


        }
        void OnEnable()
        {
            ResetBoardImages();
            if (strikerSelectionImages.Count > uiOutputData.MyBoardId)
            {
                strikerSelectionImages[uiOutputData.MyBoardId].SetActive(true);
                OpenLock();
            }

            adData.OnRewardedAdSuccessEvent += AdWatched;
            adData.OnRewardedAdFailedToLoadEvent += ShowError;
        }

        private void OnDisable()
        {
            adData.OnRewardedAdSuccessEvent -= AdWatched;
            adData.OnRewardedAdFailedToLoadEvent -= ShowError;
        }


        private void OpenLock()
        {
            UnLockStriker(0);
            foreach (AssetData data in purchaseData.AllItemsData)
            {
                if (data.purchaseItemType == PurchaseItemType.Striker && data.isPurchased)
                {
                    foreach (int id in data.itemIds)
                    {
                        UnLockStriker(id);
                    }

                }
            }

        }

        private void UnLockStriker(int strikerIndex)
        {
            if (strikerIndex >= 0 && strikerIndex < strikerLockImages.Count)
            {
                strikerLockImages[strikerIndex].gameObject.SetActive(false);
                adButtons[strikerIndex].gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"Striker index {strikerIndex} is out of range for strikerLockImages or adButtons.");
            }
        }

        public void StrikerBtnClicked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            if (!strikerLockImages[id].gameObject.activeInHierarchy)
            {
                uiOutputData.SetMyStrikerId(id);
                ResetBoardImages();
                strikerSelectionImages[uiOutputData.MyStrikerId].gameObject.SetActive(true);

            }
            else
            {
                uiData.uiManager.GoToState(HelperClasses.StateName.AssetPurchaseState);
            }

        }

        public void BackBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(currentState, false);
        }

        public void AdButtonClicked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                StartCoroutine(CheckInternetAndProceed());
                return;
            }
            Debug.Log($"Ad button clicked for board index: {id}");
            adNumberIndex = 0;
            adDetailsPanel.TurnOnPanel();
            adNumberText.text = $"Ad {adNumberIndex} of {2}";
            currentStrikerIndex = id;

            strikerImage.sprite = strikerSprites[id];
        }


        private IEnumerator CheckInternetAndProceed()
        {
            internetToastPanel.TurnOnPanel();
            yield return new WaitForSeconds(2f);
            internetToastPanel.TurnOffPanel();

        }

        public void ShowAdButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            adData.ShowRewardedAd();

        }

        public void AdWatched()
        {
            adNumberIndex++;
            adNumberText.text = $"Ad {adNumberIndex} of {2}";

            if (adNumberIndex == 2)
            {
                Debug.Log("Second Ad completed, unlocking board");
                adDetailsPanel.TurnOffPanel();

                adButtons[currentStrikerIndex].gameObject.SetActive(false);
                strikerLockImages[currentStrikerIndex].gameObject.SetActive(false);
                StrikerBtnClicked(currentStrikerIndex);
            }
        }

        public void ShowError()
        {
            StartCoroutine(DisplayError());
        }

        private IEnumerator DisplayError()
        {
            errorText.SetActive(true);
            yield return new WaitForSeconds(2);
            errorText.SetActive(false);
        }
        private void ResetBoardImages()
        {
            foreach (var item in strikerSelectionImages)
            {
                item.SetActive(false);
            }

        }

    }
}


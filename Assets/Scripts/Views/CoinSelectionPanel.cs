using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class CoinSelectionPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public UIDataSO uiData;
        public ADDataSO adData;
        public PurchaseDataSO purchaseData;


        [Header("Local Objects")]
        public PanelOnOff internetToastPanel;
        public List<Sprite> coinSprites;
        public List<GameObject> allButtons;
        public List<GameObject> coinSelectionImages;
        public List<GameObject> coinLockImages;
        public List<GameObject> adButtons;

        [Header("Ad Panel ")]
        public PanelOnOff adDetailsPanel;
        public Image coinImage;
        public GameObject errorText;
        public TMP_Text adNumberText;
        private int adNumberIndex = 0;
        private int currentcoinIndex = 0;

        [Header("Index ")]
        public int selectionImageIndex = 1;
        public int lockImageIndex = 5;
        public int adButtonIndex = 6;

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
                coinSelectionImages.Add(buttonObj.transform.GetChild(selectionImageIndex).gameObject);
                coinLockImages.Add(buttonObj.transform.GetChild(lockImageIndex).gameObject);

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
            if (coinSelectionImages.Count > uiOutputData.MyCoinsId)
            {
                coinSelectionImages[uiOutputData.MyCoinsId].SetActive(true);
                OpenLock();
            }


        }
        void OnEnable()
        {
            ResetBoardImages();
            if (coinSelectionImages.Count > uiOutputData.MyCoinsId)
            {
                coinSelectionImages[uiOutputData.MyCoinsId].SetActive(true);
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
            UnLockCoin(0);
            foreach (AssetData data in purchaseData.AllItemsData)
            {
                if (data.purchaseItemType == PurchaseItemType.Coin && data.isPurchased)
                {
                    foreach (int id in data.itemIds)
                    {
                        UnLockCoin(id);
                    }

                }
            }

        }

        private void UnLockCoin(int coinIndex)
        {
            if (coinIndex >= 0 && coinIndex < coinLockImages.Count)
            {
                coinLockImages[coinIndex].gameObject.SetActive(false);
                adButtons[coinIndex].gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"Coin" +
                    $"" +
                    $" index {coinIndex} is out of range for coinLockImages or adButtons.");
            }
        }

        public void CoinBtnClicked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            if (!coinLockImages[id].gameObject.activeInHierarchy)
            {
                uiOutputData.SetMyCoinsId(id);
                ResetBoardImages();
                coinSelectionImages[uiOutputData.MyCoinsId].gameObject.SetActive(true);

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
            currentcoinIndex = id;

            coinImage.sprite = coinSprites[id];
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

                adButtons[currentcoinIndex].gameObject.SetActive(false);
                coinLockImages[currentcoinIndex].gameObject.SetActive(false);
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
            foreach (var item in coinSelectionImages)
            {
                item.SetActive(false);
            }

        }

    }
}


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
            UnlockBoards(0, coinSelectionImages.Count-1);
            //foreach (AssetData data in purchaseData.BoardsData)
            //{
            //    if (data.isPurchased)
            //    {
            //        int id = purchaseData.BoardsData.IndexOf(data);

            //        // Unlock striker images based on purchased id
            //        if (id == 0)
            //        {
            //            UnlockBoards(3, 5); // Unlock 1,2,3,4 (indices 0-4)
            //        }
            //        else if (id == 1)
            //        {
            //            UnlockBoards(6, 8); // Unlock 5,6,7,8,9 (indices 5-9)
            //        }
            //        else if (id == 2)
            //        {
            //            UnlockBoards(9, 11); // Unlock 10,11,12,13,14 (indices 10-14)
            //        }
            //        else if (id == 3)
            //        {
            //            UnlockBoards(12, 14); // Unlock 10,11,12,13,14 (indices 10-14)
            //        }
            //        else if (id == 4)
            //        {
            //            UnlockBoards(15, 17); // Unlock 10,11,12,13,14 (indices 10-14)
            //        }
            //        else if (id == 5)
            //        {
            //            UnlockBoards(18, 20); // Unlock 10,11,12,13,14 (indices 10-14)
            //        }
            //        else if (id == 6)
            //        {
            //            UnlockBoards(0, 20); // Unlock 10,11,12,13,14 (indices 10-14)
            //        }

            //    }
            //}

            //for (int i = 0; i < purchaseData.allSingleBoards.Count; i++)
            //{
            //    if (purchaseData.allSingleBoards[i])
            //    {
            //        boardLockImages[i].gameObject.SetActive(false);
            //        adButtons[i].gameObject.SetActive(false);
            //    }
            //}

        }

        private void UnlockBoards(int startIndex, int endIndex)
        {

            for (int i = startIndex; i <= endIndex; i++)
            {
                coinLockImages[i].gameObject.SetActive(false);
                adButtons[i].gameObject.SetActive(false);
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
                uiData.uiManager.ChangeState(purchaseState, true);
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


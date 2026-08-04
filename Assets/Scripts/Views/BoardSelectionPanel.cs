using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class BoardSelectionPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public UIDataSO uiData;
        public ADDataSO adData;
        public PurchaseDataSO purchaseData;


        [Header("Local Objects")]
        public PanelOnOff internetToastPanel;
        public List<Sprite> boardSprites;
        public List<GameObject> allButtons;
        public List<GameObject> boardSelectionImages;
        public List<GameObject> boardLockImages;
        public List<GameObject> adButtons;

        [Header("Ad Panel ")]
        public PanelOnOff adDetailsPanel;
        public Image boardImage;
        public GameObject errorText;
        public TMP_Text adNumberText;
        private int adNumberIndex = 0;
        private int currentBoardIndex = 0;

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
                boardSelectionImages.Add(buttonObj.transform.GetChild(1).gameObject);
                boardLockImages.Add(buttonObj.transform.GetChild(5).gameObject);

                GameObject adButtonObj = buttonObj.transform.GetChild(6).gameObject;
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
            if (boardSelectionImages.Count > uiOutputData.MyBoardId)
            {
                boardSelectionImages[uiOutputData.MyBoardId].SetActive(true);
                OpenLock();
            }


        }
        void OnEnable()
        {
            ResetBoardImages();
            if (boardSelectionImages.Count > uiOutputData.MyBoardId)
            {
                boardSelectionImages[uiOutputData.MyBoardId].SetActive(true);
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
            UnLockBoard(0);
            foreach (AssetData data in purchaseData.AllItemsData)
            {
                if (data.purchaseItemType == PurchaseItemType.Board && data.isPurchased)
                {
                    foreach (int id in data.itemIds)
                    {
                        UnLockBoard(id);
                    }

                }
            }
        }

        private void UnLockBoard(int boardIndex)
        {
            if (boardIndex >= 0 && boardIndex < boardLockImages.Count)
            {
                boardLockImages[boardIndex].gameObject.SetActive(false);
                adButtons[boardIndex].gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"Board index {boardIndex} is out of range for boardLockImages or adButtons.");
            }
        }

        public void BoardSelected(int id)
        {
            AudioManager.instance.PlayButtonClickSound();

            if (!boardLockImages[id].gameObject.activeInHierarchy)
            {
                uiOutputData.SetMyBoardId(id);
                ResetBoardImages();
                boardSelectionImages[uiOutputData.MyBoardId].gameObject.SetActive(true);

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
            currentBoardIndex = id;

            boardImage.sprite = boardSprites[id];
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
            
                adButtons[currentBoardIndex].gameObject.SetActive(false);
                boardLockImages[currentBoardIndex].gameObject.SetActive(false);

                BoardSelected(currentBoardIndex);
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
            foreach (var item in boardSelectionImages)
            {
                item.SetActive(false);
            }

        }

    }
}


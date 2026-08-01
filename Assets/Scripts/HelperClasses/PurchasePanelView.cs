using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class PurchasePanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public PurchaseDataSO purchaseData;
        public UIDataSO uiData;


        [Header("Panel Objects")]
        public string currentState;
       

        [Header("List Elements")] 
        public List<TMP_Text> boardPriceTexts;

        private void OnEnable()
        {

            purchaseData.BoardAssetPurchasedEvent += SetProductPrices;

           // SetProductPrices();
        }

        private void OnDisable()
        {
            purchaseData.BoardAssetPurchasedEvent -= SetProductPrices;
          
        }

        public void BoardBundleClicked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            string productId = purchaseData.BoardsData[id].productId;
            purchaseData.BuyProduct(productId);
        }

        public void RefreshButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            purchaseData.GetAllItems();
            purchaseData.GetPurchasedItems();
        }


        private void SetProductPrices()
        {

            for (int i = 0; i < purchaseData.BoardsData.Count; i++)
            {
                if (boardPriceTexts[i] != null)
                {
                    if (purchaseData.BoardsData[i].isPurchased)
                    {
                        boardPriceTexts[i].text = "Purchased";
                    }
                    else
                    {
                        boardPriceTexts[i].text = purchaseData.BoardsData[i].Price;
                    }
                }
            }

        }


        public void BackBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            if (uiData.uiManager.previousStateName == StateName.HomeState)
            {
                uiData.uiManager.ChangeState(currentState, false);
            }
            else
            {
                uiData.uiManager.GoToState(uiData.uiManager.previousStateName);
            }
            
        }

        public void RefreshBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            purchaseData.GetPurchasedItems();

        }

    }
}

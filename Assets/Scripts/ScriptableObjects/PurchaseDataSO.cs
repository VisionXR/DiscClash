using com.VisionXR.HelperClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurchaseDataSO", menuName = "ScriptableObjects/PurchaseDataSO", order = 1)]
public class PurchaseDataSO : ScriptableObject
{


    [Header(" Board skus")]
    public List<AssetData> BoardsData;
    public List<bool> allSingleBoards;


    [Header(" Player skus")]
    public string[] allSkusData;


    // Actions
    public Action BoardAssetPurchasedEvent;
    public Action GetPurchasedItemsEvent;
    public Action GetAllItemsEvent;
    public Action RefreshDataEvent;
    public Action<string> BuyProductEvent;


    public Action SetPurchasedItemsEvent;

    // Methods

    private void OnEnable()
    {
        if (allSingleBoards != null)
        {
            for (int i = 0; i < allSingleBoards.Count; i++)
            {
                allSingleBoards[i] = false;
            }
        }
    }

    public void RefreshData()
    {
        RefreshDataEvent?.Invoke();
    }


    public AssetData GetBoardDataById(int id)
    {
        return BoardsData[id];
    }

    public AssetData GetBoardByProductId(string id)
    {
        foreach (var item in BoardsData)
        {
            if (item.productId == id) return item;
        }
        return null;
    }

    public void MarkBoardAsPurchased(string id)
    {
        AssetData board = GetBoardByProductId(id);
        if (board != null)
        {
            board.isPurchased = true;
        }
        BoardAssetPurchasedEvent?.Invoke();
    }


    public void SetPurchasedItems(List<AssetData> productdIds)
    {
        foreach (var id in productdIds)
        {

            foreach (var board in BoardsData)
            {
                if (board.productId == id.productId)
                {
                    board.isPurchased = true;

                }
            }

        }

        SetPurchasedItemsEvent?.Invoke();
    }

    public void SetPriceOfItems(List<AssetData> productdIds)
    {
        foreach (var id in productdIds)
        {

            foreach (var board in BoardsData)
            {
                if (board.productId == id.productId)
                {
                    board.Price = id.Price;

                }
            }

        }
    }
    public void GetPurchasedItems()
    {
        GetPurchasedItemsEvent?.Invoke();
    }

    public void GetAllItems()
    {
        GetAllItemsEvent?.Invoke();
    }

    public void BuyProduct(string productId)
    {
        // This method can be called from your UI when a purchase button is clicked
        // It will trigger the purchase flow in your PurchaseManager
        // You can pass the productId to identify which item to buy
        BuyProductEvent?.Invoke(productId);
    }
}

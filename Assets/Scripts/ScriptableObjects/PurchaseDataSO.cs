using com.VisionXR.HelperClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurchaseDataSO", menuName = "ScriptableObjects/PurchaseDataSO", order = 1)]
public class PurchaseDataSO : ScriptableObject
{

    [Header(" Item skus")]
    public List<AssetData> AllItemsData;


    [Header(" Player skus")]
    public string[] allSkusData;


    // Actions
    public Action AssetPurchasedEvent;
    public Action GetPurchasedItemsEvent;
    public Action GetAllItemsEvent;
    public Action RefreshDataEvent;
    public Action<string> BuyProductEvent;
    public Action SetPurchasedItemsEvent;

    // Methods

    public void RefreshData()
    {
        RefreshDataEvent?.Invoke();
    }

    public AssetData GetAssetDataById(int id)
    {
        return AllItemsData[id];
    }

    public AssetData GetItemByProductId(int id)
    {
       
        return AllItemsData[id];
    }

    public AssetData GetItemByProductId(string id)
    {
        foreach (var item in AllItemsData)
        {
            if (item.productId == id) return item;
        }
        return null;
    }

    public void MarkItemAsPurchased(string id)
    {
        AssetData board = GetItemByProductId(id);
        if (board != null)
        {
            board.isPurchased = true;
        }

        AssetPurchasedEvent?.Invoke();
    }


    public void SetPurchasedItems(List<AssetData> productdIds)
    {
        foreach (var id in productdIds)
        {

            foreach (var item in AllItemsData)
            {
                if (item.productId == id.productId)
                {
                    item.isPurchased = true;

                }
            }

        }

        SetPurchasedItemsEvent?.Invoke();
    }

    public void SetPriceOfItems(List<AssetData> productdIds)
    {
        foreach (var id in productdIds)
        {

            foreach (var item in AllItemsData)
            {
                if (item.productId == id.productId)
                {
                    item.Price = id.Price;
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

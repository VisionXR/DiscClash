using com.VisionXR.HelperClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurchaseDataSO", menuName = "ScriptableObjects/PurchaseDataSO", order = 1)]
public class PurchaseDataSO : ScriptableObject
{
    [Header(" Striker skus")]
    public List<AssetData> StrikersData;

    [Header(" Board skus")]
    public List<AssetData> BoardsData;

    [Header(" Coin skus")]
    public List<AssetData> CoinsData;

    [Header(" Player skus")]
    public string[] allSkusData;


    // Actions
    public Action StrikerAssetPurchasedEvent;
    public Action BoardAssetPurchasedEvent;
    public Action CoinAssetPurchasedEvent;
    public Action GetPurchasedItemsEvent;
    public Action GetAllItemsEvent;
    public Action RefreshDataEvent;

    // Methods

    public void RefreshData()
    {
        RefreshDataEvent?.Invoke();
    }

    public AssetData GetStrikerDataById(int id )
    {
        return StrikersData[id];
    }

    public AssetData GetBoardDataById(int id)
    {
        return BoardsData[id];
    }

    public AssetData GetCoinDataById(int id)
    {
        return CoinsData[id];
    }


    public void MarkStrikerAsPurchased(int id)
    {
        AssetData striker = GetStrikerDataById(id);
        if (striker != null)
        {
            striker.isPurchased = true;
        }
        StrikerAssetPurchasedEvent?.Invoke();
    }

    public void MarkBoardAsPurchased(int id)
    {
        AssetData board = GetBoardDataById(id);
        if (board != null)
        {
            board.isPurchased = true;
        }
        BoardAssetPurchasedEvent?.Invoke();
    }
    public void MarkCoinAsPurchased(int id)
    {
        AssetData coin = GetCoinDataById(id);
        if (coin != null)
        {
            coin.isPurchased = true;
        }
        CoinAssetPurchasedEvent?.Invoke();
    }


    public void InitialisePurchases(List<string> purchasedSkus)
    {
        foreach (var sku in purchasedSkus)
        {
            foreach (var striker in StrikersData)
            {
                if (striker.skuName == sku)
                {
                    striker.isPurchased = true;
                    StrikerAssetPurchasedEvent?.Invoke();
                }
            }
            foreach (var board in BoardsData)
            {
                if (board.skuName == sku)
                {
                    board.isPurchased = true;
                    BoardAssetPurchasedEvent?.Invoke();
                }
            }
            foreach (var coin in CoinsData)
            {
                if (coin.skuName == sku)
                {
                    coin.isPurchased = true;
                    CoinAssetPurchasedEvent?.Invoke();
                }
            }
        }
    }


    public void SetPurchasePrices(List<AssetData> assetDatas)
    
    {
        foreach(var assetData in assetDatas)
        {
            foreach (var striker in StrikersData)
            {
                if (striker.skuName == assetData.skuName)
                {
                    striker.Price = assetData.Price;
                    
                }
            }
            foreach (var board in BoardsData)
            {
                if(board.skuName == assetData.skuName)
                {
                    board.Price = assetData.Price;
                }
            }
            foreach (var coin in CoinsData)
            {
               if(coin.skuName != assetData.skuName)
                {
                    coin.Price = assetData.Price;
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
}

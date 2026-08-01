using com.VisionXR.HelperClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing; // Required for Unity IAP

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    [Header("Scriptable Objects")]
    public PurchaseDataSO purchaseData;

    private IStoreController storeController; // Handles purchases and queries
    private IExtensionProvider storeExtensionProvider; // Accesses platform-specific stores (Google Play)

    private Coroutine allItemsRoutine;
    private Coroutine purchasedItemsRoutine;

    private void Start()
    {
        InitializeUnityIAP();
    }

    private void OnEnable()
    {
        purchaseData.GetPurchasedItemsEvent += GetPurchasedItems;
        purchaseData.GetAllItemsEvent += GetAllItems;
        purchaseData.RefreshDataEvent += RefreshData;
        purchaseData.BuyProductEvent += BuyProduct;
    }

    private void OnDisable()
    {
        purchaseData.GetPurchasedItemsEvent -= GetPurchasedItems;
        purchaseData.GetAllItemsEvent -= GetAllItems;
        purchaseData.RefreshDataEvent -= RefreshData;
        purchaseData.BuyProductEvent -= BuyProduct;

        // Stop routines if the object gets disabled to avoid memory leaks or reference errors
        StopRunningRoutines();
    }

    private void InitializeUnityIAP()
    {
        if (storeController != null || purchaseData.allSkusData == null)
            return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (string sku in purchaseData.allSkusData)
        {
            builder.AddProduct(sku, ProductType.NonConsumable);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    public void RefreshData()
    {
        GetAllItems();
        GetPurchasedItems();
    }

    // 1. Triggers the Coroutine to fetch items that the player has already bought
    public void GetPurchasedItems()
    {
        if (purchasedItemsRoutine != null)
        {
            StopCoroutine(purchasedItemsRoutine);
        }
        purchasedItemsRoutine = StartCoroutine(GetPurchasedItemsRoutine());
    }

    // 2. Triggers the Coroutine to fetch all registered products with localized pricing
    public void GetAllItems()
    {
        Debug.Log("Fetching all items and their prices...");

        if (allItemsRoutine != null)
        {
            StopCoroutine(allItemsRoutine);
        }
        allItemsRoutine = StartCoroutine(GetAllItemsRoutine());
    }

    // 3. Call this method from your UI buttons when a user wants to buy a board
    public void BuyProduct(string productId)
    {
        if (storeController != null)
        {
            Product product = storeController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.LogError("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.LogError("BuyProductID FAIL. Not initialized.");
        }
    }

    private void StopRunningRoutines()
    {
        if (allItemsRoutine != null) StopCoroutine(allItemsRoutine);
        if (purchasedItemsRoutine != null) StopCoroutine(purchasedItemsRoutine);
    }

    #region Coroutines for Data Fetching

    private IEnumerator GetPurchasedItemsRoutine()
    {
        // Wait gracefully if the store controller is still initializing
        if (storeController == null)
        {
           
            yield return new WaitUntil(() => storeController != null);
        }

        List<AssetData> assetDatas = new List<AssetData>();

        // Loop through all products registered in the application
        foreach (var product in storeController.products.all)
        {
            AssetData data = new AssetData();
            data.productId = product.definition.id;

            // If it's owned (non-consumables retain ownership on the device/account)
            if (product.hasReceipt)
            {
                data.isPurchased = true;
                assetDatas.Add(data);

                var item = purchaseData.GetItemByProductId(data.productId);
                if (item != null)
                {
                    Debug.Log("Purchased " + item.skuName);
                }
            }
        }

        // Send the owned SKUs back to your ScriptableObject data holder
        purchaseData.SetPurchasedItems(assetDatas);
        purchasedItemsRoutine = null;
    }

    private IEnumerator GetAllItemsRoutine()
    {
        // Wait gracefully if the store controller is still initializing
        if (storeController == null)
        {
            
            yield return new WaitUntil(() => storeController != null);
        }

        List<AssetData> assetDatas = new List<AssetData>();

        foreach (var product in storeController.products.all)
        {
            AssetData data = new AssetData();
            data.productId = product.definition.id;

            // 1. Grab the raw price number
            decimal rawPrice = product.metadata.localizedPrice;

            // 2. Grab the clean ISO currency code
            string currencyCode = product.metadata.isoCurrencyCode;

            // 3. Format it safely based on the currency
            if (currencyCode == "INR")
            {
                data.Price = "Rs. " + rawPrice.ToString("N0");

            }
            else
            {
                data.Price = rawPrice.ToString("N2") + " " + currencyCode;
            }

            assetDatas.Add(data);
        }

        // Apply all compiled prices back to the scriptable object once the loop finishes
        purchaseData.SetPriceOfItems(assetDatas);
        allItemsRoutine = null;
    }

    #endregion

    #region Unity IAP Interface Implementation (IStoreListener)

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;

        // Populate your UI and data fields once connection is live
        RefreshData();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Unity IAP Initialization Failed: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"Unity IAP Initialization Failed: {error}. Message: {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string prodctId = args.purchasedProduct.definition.id;

        purchaseData.MarkItemAsPurchased(prodctId);

        // Instantly recalculate owned items to grant access to the board
        RefreshData();

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Purchase of {product.definition.id} failed due to: {failureReason}");
    }

    #endregion
}
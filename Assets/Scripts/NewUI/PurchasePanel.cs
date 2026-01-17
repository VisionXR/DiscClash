
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePanel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public PurchaseDataSO purchaseData;

    [Header("Panel Elements")]
    public GameObject strikerPanel;
    public GameObject boardPanel;
    public GameObject coinPanel;
    public GameObject mainPanel;
    public GameObject mainSettingsPanel;

    [Header("List Elements")]
    public List<TMP_Text> strikerPriceTexts;
    public List<TMP_Text> boardPriceTexts;
    public List<TMP_Text> coinPriceTexts;

    [Header("Panel Elements")]
    public Image strikerPanelImage;
    public Image boardPanelImage;
    public Image coinPanelImage;


    private void OnEnable()
    {
        purchaseData.StrikerAssetPurchasedEvent += SetProductPrices;
        purchaseData.BoardAssetPurchasedEvent += SetProductPrices;
        purchaseData.CoinAssetPurchasedEvent += SetProductPrices;
        SetProductPrices();
    }

    private void OnDisable()
    {
        purchaseData.StrikerAssetPurchasedEvent -= SetProductPrices;
        purchaseData.BoardAssetPurchasedEvent -= SetProductPrices;
        purchaseData.CoinAssetPurchasedEvent -= SetProductPrices;
    }



    public void Initialise(int id)
    {
        Reset();
        switch (id)
        {
            case 0:
                strikerPanel.SetActive(true);
                strikerPanelImage.gameObject.GetComponent<UIGradient>().enabled = true;
                strikerPanelImage.color = Color.white;
                break;
            case 1:
                boardPanel.SetActive(true);
                boardPanelImage.gameObject.GetComponent<UIGradient>().enabled = true;
                boardPanelImage.color = Color.white;
                break;
            case 2:
                coinPanel.SetActive(true);
                coinPanelImage.gameObject.GetComponent<UIGradient>().enabled = true;
                coinPanelImage.color = Color.white;
                break;
            default:
                Debug.LogError("Invalid panel ID: " + id);
                break;
        }
    }


    public void BackBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        gameObject.SetActive(false);

    }

    public void CrossButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        gameObject.SetActive(false);
    }

    public void StrikerButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        Reset();
        strikerPanel.SetActive(true);
        strikerPanelImage.gameObject.GetComponent<UIGradient>().enabled = true;
        strikerPanelImage.color = Color.white;
    }

    public void BoardButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        Reset();
        boardPanel.SetActive(true);
        boardPanelImage.gameObject.GetComponent<UIGradient>().enabled = true;
        boardPanelImage.color = Color.white;
    }

    public void CoinButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        Reset();
        coinPanel.SetActive(true);
        coinPanelImage.gameObject.GetComponent<UIGradient>().enabled = true;
        coinPanelImage.color = Color.white;
    }



    public void StrikerBundleClicked(int id)
    {
        AudioManager.instance.PlayButtonClickSound();
        string sku = purchaseData.StrikersData[id].skuName;
      
    }

    public void BoardBundleClicked(int id)
    {
        AudioManager.instance.PlayButtonClickSound();
        string sku = purchaseData.BoardsData[id].skuName;
      
    }

    public void CoinBundleClicked(int id)
    {
        AudioManager.instance.PlayButtonClickSound();
        string sku = purchaseData.CoinsData[id].skuName;
      

    }


    public void RefreshButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        purchaseData.GetAllItems();
        purchaseData.GetPurchasedItems();
    }


    private void SetProductPrices()
    {
        for (int i = 0; i < purchaseData.StrikersData.Count; i++)
        {
            if (strikerPriceTexts[i] != null)
            {
                if(purchaseData.StrikersData[i].isPurchased)
                {
                    strikerPriceTexts[i].text = "Purchased";
                }
                else
                {
                    strikerPriceTexts[i].text = purchaseData.StrikersData[i].Price;
                }
            }
        }
        for (int i = 0; i < purchaseData.BoardsData.Count; i++)
        {
            if (boardPriceTexts[i] != null)
            {
                if(purchaseData.BoardsData[i].isPurchased)
                {
                    boardPriceTexts[i].text = "Purchased";
                }
                else
                {
                    boardPriceTexts[i].text = purchaseData.BoardsData[i].Price;
                }
            }
        }
        for (int i = 0; i < purchaseData.CoinsData.Count; i++)
        {
            if (coinPriceTexts[i] != null)
            {
                if(purchaseData.CoinsData[i].isPurchased)
                {
                    coinPriceTexts[i].text = "Purchased";
                }
                else
                {
                    coinPriceTexts[i].text = purchaseData.CoinsData[i].Price;
                }
            }
        }
    }


    private void Reset()
    {
        strikerPanel.SetActive(false);
        boardPanel.SetActive(false);
        coinPanel.SetActive(false);

        strikerPanelImage.gameObject.GetComponent<UIGradient>().enabled = false;
        strikerPanelImage.color = AppProperties.instance.IdleColor;

        boardPanelImage.gameObject.GetComponent<UIGradient>().enabled = false;
        boardPanelImage.color = AppProperties.instance.IdleColor;

        coinPanelImage.gameObject.GetComponent<UIGradient>().enabled = false;
        coinPanelImage.color = AppProperties.instance.IdleColor;

    }

}

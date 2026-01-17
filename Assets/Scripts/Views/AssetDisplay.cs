using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetDisplay : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public MyPlayerSettings playerSettings;

    [Header(" Panel Objects")]
    public GameObject BoardsPanel;
    public GameObject StrikersPanel;
    public GameObject CoinsPanel; // Added CoinsPanel

    [Header(" Selection Images")]
    public Image BoardDisplayImage;
    public Image StrikerDisplayImage;
    public Image CoinDisplayImage; // Added CoinDisplayImage

    [Header(" Actual Images")]
    public List<Sprite> boardImages;
    public List<Sprite> strikerImages;
    public List<Sprite> coinImages; // Added coinImages


    private void OnEnable()
    {
        playerSettings.BoardChangedEvent += BoardChanged;
        playerSettings.StrikerChangedEvent += StrikerChanged;
        playerSettings.CoinsChangedEvent += CoinsChanged; // Subscribe to CoinsChanged
        Intialise();
    }

    private void OnDisable()
    {
        playerSettings.BoardChangedEvent -= BoardChanged;
        playerSettings.StrikerChangedEvent -= StrikerChanged;
        playerSettings.CoinsChangedEvent -= CoinsChanged; // Unsubscribe from CoinsChanged
    }


    private void Intialise()
    {
      
        BoardDisplayImage.sprite = boardImages[playerSettings.MyBoard];
        StrikerDisplayImage.sprite = strikerImages[playerSettings.MyStrikerId];
        CoinDisplayImage.sprite = coinImages[playerSettings.MyCoinsId]; // Set coin image
    }

    private void StrikerChanged(int obj)
    {
        Intialise();
    }

    private void BoardChanged(int id)
    {
        Intialise();
    }

    private void CoinsChanged(int id)
    {
        Intialise();
    }

    public void BoardButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        BoardsPanel.SetActive(true);
    }

    public void StrikerButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        StrikersPanel.SetActive(true);
    }

    public void CoinsButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        CoinsPanel.SetActive(true);
    }
}

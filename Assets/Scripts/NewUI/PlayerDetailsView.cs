using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetailsView : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIOutputDataSO uiOutputData;

    [Header("Text Objects")]          
    public TMP_Text  PlayerNameText;          
    public TMP_Text CoinScore;  


    [Header("Image Objects")]
    public Image CoinImage;
    public Image PlayerImage;
    


    private void OnDisable()
    {
        
        PlayerNameText.text = "";
        CoinScore.text = "0";
        PlayerImage.sprite = null;
        CoinImage.sprite = null;
      
    }


    public void SetGameName(string gameName)
    {
       //  GameTitleText.text = gameName; 
    }

    public void SetPlayerName(string playerName)
    {
        PlayerNameText.text = playerName;       
    }

    public void SetCoinImage(Sprite coin)
    {
        CoinImage.sprite = coin;
    }

    public void SetRedImage(Sprite red)
    {
       // RedImage.sprite = red;
    }


    public void SetPlayerImage(Sprite image)
    {
        if (image != null)
        {
            PlayerImage.sprite = image;
        }
    }

    public void SetScore(int totalScore)
    { 
        CoinScore.text = totalScore.ToString();
        
    }



}

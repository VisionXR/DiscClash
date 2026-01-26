using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetailsView : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIOutputDataSO uiOutputData;

    [Header("Text Objects")]
    public TMP_Text  GameTitleText;             
    public TMP_Text  PlayerNameText;          
    public TMP_Text CoinScore;  
    public TMP_Text RedScore;       
    public TMP_Text TotalScore;

    [Header("Image Objects")]
    public Image CoinImage;
    public Image RedImage;
    public Image PlayerImage;
    public Image PlayerTimerImage;
    public Image PlayerTurnImage;

    [Header("Other Objects")]
    public ImageScroller PlayerImageScroller;
    public Button playerReadyButton;
    public Animator buttonAnimator;
    public TMP_Text playerStatus;


    private void OnDisable()
    {
        
        PlayerNameText.text = "";
        CoinScore.text = "0";
        RedScore.text = "0";
        TotalScore.text = "0";
        PlayerImage.sprite = null;
        CoinImage.sprite = null;
        RedImage.sprite = null;
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
        RedImage.sprite = red;
    }

    public void SetTurnImage(Color c)
    {
        PlayerTurnImage.color = c;
    }
    public void SetPlayerImage(Sprite image)
    {
        if (image != null)
        {
            PlayerImage.sprite = image;
        }
    }

    public void SetScore(int coinScore, int redScore, int totalScore)
    { 
        CoinScore.text = coinScore.ToString();
        RedScore.text = redScore.ToString();
        TotalScore.text = totalScore.ToString();    
    }


    public void SetTimer(float a)
    {
        PlayerTimerImage.fillAmount = 1-a;
    }

    public void ResetTimer()
    {
        PlayerTimerImage.fillAmount = 1;
    }

}

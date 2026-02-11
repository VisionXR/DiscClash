using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamDetailsView : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIOutputDataSO uiOutputData;

    [Header("Team Objects")]
  
    public TMP_Text CoinScore;
    public Image CoinImage;



    [Header("Player Objects")]             
    public TMP_Text  Player1NameText;          
    public TMP_Text  Player2NameText;          
  

    [Header("Image Objects")]
    public Image Player1Image;
    public Image Player2Image;
    public Image Player1TimerImage;
    public Image Player2TimerImage;




    private void OnDisable()
    {
       
        Player1NameText.text = "";
        Player2NameText.text = "";
        CoinScore.text = "0";
        Player1Image.sprite = null;
        Player2Image.sprite = null;
        CoinImage.sprite = null;
    }


    public void SetGameName(string gameName)
    {
        
    }

    public void SetPlayerName(int id,string playerName)
    {
        if (id == 1 || id == 3)
        {
            Player1NameText.text = playerName;
        }
        else
        {
            Player2NameText.text = playerName;
        }
    }

    public void SetCoinImage(Sprite coin)
    {
        CoinImage.sprite = coin;
    }

    public void SetRedImage(Sprite red)
    {
        
    }

    public void SetPlayerImage(int id,Sprite image)
    {
        if (id == 1 || id == 3)
        {
            if (image != null)
            {
                Player1Image.sprite = image;
            }
        }
        else
        {
            if (image != null)
            {
                Player2Image.sprite = image;
            }
        }
    }

    public void SetTurnImage(int id,Color color)
    {
        if (id == 1 || id == 3)
        {
            Player1TimerImage.color = color;     
        }
        else
        {
            Player2TimerImage.color = color;     
        }       

    }

    public void SetScore(int coinScore, int redScore, int totalScore)
    { 
        CoinScore.text = coinScore.ToString();
      
    }

    public void SetTimer(int id,float a)
    {
        if (id == 1 || id == 3)
        {
            Player1TimerImage.fillAmount = 1-a;
        }
        else
        {
            Player2TimerImage.fillAmount = 1-a;
        }
    }

    public void ResetTimer(int id)
    {
        if (id == 1 || id == 3)
        {
            Player1TimerImage.fillAmount = 1;
        }
        else
        {
            Player2TimerImage.fillAmount = 1;
        }
    }

    public void SetButton(int id)
    {
      
    }

}
    
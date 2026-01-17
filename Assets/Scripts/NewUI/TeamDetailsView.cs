using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamDetailsView : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIOutputDataSO uiOutputData;

    [Header("Team Objects")]
    public TMP_Text GameTitleText;
    public TMP_Text CoinScore;
    public TMP_Text RedScore;
    public TMP_Text TotalScore;
    public Image CoinImage;
    public Image RedImage;


    [Header("Player Objects")]             
    public TMP_Text  Player1NameText;          
    public TMP_Text  Player2NameText;          
  

    [Header("Image Objects")]
    public Image Player1Image;
    public Image Player2Image;
    public Image Player1TimerImage;
    public Image Player2TimerImage;
    public Image Player1TurnImage;
    public Image Player2TurnImage;

    [Header("Other Objects")]
    public ImageScroller Player1ImageScroller;
    public ImageScroller Player2ImageScroller;
    public Button player1ReadyButton;
    public Button player2ReadyButton;

    public Animator button1Animator;
    public TMP_Text player1Status;
    public Animator button2Animator;
    public TMP_Text player2Status;


    private void OnDisable()
    {
        GameTitleText.text = "";
        Player1NameText.text = "";
        Player2NameText.text = "";
        CoinScore.text = "0";
        RedScore.text = "0";
        TotalScore.text = "0";
        Player1Image.sprite = null;
        Player2Image.sprite = null;
        CoinImage.sprite = null;
        RedImage.sprite = null;
    }


    public void SetGameName(string gameName)
    {
         GameTitleText.text = gameName; 
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
        RedImage.sprite = red;
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
            Player1TurnImage.color = color;     
        }
        else
        {
            Player2TurnImage.color = color;     
        }       

    }

    public void SetScore(int coinScore, int redScore, int totalScore)
    { 
        CoinScore.text = coinScore.ToString();
        RedScore.text = redScore.ToString();
        TotalScore.text = totalScore.ToString();    
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
        if (id == 1 || id == 3)
        {
            player1ReadyButton.interactable = true;
            button1Animator.SetBool("Highlight", true);
        }
        else
        {
            player2ReadyButton.interactable = true;
            button2Animator.SetBool("Highlight", true);
        }
    }

    public void ResetButton(int id)
    {
        if (id == 1 || id == 3)
        {
            player1ReadyButton.interactable = false;
            button1Animator.SetBool("Highlight", false);
        }
        else
        {
            player2ReadyButton.interactable = false;
            button2Animator.SetBool("Highlight", false);
        }
    }

    public void ReadyBtnClicked(int id)
    {
        Debug.Log(" Ready clicked " + id);
        AudioManager.instance.PlayButtonClickSound();
        ResetButton(id);
        SetStatus(id,"Ready");
        uiOutputData.PlayerReady(id);

    }

    public void SetStatus(int id,string status)
    {
        if (id == 1 || id == 3)
        {
            player1Status.text = status;
        }
        else
        {
            player2Status.text = status;
        }
    }


}
    
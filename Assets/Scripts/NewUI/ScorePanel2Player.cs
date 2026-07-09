using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;
using UnityEngine;
using com.VisionXR.GameElements;
using com.VisionXR.Views;
using System.Collections;
using UnityEngine.UI;

public class ScorePanel2Player : MonoBehaviour
{
    [Header(" scriptable objects")]
    public UIOutputDataSO uiOutputData;
    public UIInputDataSO uiInputData;
    public GameDataSO gameData;
    public PlayersDataSO playerData;
    public UIDataSO uiData;
    public CamPositionSO camPositionData;

    [Header(" player objects")]
    public PlayerDetailsView leftPlayer;
    public PlayerDetailsView rightPlayer;
    public GameResultPanelView gameResultPanelView;
    public string gameResultState;
    public string pauseState;


    [Header(" UI Elements")]
    public Image camViewImage;  
    public Sprite FrontViewSprite;
    public Sprite TopViewSprite;

    // local variables
     private Coroutine turnIndicatorCoroutine;

    private void OnEnable()
    {
              
        gameData.TurnChangedEvent += TurnChanged;

        uiInputData.ShowGameResultEvent += ShowGameResult;

        uiInputData.PlayAgainEvent += Reset;
       

        uiInputData.ShowPlayerDetailsEvent += ShowPlayerDetails;


        playerData.PlayerStrikeStartedEvent += PlayerStrikeStarted;
        playerData.PlayerImageLoadedEvent += ShowImages;


      
    }

    private void OnDisable()
    {
       
        gameData.TurnChangedEvent -= TurnChanged;

        uiInputData.ShowGameResultEvent -= ShowGameResult;

        uiInputData.PlayAgainEvent -= Reset;
       

        uiInputData.ShowPlayerDetailsEvent -= ShowPlayerDetails;    

        playerData.PlayerStrikeStartedEvent -= PlayerStrikeStarted;
        playerData.PlayerImageLoadedEvent -= ShowImages;

        

    }

  

    public void ShowImages()
    {
        Player p1 = playerData.GetPlayer(1);
        Player p2 = playerData.GetPlayer(2);

        if (p1 != null)
        {
            leftPlayer.SetPlayerImage(p1.GetMyImage());
            leftPlayer.SetPlayerName(p1.myName);
        }

        if (p2 != null)
        {
            rightPlayer.SetPlayerImage(p2.GetMyImage());
            rightPlayer.SetPlayerName(p2.myName);
        }
    }

    private void PlayerStrikeStarted(int id, float arg2)
    {
        
    }
    private void ShowGameResult(GameResult result)
    {
        
        TurnChanged(result.winningPlayerId);
        uiData.uiManager.ShowCanvas(0);
        uiData.uiManager.ChangeState(gameResultState, true);
        gameResultPanelView.ShowResult(result);
        gameObject.SetActive(false);
    }

    public void ShowPlayerDetails(Player p)
    {
        if (p.myId == 1)
        {         
            leftPlayer.SetPlayerName(p.myName);
            leftPlayer.SetPlayerImage(p.GetMyImage());         
        }
        else {

            
            rightPlayer.SetPlayerName(p.myName);
            rightPlayer.SetPlayerImage(p.GetMyImage());
                 
        }

        SetCoins(p.myId);
        Reset();

    }
    private void TurnChanged(int id)
    {
        if(turnIndicatorCoroutine != null)
        {
            StopCoroutine(turnIndicatorCoroutine);
            turnIndicatorCoroutine = null;
        }
        
        turnIndicatorCoroutine = StartCoroutine(TurnIndicator(id));
    }


    private IEnumerator TurnIndicator(int id)
    {
        while (true)
        {
            if (id == 1)
            {
                leftPlayer.SetPlayerTurnIndicator(true);
                yield return new WaitForSeconds(0.5f);
                leftPlayer.SetPlayerTurnIndicator(false);
            }
            else
            {
                rightPlayer.SetPlayerTurnIndicator(true);
                yield return new WaitForSeconds(0.5f);
                rightPlayer.SetPlayerTurnIndicator(false);
            }


        }
    }

    public void PauseButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        uiData.uiManager.ChangeState(pauseState, true);
    }

    public void CameraBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        Player p = playerData.GetMainPlayer();
        if(camViewImage.sprite == FrontViewSprite)
        {
            camViewImage.sprite = TopViewSprite;
            camPositionData.SetCamPositionTopView(p.myId);
        }
        else
        {
            camViewImage.sprite = FrontViewSprite;
            camPositionData.SetCamPositionFrontView(p.myId);
        }
    }


    public void SetCoins(int id)
    {
       Player p = playerData.GetPlayer(id);
        if (p == null)
        {
            Debug.Log("No player found with id: " + id);
            return;
        }

        if (id == 1)
        {
            if (uiOutputData.challenge == Challenge.BlackAndWhite)
            {
                if (p.myCoin == PlayerCoin.White)
                {
                    leftPlayer.SetCoinImage(uiOutputData.WhiteCoin);
                    leftPlayer.SetRedImage(uiOutputData.RedCoin);
                }
                else
                {
                    leftPlayer.SetCoinImage(uiOutputData.BlackCoin);
                    leftPlayer.SetRedImage(uiOutputData.RedCoin);
                }
            }
            else if (uiOutputData.challenge == Challenge.FreeStyle)
            {
                leftPlayer.SetCoinImage(uiOutputData.BlackAndWhiteCoin);
                leftPlayer.SetRedImage(uiOutputData.RedCoin);
            }
        }
        else
        {
            if (uiOutputData.challenge == Challenge.BlackAndWhite)
            {
                if (p.myCoin == PlayerCoin.White)
                {
                    rightPlayer.SetCoinImage(uiOutputData.WhiteCoin);
                    rightPlayer.SetRedImage(uiOutputData.RedCoin);
                }
                else
                {
                    rightPlayer.SetCoinImage(uiOutputData.BlackCoin);
                    rightPlayer.SetRedImage(uiOutputData.RedCoin);
                }
            }
            else if (uiOutputData.challenge == Challenge.FreeStyle)
            {
                rightPlayer.SetCoinImage(uiOutputData.BlackAndWhiteCoin);
                rightPlayer.SetRedImage(uiOutputData.RedCoin);
            }
        }
      
    }

    private void ResetImages()
    {
        leftPlayer.SetPlayerImage(null);
        rightPlayer.SetPlayerImage(null);
    }
    private void Reset()
    {    
        leftPlayer.SetScore(0);
        rightPlayer.SetScore(0);
    }
}


                                                                
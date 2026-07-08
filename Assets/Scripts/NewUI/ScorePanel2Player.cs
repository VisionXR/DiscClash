using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;
using System;
using UnityEngine;
using com.VisionXR.GameElements;
using com.VisionXR.Views;
using System.Collections;

public class ScorePanel2Player : MonoBehaviour
{
    [Header(" scriptable objects")]
    public UIOutputDataSO uiOutputData;
    public UIInputDataSO uiInputData;
    public GameDataSO gameData;
    public PlayersDataSO playerData;


    [Header(" player objects")]
    public PlayerDetailsView leftPlayer;
    public PlayerDetailsView rightPlayer;
    


    private Coroutine turnTimeRoutine = null;

    private void OnEnable()
    {
              
        gameData.TurnChangedEvent += TurnChanged;

        uiInputData.ShowGameResultEvent += ShowGameResult;

        uiInputData.PlayAgainEvent += Reset;
        uiOutputData.CoinsSetEvent += SetCoins;

        uiInputData.ShowPlayerDetailsEvent += ShowPlayerDetails;


        playerData.PlayerStrikeStartedEvent += PlayerStrikeStarted;
        playerData.PlayerImageLoadedEvent += ShowImages;


        Reset();
    }

    private void OnDisable()
    {
       
        gameData.TurnChangedEvent -= TurnChanged;

        uiInputData.ShowGameResultEvent -= ShowGameResult;

        uiInputData.PlayAgainEvent -= Reset;
        uiOutputData.CoinsSetEvent -= SetCoins;

        uiInputData.ShowPlayerDetailsEvent -= ShowPlayerDetails;    

        playerData.PlayerStrikeStartedEvent -= PlayerStrikeStarted;
        playerData.PlayerImageLoadedEvent -= ShowImages;

        
        StopTurnTime();
        ResetImages();
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
        StopTurnTime();
    }
    private void ShowGameResult(GameResult result)
    {
        StopTurnTime();
        TurnChanged(result.winningPlayerId);    
      
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

        SetCoins();


    }
    private void TurnChanged(int id)
    {

        AudioManager.instance.StopClockSound();

        StartTurnTime(id);

    }

    private void StartTurnTime(int id)
    {
        if (uiOutputData.gameType == GameType.OnlineMultiPlayer || uiOutputData.gameType == GameType.PlayWithFriends)
        {
            if (turnTimeRoutine == null)
            {
                turnTimeRoutine = StartCoroutine(StartTurnTimeRoutine(id));
            }
        }
    }

    public void StopTurnTime()
    {
        if (turnTimeRoutine != null)
        {
            StopCoroutine(turnTimeRoutine);
            turnTimeRoutine = null;
        }
    
    }


    // write a coroutine called startTurnTime where in 45 seconds it goes from 0 to 1
    public IEnumerator StartTurnTimeRoutine(int id)
    {
        

        for (int i = 0; i <= 45; i++)
        {
            yield return new WaitForSeconds(1);
          
        }

        AudioManager.instance.StopClockSound();
        turnTimeRoutine = null;

    }



    public void SetCoins()
    {
        SetCoins(1);
        SetCoins(2);
    }

    public void SetCoins(int id)
    {
        Player p = playerData.GetPlayer(id);
        if(p == null)
        {
            return;
        }

        if(id == 1)
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
        StopTurnTime();
     
        leftPlayer.SetScore(0);
        rightPlayer.SetScore(0);

    }
}


                                                                
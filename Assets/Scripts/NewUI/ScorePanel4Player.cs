using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;
using System;
using UnityEngine;
using com.VisionXR.GameElements;
using com.VisionXR.Views;
using System.Collections;

public class ScorePanel4Player : MonoBehaviour
{
    [Header(" scriptable objects")]
    public UIOutputDataSO uiOutputData;
    public UIInputDataSO uiInputData;
    public GameDataSO gameData;
    public PlayersDataSO playerData;


    [Header(" player objects")]
    public TeamDetailsView teamA;
    public TeamDetailsView teamB;


    private Coroutine turnTimeRoutine = null;


    private void OnEnable()
    {
          
        gameData.TurnChangedEvent += TurnChanged;
        uiOutputData.CoinsSetEvent += SetCoins;


        uiInputData.ShowGameResultEvent += ShowGameResult;
        uiInputData.PlayAgainEvent += Reset;
        uiInputData.ShowPlayerDetailsEvent += ShowPlayerDetails;

        playerData.PlayerStrikeStartedEvent += PlayerStrikeStarted;
        playerData.PlayerImageLoadedEvent += ShowImages;


        Reset();
    }

    private void OnDisable()
    {
        
        gameData.TurnChangedEvent -= TurnChanged;
        uiOutputData.CoinsSetEvent -= SetCoins;

        uiInputData.ShowGameResultEvent -= ShowGameResult;
        uiInputData.PlayAgainEvent -= Reset;
        uiInputData.ShowPlayerDetailsEvent -= ShowPlayerDetails;

        playerData.PlayerStrikeStartedEvent -= PlayerStrikeStarted;

        playerData.PlayerImageLoadedEvent -= ShowImages;


        StopTurnTime();
        ResetImages();
    }

    private void ResetImages()
    {
        teamA.SetPlayerImage(1,null);
        teamA.SetPlayerImage(2, null);
        teamB.SetPlayerImage(3, null);
        teamB.SetPlayerImage(4, null);
    }

    private void ShowImages()
    {
        Player p1 = playerData.GetPlayer(1);
        Player p2 = playerData.GetPlayer(2);

        if (p1 != null)
        {
            teamA.SetPlayerImage(p1.myId, p1.GetMyImage());
        }

        if (p2 != null)
        {
            teamA.SetPlayerImage(p2.myId, p2.GetMyImage());
        }


        Player p3 = playerData.GetPlayer(3);
        Player p4 = playerData.GetPlayer(4);

        if (p3 != null)
        {
            teamB.SetPlayerImage(p3.myId, p3.GetMyImage());
        }

        if (p4!= null)          
        {
            teamB.SetPlayerImage(p4.myId, p4.GetMyImage());
        }
       

    }

    private void PlayerStrikeStarted(int id, float arg2)
    {
        StopTurnTime();
    }
    private void Reset()
    {
        StopTurnTime();
        ResetIndicators();
        teamA.SetScore(0, 0, 0);
        teamB.SetScore(0, 0, 0);

    }


    private void ShowGameResult(GameResult result)
    {
        StopTurnTime();
        TurnChanged(result.winningPlayerId);
      
    }

    public void ShowPlayerDetails(Player p)
    {
        if (p.myId == 1  || p.myId == 2)
        {
          
            teamA.SetPlayerName(p.myId,p.myName);
            teamA.SetPlayerImage(p.myId,p.GetMyImage());

        }
        else {

        
            teamB.SetPlayerName(p.myId,p.myName);
            teamB.SetPlayerImage(p.myId,p.GetMyImage());
          
        }

        SetCoins();

    }
    private void TurnChanged(int id)
    {
        AudioManager.instance.StopClockSound();


    }


    // write a coroutine called startTurnTime where in 45 seconds it goes from 0 to 1
    public IEnumerator StartTurnTime(int id)
    {
        teamA.SetTimer(1, 0);
        teamA.SetTimer(2, 0);
        teamB.SetTimer(3, 0);
        teamB.SetTimer(4, 0);

        for (int i = 0; i <= 45; i++)
        {
            yield return new WaitForSeconds(1);
            if (id == 1 || id==2)
            {
                teamA.SetTimer(id,i / (45.0f));
            }
            else
            {
                teamB.SetTimer(id, i / (45.0f));
            }

            if(i == 39)
            {
                AudioManager.instance.PlayClockSound();
            }
        }

        AudioManager.instance.StopClockSound();
        turnTimeRoutine = null;

    }

    public void StopTurnTime()
    {
        if (turnTimeRoutine != null)
        {
            StopCoroutine(turnTimeRoutine);
            turnTimeRoutine = null;
        }
        ResetIndicators();
    }

    private void ResetIndicators()
    {
        teamA.SetTurnImage(1, Color.white);
        teamA.SetTurnImage(2, Color.white);
        teamB.SetTurnImage(3, Color.white);
        teamB.SetTurnImage(4, Color.white);

        teamA.ResetTimer(1);
        teamA.ResetTimer(2);
        teamB.ResetTimer(3);
        teamB.ResetTimer(4);
    }

    private void SetTurnIndicator(int id)
    {
        if(id ==1 || id ==2 )
        {
            teamA.SetTurnImage(id, Color.green);
        }
        else {
            teamB.SetTurnImage(id, Color.green);    
        }

    }

    public void SetButton(int id)
    {
        if (id == 1 || id == 2)
        {
            teamA.SetButton(id);
        }
        else
        {
            teamB.SetButton(id);
        }
    }


    public void SetCoins()
    {
        SetCoins(1);
        SetCoins(2);
        SetCoins(3);
        SetCoins(4);
    }

    public void SetCoins(int id)
    {
        Player p = playerData.GetPlayer(id);
        if (p == null)
        {
            return;
        }

        if (p.myId == 1 || p.myId == 2)
        {
            if (uiOutputData.challenge == Challenge.BlackAndWhite)
            {
                if (p.myCoin == PlayerCoin.White)
                {
                    teamA.SetCoinImage(uiOutputData.WhiteCoin);
                    teamA.SetRedImage(uiOutputData.RedCoin);
                }
                else
                {
                    teamA.SetCoinImage(uiOutputData.BlackCoin);
                    teamA.SetRedImage(uiOutputData.RedCoin);
                }
            }
            else if (   uiOutputData.challenge == Challenge.FreeStyle)
            {
                teamA.SetCoinImage(uiOutputData.BlackAndWhiteCoin);
                teamA.SetRedImage(uiOutputData.RedCoin);
            }
        }
        else
        {
            if (uiOutputData.challenge == Challenge.BlackAndWhite)
            {
                if (p.myCoin == PlayerCoin.White)
                {
                    teamB.SetCoinImage(uiOutputData.WhiteCoin);
                    teamB.SetRedImage(uiOutputData.RedCoin);
                }
                else
                {
                    teamB.SetCoinImage(uiOutputData.BlackCoin);
                    teamB.SetRedImage(uiOutputData.RedCoin);
                }
            }
            else if (uiOutputData.challenge == Challenge.FreeStyle)
            {
                teamB.SetCoinImage(uiOutputData.BlackAndWhiteCoin);
                teamB.SetRedImage(uiOutputData.RedCoin);
            }
        }
    }
}


                                                                
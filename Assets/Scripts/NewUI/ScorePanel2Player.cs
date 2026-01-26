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
    public GameObject InviteObject;
    public PlayerDetailsView leftPlayer;
    public PlayerDetailsView rightPlayer;
    

    [Header(" canvas objects")]
    public GameObject CenterCanvas;
    public GameObject OtherPlayerDisconnectionPanel;
    public GameResultPanelView gameResultView;



    private Coroutine turnTimeRoutine = null;

    private void OnEnable()
    {
        leftPlayer.gameObject.SetActive(true);
        rightPlayer.gameObject.SetActive(true);

        leftPlayer.PlayerImageScroller.StartScrolling();
        rightPlayer.PlayerImageScroller.StartScrolling();


        gameData.TurnChangedEvent += TurnChanged;
      
        uiOutputData.ShowGameResultEvent += ShowGameResult;
        uiOutputData.HomeEvent += TurnOff;
        uiOutputData.ExitGameEvent += TurnOff;
        uiOutputData.PlayAgainEvent += Reset;
        uiOutputData.CoinsSetEvent += SetCoins;

        uiInputData.ShowPlayerDetailsEvent += ShowPlayerDetails;
        uiInputData.SetPlayerStatusEvent += SetStatus;
        uiInputData.OtherPlayerLeftGameEvent += ShowOtherPlayerDisconnection;

        playerData.PlayerStrikeStartedEvent += PlayerStrikeStarted;
        playerData.PlayerImageLoadedEvent += ShowImages;

        if (uiOutputData.gameType == GameType.MultiPlayer)
        {
            InviteObject.SetActive(true);
        }

        Reset();
    }

    private void OnDisable()
    {
        leftPlayer.gameObject.SetActive(false);
        rightPlayer.gameObject.SetActive(false);

        gameData.TurnChangedEvent -= TurnChanged;
      
        uiOutputData.ShowGameResultEvent -= ShowGameResult;
        uiOutputData.HomeEvent -= TurnOff;
        uiOutputData.ExitGameEvent -= TurnOff;
        uiOutputData.PlayAgainEvent -= Reset;
        uiOutputData.CoinsSetEvent -= SetCoins;

        uiInputData.ShowPlayerDetailsEvent -= ShowPlayerDetails;
     
        uiInputData.SetPlayerStatusEvent -= SetStatus;
        uiInputData.OtherPlayerLeftGameEvent -= ShowOtherPlayerDisconnection;
        playerData.PlayerStrikeStartedEvent -= PlayerStrikeStarted;
        playerData.PlayerImageLoadedEvent -= ShowImages;

        InviteObject.SetActive(false);
        StopTurnTime();
        ResetImages();
    }

    private void ResetImages()
    {
        leftPlayer.SetPlayerImage(AppProperties.instance.DummyPersonIcon);
        rightPlayer.SetPlayerImage(AppProperties.instance.DummyPersonIcon);
    }

    private void ShowImages()
    {
        Player p1 = playerData.GetPlayer(1);
        Player p2 = playerData.GetPlayer(2);

        if (p1 != null)
        {
            leftPlayer.SetPlayerImage(p1.GetMyImage());
        }

        if (p2 != null)
        {
            rightPlayer.SetPlayerImage(p2.GetMyImage());
        }
    }

    private void PlayerStrikeStarted(int id, float arg2)
    {
        StopTurnTime();
    }

    private void TurnOff()
    {
        gameObject.SetActive(false);
    }

    private void Reset()
    {
        StopTurnTime();
        ResetIndicators(); 
        ResetStatus();
        leftPlayer.SetScore(0, 0, 0);
        rightPlayer.SetScore(0, 0, 0);

    }

    private void ShowGameResult(GameResult result)
    {
        StopTurnTime();
        TurnChanged(result.winningPlayerId);
        CenterCanvas.SetActive(true);
        gameResultView.gameObject.SetActive(true) ;
        gameResultView.ShowResult(result);
    }

    public void ShowOtherPlayerDisconnection()
    {
        CenterCanvas.SetActive(true);
        OtherPlayerDisconnectionPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ShowPlayerDetails(Player p)
    {
        if (p.myId == 1)
        {
            leftPlayer.SetGameName(Enum.GetName(typeof(Game), uiOutputData.game));  
            leftPlayer.SetPlayerName(p.myName);
            leftPlayer.SetPlayerImage(p.GetMyImage());
            leftPlayer.PlayerImageScroller.StopScrolling();
        }
        else {

            rightPlayer.SetGameName(Enum.GetName(typeof(Game), uiOutputData.game));
            rightPlayer.SetPlayerName(p.myName);
            rightPlayer.SetPlayerImage(p.GetMyImage());
            rightPlayer.PlayerImageScroller.StopScrolling();
        
        }

        SetCoins();

        if (playerData.CurrentPlayers.Count == 2)
        {
            InviteObject.SetActive(false);
        }
    }
    private void TurnChanged(int id)
    {

        AudioManager.instance.StopClockSound();

        if (uiOutputData.gameType == GameType.SinglePlayer)
        {
            if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
            {
                if (uiOutputData.game == Game.BlackAndWhite)
                {
                    Player p = playerData.GetPlayer(1);
                    if(p.myCoin == PlayerCoin.White)
                    {
                        int whiteScore1 = gameData.P1Whites+gameData.P2Whites;
                        int redScore1 = gameData.P1Red * 3;
                        int totalScore1 = whiteScore1+redScore1;

                        leftPlayer.SetScore(whiteScore1, redScore1, totalScore1);

                        int blackScore2 = gameData.P1Blacks + gameData.P2Blacks;   
                        int redScore2 = gameData.P2Red * 3;
                        int totalScore2 = blackScore2 + redScore2;

                        rightPlayer.SetScore(blackScore2, redScore2, totalScore2);


                    }
                    else
                    {
                        int blackScore1 = gameData.P1Blacks + gameData.P2Blacks;   
                        int redScore1 = gameData.P1Red * 3;
                        int totalScore1 = blackScore1 + redScore1;

                        leftPlayer.SetScore(blackScore1, redScore1, totalScore1);   

                        int whiteScore2 = gameData.P1Whites + gameData.P2Whites; 
                        int redScore2 = gameData.P2Red * 3;
                        int totalScore2 = whiteScore2 + redScore2;

                        rightPlayer.SetScore(whiteScore2, redScore2, totalScore2);

                    }
                }
                else if (uiOutputData.game == Game.FreeStyle)   
                {
                    int blackScore1 = gameData.P1Blacks;
                    int whiteScore1 = gameData.P1Whites; 
                    int redScore1 = gameData.P1Red * 3;
                    int totalScore1 = blackScore1 + redScore1+whiteScore1;

                    leftPlayer.SetScore(blackScore1+whiteScore1, redScore1, totalScore1);

                    int whiteScore2 =  gameData.P2Whites;
                    int blackScore2 =  gameData.P2Blacks;    
                    int redScore2 = gameData.P2Red*3;
                    int totalScore2 = whiteScore2 + redScore2+blackScore2;

                    rightPlayer.SetScore(whiteScore2+blackScore2, redScore2, totalScore2);
                }
            }
        }
        else if (uiOutputData.gameType == GameType.MultiPlayer)
        {
            if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
            {
                if (uiOutputData.game == Game.BlackAndWhite)
                {
                    Player p = playerData.GetPlayer(1);
                    if (p.myCoin == PlayerCoin.White)
                    {
                        int whiteScore1 = gameData.P1Whites + gameData.P2Whites;
                        int redScore1 = gameData.P1Red*3;
                        int totalScore1 = whiteScore1 + redScore1;

                        leftPlayer.SetScore(whiteScore1, redScore1, totalScore1);

                        int blackScore2 = gameData.P1Blacks + gameData.P2Blacks;
                        int redScore2 = gameData.P2Red*3;
                        int totalScore2 = blackScore2 + redScore2;

                        rightPlayer.SetScore(blackScore2, redScore2, totalScore2);


                    }
                    else
                    {
                        int blackScore1 = gameData.P1Blacks + gameData.P2Blacks;
                        int redScore1 = gameData.P1Red*3;
                        int totalScore1 = blackScore1 + redScore1;

                        leftPlayer.SetScore(blackScore1, redScore1, totalScore1);

                        int whiteScore2 = gameData.P1Whites + gameData.P2Whites;
                        int redScore2 = gameData.P2Red * 3;
                        int totalScore2 = whiteScore2 + redScore2;

                        rightPlayer.SetScore(whiteScore2, redScore2, totalScore2);

                    }
                }
                else if (uiOutputData.game == Game.FreeStyle)
                {
                    int blackScore1 = gameData.P1Blacks;
                    int whiteScore1 = gameData.P1Whites;
                    int redScore1 = gameData.P1Red * 3;
                    int totalScore1 = blackScore1 + redScore1 + whiteScore1;

                    leftPlayer.SetScore(blackScore1 + whiteScore1, redScore1, totalScore1);

                    int whiteScore2 = gameData.P2Whites;
                    int blackScore2 = gameData.P2Blacks;
                    int redScore2 = gameData.P2Red*3;
                    int totalScore2 = whiteScore2 + redScore2 + blackScore2;

                    rightPlayer.SetScore(whiteScore2 + blackScore2, redScore2, totalScore2);
                }

            }
        }


        ResetIndicators();
        SetTurnIndicator(id);

        if (uiOutputData.gameType == GameType.MultiPlayer)
        {
            if (turnTimeRoutine == null)
            {
                turnTimeRoutine = StartCoroutine(StartTurnTime(id));
            }
        }
    }

    // write a coroutine called startTurnTime where in 45 seconds it goes from 0 to 1
    public IEnumerator StartTurnTime(int id)
    {
        leftPlayer.SetTimer(0);
        rightPlayer.SetTimer(0);

        for (int i = 0; i <= 45; i++)
        {
            yield return new WaitForSeconds(1);
            if(id == 1)
            {
                leftPlayer.SetTimer(i / (45.0f));
            }
            else
            {
                rightPlayer.SetTimer(i / (45.0f));
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
        if(turnTimeRoutine != null)
        {
            StopCoroutine(turnTimeRoutine);
            turnTimeRoutine = null;
        }
        leftPlayer.ResetTimer();
        rightPlayer.ResetTimer();
    }
    private void ResetIndicators()
    {
        leftPlayer.SetTurnImage( Color.white); 
        rightPlayer.SetTurnImage( Color.white );
        leftPlayer.ResetTimer();
        rightPlayer.ResetTimer();
    }

    private void SetTurnIndicator(int id)
    {
        if (id == 1)
        {
            leftPlayer.SetTurnImage(Color.green);
        }
        else
        {
            rightPlayer.SetTurnImage(Color.green);
        }

    }

    public void ResetStatus()
    {
       
    }

    public void SetStatus(int id,string status)
    {

       
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
            if (uiOutputData.game == Game.BlackAndWhite)
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
            else if (uiOutputData.game == Game.FreeStyle)
            {
                leftPlayer.SetCoinImage(uiOutputData.BlackAndWhiteCoin);
                leftPlayer.SetRedImage(uiOutputData.RedCoin);
            }
        }
        else
        {
            if (uiOutputData.game == Game.BlackAndWhite)
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
            else if (uiOutputData.game == Game.FreeStyle)
            {
                rightPlayer.SetCoinImage(uiOutputData.BlackAndWhiteCoin);
                rightPlayer.SetRedImage(uiOutputData.RedCoin);
            }
        }
      
    }
}


                                                                
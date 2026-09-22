using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

public class FineLogic : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIOutputDataSO uiOutputData;
    public GameDataSO gameData;
    public CoinDataSO coinData;

    [Header("Actions")]
    public Action<PlayerCoin> PutFineEvent;
    public Action<PlayerCoin> ShowFoulEvent;

    public void CheckFine(Player currentPlayer, int Whites, int Blacks, int Reds, bool isFoul)
    {
        if (uiOutputData.challenge == Challenge.BlackAndWhite || uiOutputData.challenge == Challenge.BWTournament)
        {
            CheckFineBW(currentPlayer, Whites, Blacks, Reds, isFoul);
        }
        else if(uiOutputData.challenge == Challenge.FreeStyle || uiOutputData.challenge == Challenge.FSTournament)
        {
            CheckFineFS(currentPlayer, Whites, Blacks, Reds, isFoul);
        }
    }


    public void CheckFineBW(Player currentPlayer, int Whites, int Blacks, int Reds, bool isFoul)
    {
        // Check and update the status of the red coin based on the current move.
        DetermineRedCoinStatusBW(currentPlayer.myCoin, Whites, Blacks, Reds, isFoul);

        // If the move resulted in a foul.
        if (isFoul)
        {
            HandleFoulSituationBW(currentPlayer,currentPlayer.myCoin);
           
        }
    }

    public void CheckFineFS(Player currentPlayer, int Whites, int Blacks, int Reds, bool isFoul)
    {
        // Check and update the status of the red coin based on the current move.
        DetermineRedCoinStatusFS(currentPlayer.myCoin, Whites, Blacks, Reds, isFoul);

        // If the move resulted in a foul.
        if (isFoul)
        {
            HandleFoulSituationFS(currentPlayer,PlayerCoin.White);

        }
    }


    public void DetermineRedCoinStatusBW(PlayerCoin playerCoin, int Whites, int Blacks, int Reds, bool isFoul)
    {
        // Check if red coin has been potted
        bool redPotted = Reds > 0;

        // Check if the current player's coin is white or black
        bool currentPlayerIsWhite = playerCoin == PlayerCoin.White;
        bool currentPlayerIsBlack = !currentPlayerIsWhite;

        // Check if any white or black coins have been potted
        bool whitePotted = Whites > 0;
        bool blackPotted = Blacks > 0;

        // If a foul is committed, handle the red coin (if potted)
        if (isFoul && redPotted)
        {
            HandleRedCoinFoul(currentPlayerIsWhite, whitePotted, currentPlayerIsBlack, blackPotted);
            return;
        }

        // If the red coin needs to be covered
        if (gameData.ShouldICoverCoin)
        {
            HandleRedCoinCovering(currentPlayerIsWhite, whitePotted, currentPlayerIsBlack, blackPotted);
            return;
        }

        // Determine the status of red coin based on which coins have been potted
        if (redPotted)
        {
            if (currentPlayerIsWhite && !whitePotted || currentPlayerIsBlack && !blackPotted)
            {
                gameData.ShouldICoverCoin = true;

            }
            else if (currentPlayerIsWhite && whitePotted || currentPlayerIsBlack && blackPotted)
            {
                gameData.isRedCovered = true;

            }
        }
    }


    public void DetermineRedCoinStatusFS(PlayerCoin playerCoin, int Whites, int Blacks, int Reds, bool isFoul)
    {
        // Your existing logic here...
        // I haven't modified this as you seemed to be content with the previous explanations.
        if (!gameData.ShouldICoverCoin && !isFoul)
        {
            if (Reds > 0 && Whites == 0 && Blacks == 0)
            {
                gameData.ShouldICoverCoin = true;

            }
            else if (Reds > 0 && (Whites > 0 || Blacks > 0))
            {
                gameData.isRedCovered = true;

            }
        }
        else if (!gameData.ShouldICoverCoin && isFoul)
        {

            if (Reds > 0 && Whites == 0 && Blacks == 0)
            {

                PutFineEvent?.Invoke(PlayerCoin.Red);
                gameData.TotalReds++;
                gameData.ShouldICoverCoin = false;
                gameData.isRedCovered = false;
                ResetRedsForAllPlayers();

            }
            else if (Reds > 0 && (Whites > 0 || Blacks > 0))
            {
                gameData.isRedCovered = true;

            }


        }
        else if (gameData.ShouldICoverCoin)
        {

            if (Whites > 0 || Blacks > 0)
            {
                gameData.ShouldICoverCoin = false;
                gameData.isRedCovered = true;

            }
            else
            {


                PutFineEvent?.Invoke(PlayerCoin.Red);
                gameData.TotalReds++;

                gameData.ShouldICoverCoin = false;
                gameData.isRedCovered = false;
                ResetRedsForAllPlayers();

            }

        }
    }

    /// <summary>
    /// Handles actions to be taken if the currentPlayer commits a foul.
    /// </summary>
    private void HandleFoulSituationBW(Player currentPlayer,PlayerCoin playerCoin)
    {
        ShowFoulEvent?.Invoke(playerCoin);
            
        if (currentPlayer.myCoin == PlayerCoin.White)
        {
            if (gameData.TotalWhites < gameData.AllWhites)
            {
                PutFineEvent?.Invoke(playerCoin);
                gameData.TotalWhites++;
                DecrementPlayerScoreBW(currentPlayer);
            }
            
        }
        else if (currentPlayer.myCoin == PlayerCoin.Black)
        {
            if (gameData.AllBlacks < gameData.TotalBlacks)
            {
                PutFineEvent?.Invoke(playerCoin);
                gameData.TotalBlacks++;
                DecrementPlayerScoreBW(currentPlayer);
            }
        }    

    }

    private void HandleFoulSituationFS(Player currentPlayer, PlayerCoin playerCoin)
    {
        ShowFoulEvent?.Invoke(playerCoin);

        if (uiOutputData.gameType == GameType.VsCPU)
        {
            if(uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
            {
                if(currentPlayer.myId == 1)
                {

                }
                else
                {

                }
            }
            else
            {
                if (currentPlayer.myTeam == Team.TeamA)
                {

                }
                else
                {

                }
            }
        }
        else if (uiOutputData.gameType == GameType.PlayWithFriends)
        {
            if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
            {
                if (currentPlayer.myId == 1)
                {
                    if(gameData.P1Whites > 0)
                    {
                        PutFineEvent?.Invoke(PlayerCoin.White);
                        gameData.TotalWhites++;
                        DecrementPlayerScoreFS(currentPlayer);
                    }
                    else if (gameData.P1Blacks > 0)
                    {
                        PutFineEvent?.Invoke(PlayerCoin.Black);
                        gameData.TotalBlacks++;
                        DecrementPlayerScoreFS(currentPlayer);
                    }
                }
                else
                {
                    if (gameData.P2Whites > 0)
                    {
                        PutFineEvent?.Invoke(PlayerCoin.White);
                        gameData.TotalWhites++;
                        DecrementPlayerScoreFS(currentPlayer);
                    }
                    else if (gameData.P2Blacks > 0)
                    {
                        PutFineEvent?.Invoke(PlayerCoin.Black);
                        gameData.TotalBlacks++;
                        DecrementPlayerScoreFS(currentPlayer);
                    }
                }
            }
            else
            {
                if (currentPlayer.myTeam == Team.TeamA)
                {
                    if ((gameData.P1Whites+gameData.P2Whites) > 0)
                    {
                        PutFineEvent?.Invoke(PlayerCoin.White);
                        gameData.TotalWhites++;
                        DecrementPlayerScoreFS(currentPlayer);
                    }
                    else if ((gameData.P1Blacks+gameData.P2Blacks) > 0)
                    {
                        PutFineEvent?.Invoke(PlayerCoin.Black);
                        gameData.TotalBlacks++;
                        DecrementPlayerScoreFS(currentPlayer);
                    }
                }
                else
                {
                    if ((gameData.P3Whites + gameData.P4Whites) > 0)
                    {
                        PutFineEvent?.Invoke(PlayerCoin.White);
                        gameData.TotalWhites++;
                        DecrementPlayerScoreFS(currentPlayer);
                    }
                    else if ((gameData.P3Blacks + gameData.P4Blacks) > 0)
                    {
                        PutFineEvent?.Invoke(PlayerCoin.Black);
                        gameData.TotalBlacks++;
                        DecrementPlayerScoreFS(currentPlayer);
                    }
                }
            }
        }
    }

    private void HandleRedCoinFoul(bool currentPlayerIsWhite, bool whitePotted, bool currentPlayerIsBlack, bool blackPotted)
    {
        // Check the conditions when a foul is committed after potting the red coin
        if ((currentPlayerIsWhite && !whitePotted) || (currentPlayerIsBlack && !blackPotted))
        {
            PutFineEvent?.Invoke(PlayerCoin.Red);
            gameData.TotalReds++;
            gameData.ShouldICoverCoin = false;
            gameData.isRedCovered = false;
            ResetRedsForAllPlayers();
        }
        else if ((currentPlayerIsWhite && whitePotted) || (currentPlayerIsBlack && blackPotted))
        {
            gameData.isRedCovered = true;

        }
    }

    private void HandleRedCoinCovering(bool currentPlayerIsWhite, bool whitePotted, bool currentPlayerIsBlack, bool blackPotted)
    {
        // Check the conditions when the red coin needs to be covered
        if ((currentPlayerIsWhite && whitePotted) || (currentPlayerIsBlack && blackPotted))
        {
            gameData.ShouldICoverCoin = false;
            gameData.isRedCovered = true;
        }
        else if ((currentPlayerIsWhite && !whitePotted) || (currentPlayerIsBlack && !blackPotted))
        {
            PutFineEvent?.Invoke(PlayerCoin.Red);
            gameData.TotalReds++;
            gameData.ShouldICoverCoin = false;
            gameData.isRedCovered = false;
            ResetRedsForAllPlayers();
        }
    }

    private void ResetRedsForAllPlayers()
    {
        gameData.P1Red = 0;
        gameData.P2Red = 0;
        gameData.P3Red = 0;
        gameData.P4Red = 0;
    }

    private void DecrementPlayerScoreBW(Player player)
    {
        if (player.myId == 1)
        {
            if (player.myCoin == PlayerCoin.White)
            {
                gameData.P1Whites--;
            }
            if (player.myCoin == PlayerCoin.Black)
            {
                gameData.P1Blacks--;
            }
        }
        else if (player.myId == 2)
        {
            if (player.myCoin == PlayerCoin.White)
            {
                gameData.P2Whites--;
            }
            if (player.myCoin == PlayerCoin.Black)
            {
                gameData.P2Blacks--;
            }
        }
        else if (player.myId == 3)
        {
            if (player.myCoin == PlayerCoin.White)
            {
                gameData.P3Whites--;
            }
            if (player.myCoin == PlayerCoin.Black)
            {
                gameData.P3Blacks--;
            }
        }
        else if (player.myId == 4)
        {
            if (player.myCoin == PlayerCoin.White)
            {
                gameData.P4Whites--;
            }
            if (player.myCoin == PlayerCoin.Black)
            {
                gameData.P4Blacks--;
            }
        }
    }

    private void DecrementPlayerScoreFS(Player player)
    {
        // Using a switch statement for clarity
        switch (player.myId)
        {
            case 1:
                gameData.P1Whites--;
                break;
            case 2:
                gameData.P2Whites--;
                break;
            case 3:
                gameData.P3Whites--;
                break;
            case 4:
                gameData.P4Whites--;
                break;
            default:
                Debug.LogWarning($"Invalid Player ID: {player.myId}");
                break;
        }
    }
}

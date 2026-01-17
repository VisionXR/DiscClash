using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

public class FineLogic : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIOutputDataSO uiOutputData;
    public GameDataSO data;

    [Header("Actions")]
    public Action<PlayerCoin> PutFineEvent;

    public void CheckFine(Player currentPlayer, int Whites, int Blacks, int Reds, bool isFoul)
    {
        if (uiOutputData.game == Game.BlackAndWhite)
        {
            CheckFineBW(currentPlayer, Whites, Blacks, Reds, isFoul);
        }
        else if(uiOutputData.game == Game.FreeStyle)
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
        if (data.ShouldICoverCoin)
        {
            HandleRedCoinCovering(currentPlayerIsWhite, whitePotted, currentPlayerIsBlack, blackPotted);
            return;
        }

        // Determine the status of red coin based on which coins have been potted
        if (redPotted)
        {
            if (currentPlayerIsWhite && !whitePotted || currentPlayerIsBlack && !blackPotted)
            {
                data.ShouldICoverCoin = true;

            }
            else if (currentPlayerIsWhite && whitePotted || currentPlayerIsBlack && blackPotted)
            {
                data.isRedCovered = true;

            }
        }
    }


    public void DetermineRedCoinStatusFS(PlayerCoin playerCoin, int Whites, int Blacks, int Reds, bool isFoul)
    {
        // Your existing logic here...
        // I haven't modified this as you seemed to be content with the previous explanations.
        if (!data.ShouldICoverCoin && !isFoul)
        {
            if (Reds > 0 && Whites == 0 && Blacks == 0)
            {
                data.ShouldICoverCoin = true;

            }
            else if (Reds > 0 && (Whites > 0 || Blacks > 0))
            {
                data.isRedCovered = true;

            }
        }
        else if (!data.ShouldICoverCoin && isFoul)
        {

            if (Reds > 0 && Whites == 0 && Blacks == 0)
            {

                PutFineEvent?.Invoke(PlayerCoin.Red);
                data.TotalReds++;
                data.ShouldICoverCoin = false;
                data.isRedCovered = false;
                ResetRedsForAllPlayers();

            }
            else if (Reds > 0 && (Whites > 0 || Blacks > 0))
            {
                data.isRedCovered = true;

            }


        }
        else if (data.ShouldICoverCoin)
        {

            if (Whites > 0 || Blacks > 0)
            {
                data.ShouldICoverCoin = false;
                data.isRedCovered = true;

            }
            else
            {


                PutFineEvent?.Invoke(PlayerCoin.Red);
                data.TotalReds++;

                data.ShouldICoverCoin = false;
                data.isRedCovered = false;
                ResetRedsForAllPlayers();

            }

        }
    }

    /// <summary>
    /// Handles actions to be taken if the currentPlayer commits a foul.
    /// </summary>
    private void HandleFoulSituationBW(Player currentPlayer,PlayerCoin playerCoin)
    {
        PutFineEvent?.Invoke(playerCoin);
     
        if (currentPlayer.myCoin == PlayerCoin.White)
        {
            data.TotalWhites++;
        }
        else if (currentPlayer.myCoin == PlayerCoin.Black)
        {
            data.TotalBlacks++;
        }

        DecrementPlayerScoreBW(currentPlayer);

    }

    private void HandleFoulSituationFS(Player currentPlayer, PlayerCoin playerCoin)
    {
        PutFineEvent?.Invoke(PlayerCoin.White);
        data.TotalWhites++;
        DecrementPlayerScoreFS(currentPlayer);

    }

    private void HandleRedCoinFoul(bool currentPlayerIsWhite, bool whitePotted, bool currentPlayerIsBlack, bool blackPotted)
    {
        // Check the conditions when a foul is committed after potting the red coin
        if ((currentPlayerIsWhite && !whitePotted) || (currentPlayerIsBlack && !blackPotted))
        {
            PutFineEvent?.Invoke(PlayerCoin.Red);
            data.TotalReds++;
            data.ShouldICoverCoin = false;
            data.isRedCovered = false;
            ResetRedsForAllPlayers();
        }
        else if ((currentPlayerIsWhite && whitePotted) || (currentPlayerIsBlack && blackPotted))
        {
            data.isRedCovered = true;

        }
    }

    private void HandleRedCoinCovering(bool currentPlayerIsWhite, bool whitePotted, bool currentPlayerIsBlack, bool blackPotted)
    {
        // Check the conditions when the red coin needs to be covered
        if ((currentPlayerIsWhite && whitePotted) || (currentPlayerIsBlack && blackPotted))
        {
            data.ShouldICoverCoin = false;
            data.isRedCovered = true;
        }
        else if ((currentPlayerIsWhite && !whitePotted) || (currentPlayerIsBlack && !blackPotted))
        {
            PutFineEvent?.Invoke(PlayerCoin.Red);
            data.TotalReds++;
            data.ShouldICoverCoin = false;
            data.isRedCovered = false;
            ResetRedsForAllPlayers();
        }
    }

    private void ResetRedsForAllPlayers()
    {
        data.P1Red = 0;
        data.P2Red = 0;
        data.P3Red = 0;
        data.P4Red = 0;
    }

    private void DecrementPlayerScoreBW(Player player)
    {
        if (player.myId == 1)
        {
            if (player.myCoin == PlayerCoin.White)
            {
                data.P1Whites--;
            }
            if (player.myCoin == PlayerCoin.Black)
            {
                data.P1Blacks--;
            }
        }
        else if (player.myId == 2)
        {
            if (player.myCoin == PlayerCoin.White)
            {
                data.P2Whites--;
            }
            if (player.myCoin == PlayerCoin.Black)
            {
                data.P2Blacks--;
            }
        }
        else if (player.myId == 3)
        {
            if (player.myCoin == PlayerCoin.White)
            {
                data.P3Whites--;
            }
            if (player.myCoin == PlayerCoin.Black)
            {
                data.P3Blacks--;
            }
        }
        else if (player.myId == 4)
        {
            if (player.myCoin == PlayerCoin.White)
            {
                data.P4Whites--;
            }
            if (player.myCoin == PlayerCoin.Black)
            {
                data.P4Blacks--;
            }
        }
    }

    private void DecrementPlayerScoreFS(Player player)
    {
        // Using a switch statement for clarity
        switch (player.myId)
        {
            case 1:
                data.P1Whites--;
                break;
            case 2:
                data.P2Whites--;
                break;
            case 3:
                data.P3Whites--;
                break;
            case 4:
                data.P4Whites--;
                break;
            default:
                Debug.LogWarning($"Invalid Player ID: {player.myId}");
                break;
        }
    }
}

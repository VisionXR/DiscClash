using com.VisionXR.HelperClasses;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "ScriptableObjects/GameDataSO", order = 1)]
public class GameDataSO : ScriptableObject
{
    public int TotalCoins;
    public int TotalWhites;
    public int TotalBlacks;
    public int TotalReds;
    public int P1Whites;
    public int P1Blacks;
    public int P1Red;
    public int P2Whites;
    public int P2Blacks;
    public int P2Red;
    public int P3Whites;
    public int P3Blacks;
    public int P3Red;
    public int P4Whites;
    public int P4Blacks;
    public int P4Red;
    public bool isRedCovered;
    public bool ShouldICoverCoin;
    public int currentTurnId = 1;
    public int TurnTime = 20;
    
    public Action<int> TurnChangedEvent;
    public Action<CurrentGameData> GameResultEvent;

    private void OnEnable()
    {
        currentTurnId = 1;
    }
    public void ChangeTurn(int id)
    {
        currentTurnId = id;
        TurnChangedEvent?.Invoke(currentTurnId);
    }
     public CurrentGameData GetCurrentGameData()
    {
        CurrentGameData data = new CurrentGameData();

        data.TotalCoins = TotalCoins;
        data.TotalWhites = TotalWhites;
        data.TotalBlacks = TotalBlacks;
        data.TotalReds = TotalReds;
        data.P1Whites = P1Whites;
        data.P1Blacks = P1Blacks;
        data.P1Red = P1Red;
        data.P2Whites = P2Whites;
        data.P2Blacks = P2Blacks;
        data.P2Red = P2Red;
        data.P3Whites = P3Whites;
        data.P3Blacks = P3Blacks;
        data.P3Red = P3Red;
        data.P4Whites = P4Whites;
        data.P4Blacks = P4Blacks;
        data.P4Red = P4Red;
        data.isRedCovered = isRedCovered;
        data.ShouldICoverCoin = ShouldICoverCoin;
        return data;

    }

    /// <summary>
    /// Sets the current game data from a given CurrentGameData object.
    /// Updates all coin counts and other related properties in the game.
    /// </summary>
    /// <param name="data">The CurrentGameData object containing the values to set.</param>
    public void SetCurrentGameData(CurrentGameData data)
    {
        // Set the total counts of coins based on the incoming data object
        TotalCoins = data.TotalCoins;
        TotalWhites = data.TotalWhites;
        TotalBlacks = data.TotalBlacks;
        TotalReds = data.TotalReds;

        // Set player 1's coin counts
        P1Whites = data.P1Whites;
        P1Blacks = data.P1Blacks;
        P1Red = data.P1Red;

        // Set player 2's coin counts
        P2Whites = data.P2Whites;
        P2Blacks = data.P2Blacks;
        P2Red = data.P2Red;

        // Set player 3's coin counts
        P3Whites = data.P3Whites;
        P3Blacks = data.P3Blacks;
        P3Red = data.P3Red;

        // Set player 4's coin counts
        P4Whites = data.P4Whites;
        P4Blacks = data.P4Blacks;
        P4Red = data.P4Red;

        // Set the red coin coverage and covering decision status
        isRedCovered = data.isRedCovered;
        ShouldICoverCoin = data.ShouldICoverCoin;


    }

    public void SetData(int totalCoins, int whites, int blacks, int reds)
    {
        ResetData();
        TotalCoins = totalCoins;
        TotalWhites = whites;
        TotalBlacks = blacks;
        TotalReds = reds;

    }

    public void ResetData()
    {
        TotalCoins = 0;
        TotalWhites = 0;
        TotalBlacks = 0;
        TotalReds = 0;
        P1Whites = 0;
        P1Blacks = 0;
        P1Red = 0;
        P2Whites = 0;
        P2Blacks = 0;
        P2Red = 0;
        P3Whites = 0;
        P3Blacks = 0;
        P3Red = 0;
        P4Whites = 0;
        P4Blacks = 0;
        P4Red = 0;
        isRedCovered = false;
        ShouldICoverCoin = false;
    }
}

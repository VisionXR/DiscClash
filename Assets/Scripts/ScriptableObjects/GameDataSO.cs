using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "ScriptableObjects/GameDataSO", order = 1)]
public class GameDataSO : ScriptableObject
{
    [Header("Scriptable Objects")]
    public UIOutputDataSO uiOutputData;
    public CoinDataSO coinData;

    [Header(" Game Variables ")]
    public int TotalCoins;
    public int AllWhites;
    public int AllBlacks;
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


    public int P1Score;
    public int P2Score;
    public int TeamAScore;
    public int TeamBScore;
    public bool isRedCovered;
    public bool ShouldICoverCoin;
    public int firstTurnId = -1;
    public int currentTurnId = 1;


    public TournamentData tournamentData;


    public Action<int> TurnChangedEvent;
    public Action<CurrentGameData> GameResultEvent;
    public Action StartGameEvent;

    private void OnEnable()
    {
        currentTurnId = 1;
        firstTurnId = -1;
        ResetData();
    }

    public void SetFirstTurnId(int id)
    {
        firstTurnId = id;
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
        AllWhites = whites;
        AllBlacks = blacks;
        TotalCoins = totalCoins;
        TotalWhites = whites;
        TotalBlacks = blacks;
        TotalReds = reds;

    }

    public void ResetData()
    {
        AllWhites = 0;
        AllBlacks = 0;
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
        P1Score = 0;
        P2Score = 0;
        TeamAScore = 0;
        TeamBScore = 0;

    }

    public int GetBWMatchPoints(Player winner)
    {
        bool single = 
       (uiOutputData.gameType == GameType.VsCPU && uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI) ||
       (uiOutputData.gameType == GameType.PlayWithFriends && uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2);


        bool isWinnerWhite = winner.myCoin == PlayerCoin.White;
        const int RedPointsMultiplier = 3;

        int opponentRemaining = 0;
        int redPoints = 0;

        if (single)
        {
            bool isPlayer1Win = (winner.myId == 1);

            if (isPlayer1Win)
            {
                opponentRemaining = isWinnerWhite ? (TotalBlacks) : (TotalWhites);
                redPoints = P1Red * RedPointsMultiplier;
            }
            else
            {
                opponentRemaining = isWinnerWhite ? (TotalBlacks) : (TotalWhites);
                redPoints = P2Red * RedPointsMultiplier;
            }
        }
        else // Team / Multiplayer Mode
        {
            bool isTeamAWin = (winner.myTeam == Team.TeamA);

            if (isTeamAWin)
            {
                opponentRemaining = isWinnerWhite ? (TotalBlacks) : (TotalWhites);
                redPoints = (P1Red + P2Red) * RedPointsMultiplier;
            }
            else
            {
                opponentRemaining = isWinnerWhite ? (TotalBlacks) : (TotalWhites);
                redPoints = (P3Red + P4Red) * RedPointsMultiplier;
            }
        }

       // int totalBase = isWinnerWhite ? TotalBlacks : TotalWhites;
        return  opponentRemaining + redPoints;
    }

    public int GetFSMatchPoints(Player winner)
    {
        int points = 0;
        if(uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
        {
            if(winner.myId == 1)
            {
                points = (P1Score) - (P2Score);
            }
            else
            {
                points =  P2Score - P1Score;
            }
        }
        else
        {
            if(winner.myTeam == Team.TeamA)
            {
                points = TeamAScore - TeamBScore;
                    
            }
            else
            {
                points = TeamBScore - TeamAScore;
            }
        }

        return points;
    }


    [Serializable]
    public class TournamentData
    {
        public int currentBoardNo = 0;
        public List<string> P1Scores;
        public List<string> P2Scores;
        public int P1TotalScore;
        public int P2TotalScore;


        public void SetBoardNo()
        {
            currentBoardNo++;
        }

        public void SetScores(string P1Score, string P2Score)
        {
            P1Scores[currentBoardNo] = P1Score;
            P2Scores[currentBoardNo] = P2Score;
        }

        public void CalculateScores()
        {
            int totalScore = 0;
            foreach (string score in P1Scores)
            {
                // TryParse returns true if successful and outputs the parsed int
                if (int.TryParse(score, out int parsedScore))
                {
                    totalScore += parsedScore;
                }
                // If it fails, it skips (leaves) the value and moves to the next
            }

            P1TotalScore = totalScore;

            totalScore = 0;
            foreach (string score in P2Scores)
            {
                // TryParse returns true if successful and outputs the parsed int
                if (int.TryParse(score, out int parsedScore))
                {
                    totalScore += parsedScore;
                }
                // If it fails, it skips (leaves) the value and moves to the next
            }
            P2TotalScore = totalScore;
        }

        public void ResetTournamentData()
        {
            currentBoardNo = 0;

            for (int i = 0; i < P1Scores.Count; i++)
            {
                P1Scores[i] = "_";
                P2Scores[i] = "_";
            }

            P1TotalScore = 0;
            P2TotalScore = 0;
        }


        public int GetPlayerScore(int playerId)
        {
            CalculateScores();

            if (playerId == 1)
            {
                return P1TotalScore;
            }
            else if (playerId == 2)
            {
                return P2TotalScore;
            }
            else
            {
                Debug.LogError("Invalid player ID: " + playerId);
                return -1; // or throw an exception
            }
        }
    }
}

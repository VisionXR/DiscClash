using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "ScriptableObjects/GameDataSO", order = 1)]
public class GameDataSO : ScriptableObject
{
    public UIOutputDataSO uiOutputData;

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
        P1Score = 0;
        P2Score = 0;
        TeamAScore = 0;
        TeamBScore = 0;

    }

    public int GetPointsForPlayer(Player mainPlayer)
    {
        int leaderboardPoints = 0;
        if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
        {
            if (mainPlayer.myId == 1)
            {
               
                    if (mainPlayer.myCoin == PlayerCoin.White)
                    {
                        leaderboardPoints = TotalBlacks - P2Blacks + P1Red*3;

                }
                    else
                    {
                        leaderboardPoints = TotalWhites - P2Whites + P1Red*3;
                       
                    }
               
            }
            else
            {

                if (mainPlayer.myCoin == PlayerCoin.White)
                {
                    leaderboardPoints = TotalBlacks - P1Blacks + P2Red*3;

                }
                else
                {
                    leaderboardPoints = TotalWhites - P1Whites + P2Red*3;
                }
            }
        }
        else
        {
            if (mainPlayer.myTeam == Team.TeamA)
            {
                if (mainPlayer.myCoin == PlayerCoin.White)
                {
                    leaderboardPoints = TotalBlacks - P3Blacks - P4Blacks + (P1Red + P2Red)*3;

                }
                else
                {
                    leaderboardPoints = TotalWhites - P3Whites - P4Whites + (P1Red + P2Red)*3;
                }
            }
            else
            {
                if (mainPlayer.myCoin == PlayerCoin.White)
                {
                    leaderboardPoints = TotalBlacks - P1Blacks - P2Blacks + (P3Red + P4Red)*3;

                }
                else
                {
                    leaderboardPoints = TotalWhites - P1Whites - P2Whites + (P3Red + P4Red)*3;
                }
            }
        }
        return leaderboardPoints;
    }


    [Serializable]
    public class TournamentData
    {
        public int currentBoardNo;
        public List<int> P1Scores;
        public List<int> P2Scores;
        public int P1TotalScore;
        public int P2TotalScore;


        public void SetBoardNo(int boardNo)
        {
            currentBoardNo = boardNo;
        }

        public void SetScores(int P1Score, int P2Score)
        {
            P1Scores[currentBoardNo] = P1Score;
            P2Scores[currentBoardNo] = P2Score;
        }

        public void CalculateScores()
        {
            int totalScore = 0;
            foreach (int score in P1Scores)
            {
                totalScore += score;
            }
            P1TotalScore = totalScore;

            totalScore = 0;
            foreach (int score in P2Scores)
            {
                totalScore += score;
            }
            P2TotalScore = totalScore;

        }

        public void ResetTournamentData()
        {
            currentBoardNo = 0;

            for (int i = 0; i < P1Scores.Count; i++)
            {
                P1Scores[i] = 0;
                P2Scores[i] = 0;
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

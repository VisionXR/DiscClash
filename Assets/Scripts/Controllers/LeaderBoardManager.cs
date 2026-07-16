using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
    using com.VisionXR.ModelClasses;

public class LeaderBoardManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public LeaderBoardSO leaderBoardData;


    private void OnEnable()
    {
        leaderBoardData.WriteToLeaderBoardEvent += WriteToLeaderBoard;
        leaderBoardData.GetTop10ScoresEvent += GetTop10Leaderboard;
    }

    private void OnDisable()
    {
        leaderBoardData.WriteToLeaderBoardEvent -= WriteToLeaderBoard;
        leaderBoardData.GetTop10ScoresEvent -= GetTop10Leaderboard;
    }


    /// <summary>
    /// Uploads singleplayer wins/score directly from the client.
    /// Best for offline/local CPU game modes where minor client authority is acceptable.
    /// </summary>
    public void WriteToLeaderBoard(int score,string apiName)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = apiName,
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request,
            result => Debug.Log("Score successfully posted to PlayFab!"),
            error => Debug.LogError($"Failed to update singleplayer score: {error.GenerateErrorReport()}")
        );
    }

    /// <summary>
    /// Retrieves the top singleplayer ranks.
    /// </summary>
    public void GetTop10Leaderboard(string apiName)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = apiName,
            StartPosition = 0,
            MaxResultsCount = 10,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true // Retrieves user's profile display name dynamically
            }
        };

        PlayFabClientAPI.GetLeaderboard(request,
            result => OnLeaderboardLoaded(result.Leaderboard, "Singleplayer"),
            error => Debug.LogError($"Failed to fetch singleplayer leaderboard: {error.GenerateErrorReport()}")
        );
    }


    private void OnLeaderboardLoaded(List<PlayerLeaderboardEntry> entries, string mode)
    {
        Debug.Log($"--- {mode} Leaderboard Loaded ---");
        List<string> names = new List<string>();
        List<int> ranks = new List<int>();
        List<int> scores = new List<int>();
        foreach (var entry in entries)
        {
            // Fallback to PlayFabId if DisplayName hasn't been set by the user yet
            string username = !string.IsNullOrEmpty(entry.DisplayName) ? entry.DisplayName : entry.PlayFabId;
            int rank = entry.Position + 1; // PlayFab ranks are 0-indexed
            int score = entry.StatValue;

            names.Add( username );
            ranks.Add( rank );
            scores.Add( rank );
   
        }

        leaderBoardData.ShowLeaderBoardData(names,ranks, scores);
    }


}
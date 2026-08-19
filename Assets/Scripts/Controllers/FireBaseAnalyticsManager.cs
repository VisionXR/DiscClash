using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class FireBaseAnalyticsManager : MonoBehaviour
{
    public static FireBaseAnalyticsManager Instance { get; private set; }

    private bool _isFirebaseInitialized = false;

    private void Awake()
    {
        // Singleton pattern to persist across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Enable Firebase Analytics collection
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                _isFirebaseInitialized = true;
                Debug.Log("[Firebase Analytics] Successfully initialized.");
            }
            else
            {
                Debug.LogError($"[Firebase Analytics] Could not resolve dependencies: {dependencyStatus}");
            }
        });
    }

    #region Carrom Game Analytics Events

    /// <summary>
    /// Log when a match begins.
    /// </summary>
    /// <param name="mode">e.g., "1v1", "Practice", "PassAndPlay", "Tournament"</param>
    /// <param name="boardType">e.g., "Classic", "DiscPool"</param>
    /// <param name="entryFee">Entry coins or gems used</param>
    public void LogGameStart(string gameType,string challenge)
    {
        if (!_isFirebaseInitialized) return;

        Debug.Log($"[Firebase Analytics] Logging game start: {gameType}, {challenge}");
        FirebaseAnalytics.LogEvent(
            "game_start",
            new Parameter("game_type", gameType),
            new Parameter("challenge", challenge)          
        );
    }

    public void LogGameExit(string gameType,string challenge, float matchDurationSeconds)
    {
        if (!_isFirebaseInitialized) return;

        FirebaseAnalytics.LogEvent(
            "game_exit",
             new Parameter("game_type", gameType),
             new Parameter("challenge", challenge),
             new Parameter("duration_seconds", matchDurationSeconds)

        );
    }

    /// <summary>
    /// Log when a match finishes.
    /// </summary>
    /// <param name="mode">e.g., "1v1", "DiscPool"</param>
    /// <param name="matchDurationSeconds">Duration of match in seconds</param>
    public void LogGameComplete(string gameType,string challenge, float matchDurationSeconds)
    {
        if (!_isFirebaseInitialized) return;

        FirebaseAnalytics.LogEvent(
            "game_complete",
             new Parameter("game_type", gameType),
             new Parameter("challenge", challenge),
             new Parameter("duration_seconds", matchDurationSeconds)
        );
    }


    #endregion
}
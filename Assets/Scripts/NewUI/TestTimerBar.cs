using UnityEngine;

public class TestTimerBar : MonoBehaviour
{
    [SerializeField] private TimerBar timerBar; // Assign in Inspector
    [SerializeField] private float testDuration = 5f;

    void Update()
    {
        // Press Space to start/restart the bar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (timerBar != null)
            {
                Debug.Log($"Starting test timer for {testDuration} seconds.");
                timerBar.StartTimer(testDuration);
            }
        }

        // Press R to stop & reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (timerBar != null)
            {
                Debug.Log("Resetting timer bar.");
                timerBar.StopTimer();
                // Optional: instantly reset fill
                timerBar.StartTimer(0.01f);
            }
        }
    }
}

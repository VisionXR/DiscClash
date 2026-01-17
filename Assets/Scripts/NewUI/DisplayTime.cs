using System.Collections;
using TMPro; // Required for TextMeshProUGUI
using UnityEngine;

public class DisplayTime : MonoBehaviour
{
    public TMP_Text timeText; // Assign your TextMeshProUGUI component here in the Inspector
    private Coroutine timeRoutine;
    private float elapsedTime = 0f; // Variable to store the elapsed time

    private void OnEnable()
    {
        ResetTime();
        // Start the coroutine when the GameObject becomes enabled
        // Ensure only one instance of the coroutine is running
        if (timeRoutine == null)
        {
            timeRoutine = StartCoroutine(ShowTime());
        }
    }

    private void OnDisable()
    {
        // Stop the coroutine when the GameObject becomes disabled
        // This prevents errors if the GameObject is destroyed or deactivated
        if (timeRoutine != null)
        {
            StopCoroutine(timeRoutine);
            timeRoutine = null;
        }
    }

    private IEnumerator ShowTime()
    {
        while (true)
        {
            // Increment the elapsed time by the time passed since the last frame
            elapsedTime += Time.deltaTime;

            // Calculate minutes and seconds
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            // Format the time as "MM:SS" with leading zeros if necessary
            // "D2" ensures two digits, padding with a leading zero if the number is less than 10
            timeText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);

            // Wait for the next frame before continuing the loop
            yield return null;
        }
    }

    // Optional: Method to reset the timer
    public void ResetTime()
    {
        elapsedTime = 0f;
        if (timeText != null)
        {
            timeText.text = "00:00"; // Reset display immediately
        }
    }
}

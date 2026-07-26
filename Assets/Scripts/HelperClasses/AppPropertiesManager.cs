using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;

public class AppPropertiesManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public AppDataSO appData;
    public MyPlayerSettings playerSettings;

    // Android Native Vibration Cache
    private AndroidJavaObject vibrator = null;

    private void Awake()
    {
        // Cache the Android Vibrator Service on initialization (Android only)
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to initialize Android Vibrator: " + e.Message);
        }
#endif
    }

    private void OnEnable()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        appData.StartVibrationEvent += StartVibration;
        appData.StartStrikingVibrationEvent += StartStrikerVibration;
        
    }

    private void OnDisable()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        appData.StartVibrationEvent -= StartVibration;
        appData.StartStrikingVibrationEvent -= StartStrikerVibration;
        
    }

    // Normal vibration (uses your custom duration loop)
    public void StartVibration()
    {
        if (playerSettings.isHapticsEnabled)
        {
            StopAllCoroutines(); // Ensure no overlapping vibration timers are running
            StartCoroutine(PlayHapticVibrationCoroutine());
        }
        
    }

    // Striker collision vibration (typically a quick, snappy response pulse)
    public void StartStrikerVibration()
    {
        if (playerSettings.isHapticsEnabled)
        {
            StopAllCoroutines();
            // Quick 40ms buzz perfect for physical game collisions (like a striker hit)
            VibrateAndroidNative(100);
        }
        
    }

    // Summary: Start haptic vibration for a given duration
    public IEnumerator PlayHapticVibrationCoroutine()
    {
        // Convert seconds to milliseconds for Android native call
        long durationInMs = (long)(appData.vibrationDuration * 1000f);
        VibrateAndroidNative(durationInMs);

        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + appData.vibrationDuration)
        {
            yield return null;
        }

        StopVibration();
    }

    public void StopVibration()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (vibrator != null)
        {
            vibrator.Call("cancel");
        }
#endif
    }

    /// <summary>
    /// Helper method to call the native Android Vibrator system API
    /// </summary>
    private void VibrateAndroidNative(long milliseconds)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (vibrator != null)
        {
            // For Android 8.0 (API 26) and above, using VibrationEffect is recommended,
            // but the basic "vibrate" method remains compatible as a robust fallback.
            vibrator.Call("vibrate", milliseconds);
        }
#else
        // Fallback for testing layouts inside the Unity Editor
       
#endif
    }

}

using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;

public class AppProperties : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public AppDataSO appData;
   

    // Android vibrator cache
#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject _vibrator;
    private AndroidJavaClass _vibrationEffectClass;
    private int _androidApiLevel = 0;
#endif

    private Coroutine _vibrationCoroutine;


    private void OnEnable()
    {
        appData.PlayButtonVibrationEvent += PlayButtonVibration;
        appData.PlayStrikerVibrationEvent += PlayStrikerVibration;
    }

    private void OnDisable()
    {
        appData.PlayButtonVibrationEvent -= PlayButtonVibration;
        appData.PlayStrikerVibrationEvent -= PlayStrikerVibration;
    }

    public void PlayButtonVibration()
    {
        PlayVibration(appData.vibrationDuration, appData.vibrationAmplitude);
    }

    public void PlayStrikerVibration()
    {
        PlayVibration(appData.vibrationDuration, appData.vibrationAmplitudeForStriking);
    }

    public void PlayVibration(float durationSeconds, float amplitude01)
    {
        // clamp
        durationSeconds = Mathf.Max(0f, durationSeconds);
        amplitude01 = Mathf.Clamp01(amplitude01);

        // stop any ongoing
        StopVibration();

#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            InitAndroidVibrator();

            if (_vibrator == null)
            {
                // fallback
                Handheld.Vibrate();
                _vibrationCoroutine = StartCoroutine(PlayVibrationCoroutine(durationSeconds));
                return;
            }

            long ms = Mathf.RoundToInt(durationSeconds * 1000f);

            if (_androidApiLevel >= 26)
            {
                // map amplitude 0..1 -> 1..255 (0 is interpreted as DEFAULT on some devices, avoid 0)
                int amp = Mathf.Clamp(Mathf.RoundToInt(amplitude01 * 255f), 1, 255);
                AndroidJavaObject effect = _vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", ms, amp);
                _vibrator.Call("vibrate", effect);
            }
            else
            {
                // older API: only duration
                _vibrator.Call("vibrate", ms);
            }

            // schedule stop if needed (cancel after duration)
            _vibrationCoroutine = StartCoroutine(PlayVibrationCoroutine(durationSeconds));
            return;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"[AppProperties] Android vibration failed: {ex.Message}");
            Handheld.Vibrate();
            _vibrationCoroutine = StartCoroutine(PlayVibrationCoroutine(durationSeconds));
            return;
        }
#endif


        // Generic fallback
        Handheld.Vibrate();
        _vibrationCoroutine = StartCoroutine(PlayVibrationCoroutine(durationSeconds));
    }

    private IEnumerator PlayVibrationCoroutine(float durationSeconds)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + durationSeconds)
        {
            yield return null;
        }

        StopVibration();
        _vibrationCoroutine = null;
    }

    public void StopVibration()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            if (_vibrator == null)
                InitAndroidVibrator();

            _vibrator?.Call("cancel");
        }
        catch { /* ignore */ }
#endif
        if (_vibrationCoroutine != null)
        {
            StopCoroutine(_vibrationCoroutine);
            _vibrationCoroutine = null;
        }
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void InitAndroidVibrator()
    {
        if (_vibrator != null) return;

        try
        {
            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                _androidApiLevel = version.GetStatic<int>("SDK_INT");
            }

            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
            _vibrator = context.Call<AndroidJavaObject>("getSystemService", "vibrator");

            if (_androidApiLevel >= 26)
            {
                _vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"[AppProperties] InitAndroidVibrator failed: {ex.Message}");
            _vibrator = null;
            _vibrationEffectClass = null;
        }
    }
#endif

}

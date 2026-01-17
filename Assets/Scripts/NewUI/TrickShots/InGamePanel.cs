using com.VisionXR.ModelClasses;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public TrickShotsDataSO trickShotData;
    public UIOutputDataSO uiOutputData;

    [Header("Panels")]
    public GameObject LevelPanel;
    public GameObject ResultPanel;
    public GameObject ExitPanel;

    [Header("UI Objects")]
    public Slider levelTimeSlider;
    public TMP_Text levelNumberText;
    public TMP_Text timeText;
    public AudioSource timerAudio;

    private Coroutine timerRoutine;
    private bool isPaused = false;

    public void OnEnable()
    {
        levelNumberText.SetText($"Level {trickShotData.currentLevelNo + 1}");

        isPaused = false;   
        timerAudio.Play();

        if (timerRoutine == null)
        {
            timerRoutine = StartCoroutine(StartTimer());
        }
    }

    public void OnDisable()
    {
        timerAudio.Stop();
        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
            timerRoutine = null;
        }
    }
    public void QuitBtnclciked()
    {
        AudioManager.instance.PlayButtonClickSound();
        ExitPanel.SetActive(true);
        isPaused = true;
        timerAudio.Stop();

    }
    public void ResumeBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        ExitPanel.SetActive(false);
        isPaused = false;
        timerAudio.Play();
    }

    private IEnumerator StartTimer()
    {
        levelTimeSlider.value = 1f;
        float elapsedTime = 0f;
        float totalTime = trickShotData.currentLevelTime;
        timeText.text = totalTime.ToString("F1") + "s";
        while (elapsedTime < totalTime)
        {
            if (!isPaused)
            {
                elapsedTime += Time.deltaTime;
                levelTimeSlider.value = Mathf.Clamp01(1f - (elapsedTime / totalTime));
                timeText.text = (totalTime - elapsedTime).ToString("F1") + "s";
                yield return null; // Wait for the next frame
            }
            else
            {
                yield return new WaitForEndOfFrame(); // Just wait while paused
            }
        }
        levelTimeSlider.value = 0f;
        timerRoutine = null;
    }

    private void ResetPanels()
    {
        LevelPanel.SetActive(false);
        gameObject.SetActive(false);
        ResultPanel.SetActive(false);
    }
}

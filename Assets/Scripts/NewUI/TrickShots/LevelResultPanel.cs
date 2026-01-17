using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;
using System.Collections;

public class LevelResultPanel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIOutputDataSO uIOutputData;
    public TrickShotsDataSO trickShotsData;

    [Header("UI Objects")]
    public TMP_Text levelNumberText;
    public GameObject LevelSuccessObj;
    public GameObject LevelFailObj;

    [Header("Stars (assign in Inspector)")]
    public GameObject Stars3;   // has 3 child star Images
    public GameObject Stars2;   // has 2 child star Images
    public GameObject Stars1;   // has 1 child star Image

    [Header("Star Reveal Settings")]
    [Tooltip("Delay between each star ping.")]
    public float starDelay = 0.25f;
    [Tooltip("Scale pop animation time per star.")]
    public float popTime = 0.12f;
    [Tooltip("Pop scale multiplier.")]
    public float popScale = 1.25f;

    [Header("Audio")]
    public AudioSource sfx;          // optional; if null, uses AudioManager
    public AudioClip starBellClip;   // bell sound to play per star

    public void NextBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        trickShotsData.LoadTrickShotLevel(trickShotsData.currentLevelNo + 1);
    }

    public void RetryBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        trickShotsData.LoadTrickShotLevel(trickShotsData.currentLevelNo);
    }

    public void PrevBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        trickShotsData.LoadTrickShotLevel(trickShotsData.currentLevelNo - 1);
    }

    public void ExitBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        uIOutputData.StopTrickShotsEvent?.Invoke();
        uIOutputData.HomeEvent?.Invoke();
        gameObject.SetActive(false);
    }

    // ------------ Result States ------------

    public void LevelSuccess(int stars)
    {
        levelNumberText.text = "Level Completed";
        LevelSuccessObj.SetActive(true);
        LevelFailObj.SetActive(false);

        // Reset star parents & children
        ResetAllStars();

        // Pick the right parent and reveal stars sequentially
        GameObject parent = stars >= 3 ? Stars3 : (stars == 2 ? Stars2 : Stars1);
        parent.SetActive(true);
        StartCoroutine(RevealStars(parent.transform, stars));
    }

    public void LevelFail()
    {
        levelNumberText.text = "Level Failed";
        LevelSuccessObj.SetActive(false);
        LevelFailObj.SetActive(true);
        ResetAllStars();
    }

    // ------------ Helpers ------------

    private void ResetAllStars()
    {
        // Hide all parents
        if (Stars3) Stars3.SetActive(false);
        if (Stars2) Stars2.SetActive(false);
        if (Stars1) Stars1.SetActive(false);

        // Ensure all star children start disabled & reset scale
        ResetChildren(Stars3);
        ResetChildren(Stars2);
        ResetChildren(Stars1);
    }

    private void ResetChildren(GameObject parent)
    {
        if (!parent) return;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            var t = parent.transform.GetChild(i);
            t.gameObject.SetActive(false);
            t.localScale = Vector3.one;
        }
    }

    private IEnumerator RevealStars(Transform parent, int count)
    {
        count = Mathf.Clamp(count, 1, parent.childCount);
        for (int i = 0; i < count; i++)
        {
            var star = parent.GetChild(i);
            star.gameObject.SetActive(true);

            // Pop scale animation
            yield return StartCoroutine(Pop(star, popScale, popTime));

            // Bell SFX
            PlayStarBell();

            // Small delay between stars
            yield return new WaitForSeconds(starDelay);
        }
    }

    private IEnumerator Pop(Transform target, float toScale, float time)
    {
        // scale up
        float t = 0f;
        Vector3 from = Vector3.one;
        Vector3 to = Vector3.one * toScale;
        while (t < time)
        {
            t += Time.unscaledDeltaTime; // respect timescale on result screen
            float k = t / time;
            target.localScale = Vector3.Lerp(from, to, k);
            yield return null;
        }
        // scale back to 1
        t = 0f;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            float k = t / time;
            target.localScale = Vector3.Lerp(to, Vector3.one, k);
            yield return null;
        }
    }

    private void PlayStarBell()
    {
        if (sfx && starBellClip)
        {
            sfx.PlayOneShot(starBellClip);
        }
        else 
        {
          // Fallback to the click sound if nothing else is set up
            AudioManager.instance?.PlayButtonClickSound();
        }
    }
}

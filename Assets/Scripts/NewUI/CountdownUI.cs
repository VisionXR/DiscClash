using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownUI : MonoBehaviour
{
    [Header("Refs")]
    public CanvasGroup root;          // optional (for fade in/out). If null, it's fine.
    public TMP_Text label;            // big number text

    [Header("Timing")]
    public int startFrom = 3;         // typically 3
    public float tickTime = 0.6f;     // each number on screen time
    public float goHoldTime = 0.4f;   // "GO" hold

    [Header("Pop")]
    public float popScale = 1.25f;
    public float popTime = 0.12f;

    [Header("Audio (optional)")]
    public AudioSource sfx;
    public AudioClip tickClip;
    public AudioClip goClip;

    public IEnumerator PlayRoutine(int from = -1)
    {
        gameObject.SetActive(true);
        if (root) root.alpha = 1f;

        int start = from > 0 ? from : startFrom;

        // 3..2..1
        for (int n = start; n >= 1; n--)
        {
            SetLabel(n.ToString());
            PlayTick();
            yield return Pop(label.transform, popScale, popTime);
            yield return new WaitForSecondsRealtime(Mathf.Max(0f, tickTime - popTime * 2f));
        }

        // GO!
        SetLabel("GO");
        //PlayGo();
        yield return Pop(label.transform, popScale, popTime);
        yield return new WaitForSecondsRealtime(goHoldTime);

        // hide
        if (root) root.alpha = 0f;
        gameObject.SetActive(false);
    }

    private void SetLabel(string text)
    {
        if (label)
        {
            label.text = text;
            label.transform.localScale = Vector3.one;
        }
    }

    private void PlayTick() { if (sfx && tickClip) sfx.PlayOneShot(tickClip); }
    //private void PlayGo() { if (sfx && goClip) sfx.PlayOneShot(goClip); }

    private IEnumerator Pop(Transform t, float toScale, float time)
    {
        // scale up
        float el = 0f;
        Vector3 from = Vector3.one;
        Vector3 to = Vector3.one * toScale;
        while (el < time)
        {
            el += Time.unscaledDeltaTime;
            t.localScale = Vector3.Lerp(from, to, el / time);
            yield return null;
        }
        // scale back
        el = 0f;
        while (el < time)
        {
            el += Time.unscaledDeltaTime;
            t.localScale = Vector3.Lerp(to, Vector3.one, el / time);
            yield return null;
        }
    }
}

using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class UIAnimator : MonoBehaviour
{
    public enum Ease
    {
        Linear,
        EaseOutCubic,
        EaseInCubic,
        EaseInOutCubic,
        EaseOutBack
    }

    [Header("Target")]
    [SerializeField] private RectTransform target;

    [Header("Zoom")]
    [SerializeField] private Vector3 hiddenScale = new Vector3(0.85f, 0.85f, 1f);
    [SerializeField] private Vector3 shownScale = Vector3.one;

    [Header("Slide (Optional)")]
    [SerializeField] private bool useSlide;
    [SerializeField] private Vector2 hiddenAnchoredPosOffset = new Vector2(0f, -40f);

    [Header("Fade (Optional)")]
    [SerializeField] private bool useFade = true;
    [SerializeField] private float hiddenAlpha = 0f;
    [SerializeField] private float shownAlpha = 1f;

    [Header("Timing")]
    [SerializeField] private float duration = 0.18f;
    [SerializeField] private float delay = 0f;
    [SerializeField] private Ease ease = Ease.EaseOutCubic;
    [SerializeField] private bool useUnscaledTime = true;

    [Header("Show Bounce (Only on Zoom In)")]
    [SerializeField] private bool useShowBounce = true;
    [SerializeField] private float showBounceStrength = 0.14f; // 0.08 - 0.22 good range
    [SerializeField] private float showBounceDamping = 10f;    // higher = settles faster

    [Header("Interactivity")]
    [SerializeField] private bool blockRaycastsWhenHidden = true;
    [SerializeField] private bool disableGameObjectWhenHidden;

    [Header("Runtime Test")]
    [SerializeField] private bool enableTestKeys = true;
    [SerializeField] private KeyCode showKey = KeyCode.Equals;      // '='
    [SerializeField] private KeyCode hideKey = KeyCode.Minus;       // '-'
    [SerializeField] private KeyCode toggleKey = KeyCode.BackQuote; // '`'

    private CanvasGroup canvasGroup;
    private Vector2 shownAnchoredPos;
    private Vector2 hiddenAnchoredPos;

    private Coroutine running;
    private bool isShown;

    private void Reset()
    {
        target = GetComponent<RectTransform>();
    }

    private void Awake()
    {
        if (target == null) target = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (useFade && canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        shownAnchoredPos = target.anchoredPosition;
        hiddenAnchoredPos = shownAnchoredPos + hiddenAnchoredPosOffset;
    }

    private void Update()
    {
        if (!enableTestKeys) return;

        if (Input.GetKeyDown(showKey)) Show();
        if (Input.GetKeyDown(hideKey)) Hide();
        if (Input.GetKeyDown(toggleKey)) Toggle();
    }

    public void Toggle()
    {
        if (isShown) Hide();
        else Show();
    }

    public void Show()
    {
        Play(true);
    }

    public void Hide()
    {
        Play(false);
    }

    public void SetInstantShown(bool shown)
    {
        if (running != null) StopCoroutine(running);
        running = null;

        isShown = shown;

        if (shown)
        {
            if (disableGameObjectWhenHidden && !gameObject.activeSelf) gameObject.SetActive(true);

            target.localScale = shownScale;
            target.anchoredPosition = shownAnchoredPos;

            if (useFade && canvasGroup != null) canvasGroup.alpha = shownAlpha;

            SetRaycastState(true);
        }
        else
        {
            target.localScale = hiddenScale;
            target.anchoredPosition = hiddenAnchoredPos;

            if (useFade && canvasGroup != null) canvasGroup.alpha = hiddenAlpha;

            SetRaycastState(false);

            if (disableGameObjectWhenHidden) gameObject.SetActive(false);
        }
    }

    private void Play(bool show)
    {
        if (running != null) StopCoroutine(running);

        if (show && disableGameObjectWhenHidden && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        running = StartCoroutine(Animate(show));
    }

    private IEnumerator Animate(bool show)
    {
        if (delay > 0f)
        {
            if (useUnscaledTime) yield return new WaitForSecondsRealtime(delay);
            else yield return new WaitForSeconds(delay);
        }

        isShown = show;

        Vector3 startScale = target.localScale;
        Vector3 endScale = show ? shownScale : hiddenScale;

        Vector2 startPos = target.anchoredPosition;
        Vector2 endPos = show ? shownAnchoredPos : hiddenAnchoredPos;

        float startAlpha = 1f;
        float endAlpha = 1f;

        if (useFade && canvasGroup != null)
        {
            startAlpha = canvasGroup.alpha;
            endAlpha = show ? shownAlpha : hiddenAlpha;
        }

        if (show)
        {
            SetRaycastState(true);
        }
        else
        {
            if (blockRaycastsWhenHidden) SetRaycastState(false);
        }

        float t = 0f;
        while (t < duration)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;

            float p = duration <= 0f ? 1f : Mathf.Clamp01(t / duration);
            float e = ApplyEase(p, ease);

            Vector3 scale = Vector3.LerpUnclamped(startScale, endScale, e);
            Vector2 pos = useSlide
                ? Vector2.LerpUnclamped(startPos, endPos, e)
                : Vector2.LerpUnclamped(startPos, endPos, e);

            // ✅ Show-bounce only (zoom in)
            if (show && useShowBounce)
            {
                // Damped overshoot that settles to 0 at the end
                float bounce = Mathf.Sin(p * Mathf.PI * 2f) * Mathf.Exp(-showBounceDamping * p) * showBounceStrength;
                float mult = 1f + bounce;
                scale = new Vector3(scale.x * mult, scale.y * mult, scale.z);
            }

            target.localScale = scale;
            target.anchoredPosition = pos;

            if (useFade && canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.LerpUnclamped(startAlpha, endAlpha, e);
            }

            yield return null;
        }

        target.localScale = endScale;
        target.anchoredPosition = endPos;

        if (useFade && canvasGroup != null) canvasGroup.alpha = endAlpha;

        if (!show)
        {
            SetRaycastState(false);

            if (disableGameObjectWhenHidden)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            SetRaycastState(true);
        }

        running = null;
    }

    private void SetRaycastState(bool enabled)
    {
        if (!useFade || canvasGroup == null) return;

        if (blockRaycastsWhenHidden)
        {
            canvasGroup.blocksRaycasts = enabled;
            canvasGroup.interactable = enabled;
        }
    }

    private static float ApplyEase(float t, Ease e)
    {
        switch (e)
        {
            case Ease.Linear:
                return t;

            case Ease.EaseInCubic:
                return t * t * t;

            case Ease.EaseOutCubic:
                {
                    float u = 1f - t;
                    return 1f - (u * u * u);
                }

            case Ease.EaseInOutCubic:
                return t < 0.5f
                    ? 4f * t * t * t
                    : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;

            case Ease.EaseOutBack:
                {
                    const float c1 = 1.70158f;
                    const float c3 = c1 + 1f;
                    float u = t - 1f;
                    return 1f + c3 * (u * u * u) + c1 * (u * u);
                }

            default:
                return t;
        }
    }

    // Hooks for UI Buttons
    public void OnClickShow() => Show();
    public void OnClickHide() => Hide();
    public void OnClickToggle() => Toggle();
}

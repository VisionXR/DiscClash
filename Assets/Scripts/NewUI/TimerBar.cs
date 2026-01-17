using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerBar : MonoBehaviour
{
    [Header("Wiring")]
    [SerializeField] private Image fillImage;     // the green fill (Image: Filled Horizontal)
    [SerializeField] private Image stripesImage;  // the Image using UI/ScrollingStripes
    [SerializeField] private Image bgImage;       // optional, for enabling/disabling

    [Header("Timing")]
    [SerializeField] private float duration = 10f;  // seconds
    [SerializeField] private bool autoStart = true;

    [Header("Colors")]
    [SerializeField] private Color green = new Color(0.15f, 0.9f, 0.2f);
    [SerializeField] private Color yellow = new Color(1f, 0.85f, 0.2f);
    [SerializeField] private Color red = new Color(1f, 0.25f, 0.2f);
    [Range(0f, 1f)][SerializeField] private float yellowAt = 0.5f; // <=50% turns yellow
    [Range(0f, 1f)][SerializeField] private float redAt = 0.2f;    // <=20% turns red

    [Header("Stripes")]
    [SerializeField] private float stripeSpeed = -0.8f; // negative scrolls right
    [SerializeField] private float stripeTiling = 8f;
    [SerializeField, Range(0f, 1f)] private float stripeAlpha = 0.25f;

    private Material _runtimeMat;
    private Coroutine _co;

    void Awake()
    {
        // Safety & per-instance material
        if (stripesImage && stripesImage.material)
            _runtimeMat = Instantiate(stripesImage.material);
        if (_runtimeMat != null)
        {
            _runtimeMat.SetVector("_UVSpeed", new Vector4(stripeSpeed, 0, 0, 0));
            _runtimeMat.SetVector("_Tiling", new Vector4(stripeTiling, 1, 0, 0));
            _runtimeMat.SetFloat("_Alpha", stripeAlpha);
            stripesImage.material = _runtimeMat;
        }

        if (fillImage != null)
        {
            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
            fillImage.fillAmount = 1f;
            fillImage.color = green;
        }
    }

    void OnEnable()
    {
        if (autoStart) StartTimer(duration);
    }

    public void StartTimer(float seconds)
    {
        duration = Mathf.Max(0.01f, seconds);
        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(CoTimer());
    }

    public void StopTimer()
    {
        if (_co != null) StopCoroutine(_co);
    }

    private IEnumerator CoTimer()
    {
        float t = duration;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float normalized = Mathf.Clamp01(t / duration); // 1→0
            UpdateVisuals(normalized);
            yield return null;
        }
        UpdateVisuals(0f);
        // TODO: invoke a callback/event here if you need "time’s up" logic
    }

    private void UpdateVisuals(float normalized)
    {
        if (fillImage)
        {
            fillImage.fillAmount = normalized;

            // Color thresholds (green → yellow → red)
            if (normalized <= redAt)
                fillImage.color = Color.Lerp(yellow, red, Mathf.InverseLerp(redAt, 0f, normalized));
            else if (normalized <= yellowAt)
                fillImage.color = Color.Lerp(green, yellow, Mathf.InverseLerp(1f, yellowAt, normalized));
            else
                fillImage.color = green;
        }
    }

    // Optional runtime tweaks
    public void SetStripeSpeed(float speed)
    {
        stripeSpeed = speed;
        if (_runtimeMat) _runtimeMat.SetVector("_UVSpeed", new Vector4(stripeSpeed, 0, 0, 0));
    }

    public void SetStripeTiling(float tiling)
    {
        stripeTiling = Mathf.Max(0.01f, tiling);
        if (_runtimeMat) _runtimeMat.SetVector("_Tiling", new Vector4(stripeTiling, 1, 0, 0));
    }

    public void SetStripeAlpha(float a)
    {
        stripeAlpha = Mathf.Clamp01(a);
        if (_runtimeMat) _runtimeMat.SetFloat("_Alpha", stripeAlpha);
    }
}

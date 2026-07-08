using UnityEngine;
using System.Collections;

public class SliderTutorialAnimator : MonoBehaviour
{
    [Header("Target References")]
    [Tooltip("The empty parent containing Hand and Down Arrow")]
    [SerializeField] private RectTransform handGroupParent;
    [Tooltip("The Right Arrow image that pulses")]
    [SerializeField] private RectTransform rightArrow;

    [Header("Hand/Drag Settings")]
    [SerializeField] private float dragDistance = 300f;
    [SerializeField] private float animationSpeed = 1.5f;
    [SerializeField] private float startScale = 1.3f;
    [SerializeField] private float pressedScale = 1.0f;

    [Header("Right Arrow Pulse Settings")]
    [SerializeField] private float pulseSpeed = 5f;
    [SerializeField] private float pulseAmount = 0.15f;
    [SerializeField] private float loopTime = 3f;

    private CanvasGroup _handCanvasGroup;
    private Vector3 _handInitialLocalPos;
    private Vector3 _arrowInitialScale;

    void Awake()
    {
        if (handGroupParent != null)
        {
            _handInitialLocalPos = handGroupParent.localPosition;

            // Get or add CanvasGroup to the hand group specifically
            _handCanvasGroup = handGroupParent.GetComponent<CanvasGroup>();
            if (_handCanvasGroup == null) _handCanvasGroup = handGroupParent.gameObject.AddComponent<CanvasGroup>();
        }

        if (rightArrow != null)
        {
            _arrowInitialScale = rightArrow.localScale;
        }
    }

    void OnEnable()
    {
        if (handGroupParent != null)
            StartCoroutine(PlayHandRoutine());
    }

    void Update()
    {
        if (rightArrow != null)
        {
            ApplyPulseEffect();
        }
    }

    private void ApplyPulseEffect()
    {
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        rightArrow.localScale = _arrowInitialScale + new Vector3(pulse, pulse, 0);
    }

    IEnumerator PlayHandRoutine()
    {
        while (true)
        {
            // 1. RESET
            handGroupParent.localPosition = _handInitialLocalPos;
            handGroupParent.localScale = Vector3.one * startScale;
            _handCanvasGroup.alpha = 0;

            // 2. PRESS (Scale Down + Fade In)
            yield return StartCoroutine(LerpHand(startScale, pressedScale, 0, 1, 0.4f / animationSpeed, false));

            yield return new WaitForSeconds(0.2f);

            // 3. DRAG (Move Down)
            float elapsed = 0;
            float duration = 1.0f / animationSpeed;
            Vector3 targetPos = _handInitialLocalPos + (Vector3.down * dragDistance);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                handGroupParent.localPosition = Vector3.Lerp(_handInitialLocalPos, targetPos, Mathf.SmoothStep(0, 1, t));
                yield return null;
            }

            // 4. LIFT (Scale Up + Fade Out)
            yield return StartCoroutine(LerpHand(pressedScale, startScale, 1, 0, 0.4f / animationSpeed, false));

            yield return new WaitForSeconds(loopTime); // Wait before looping
        }
    }

    // Helper to keep the main routine clean
    IEnumerator LerpHand(float sStart, float sEnd, float aStart, float aEnd, float duration, bool isSmooth)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            if (isSmooth) t = Mathf.SmoothStep(0, 1, t);

            handGroupParent.localScale = Vector3.one * Mathf.Lerp(sStart, sEnd, t);
            _handCanvasGroup.alpha = Mathf.Lerp(aStart, aEnd, t);
            yield return null;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyScrollController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;

    [Header("Settings")]
    [SerializeField][Range(0.1f, 0.5f)] private float scrollStep = 0.25f;
    [SerializeField] private float transitionDuration = 0.3f;
    [SerializeField] private bool autoHideButtons = true;

    private Coroutine scrollCoroutine;

    private void Start()
    {
        // Add listeners via code to keep the Inspector clean
        leftArrow.onClick.AddListener(() => StartScroll(-1));
        rightArrow.onClick.AddListener(() => StartScroll(1));

        // Initial button check
        if (autoHideButtons) UpdateButtonVisibility(scrollRect.horizontalNormalizedPosition);

        // Listen for manual swipes/drags to update button visibility
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    private void OnScrollValueChanged(Vector2 value)
    {
        if (autoHideButtons)
        {
            UpdateButtonVisibility(value.x);
        }
    }

    private void StartScroll(int direction)
    {
        float targetPos = scrollRect.horizontalNormalizedPosition + (direction * scrollStep);
        targetPos = Mathf.Clamp01(targetPos);

        if (scrollCoroutine != null) StopCoroutine(scrollCoroutine);
        scrollCoroutine = StartCoroutine(LerpScroll(targetPos));
    }

    private IEnumerator LerpScroll(float target)
    {
        float start = scrollRect.horizontalNormalizedPosition;
        float elapsed = 0;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;

            // Using SmoothStep for a more premium "mobile" feel
            scrollRect.horizontalNormalizedPosition = Mathf.SmoothStep(start, target, t);
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = target;
    }

    private void UpdateButtonVisibility(float normalizedX)
    {
        // Using a small epsilon (0.01) to account for floating point precision
        leftArrow.gameObject.SetActive(normalizedX > 0.01f);
        rightArrow.gameObject.SetActive(normalizedX < 0.99f);
    }
}
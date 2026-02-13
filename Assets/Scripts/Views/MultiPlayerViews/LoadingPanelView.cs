using UnityEngine;
using TMPro;
using System.Collections;

public class LoadingPanelView : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Settings")]
    [SerializeField] private string baseMessage = "Fetching data from cloud";
    [SerializeField] private float animationSpeed = 0.5f;

    private Coroutine animationCoroutine;

    private void OnEnable()
    {
        // Start the animation as soon as the panel is active
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimateText());
    }

    private void OnDisable()
    {
        // Clean up when the panel is hidden
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    private IEnumerator AnimateText()
    {
        int dotCount = 0;
        while (true)
        {
            string dots = new string('.', dotCount);
            statusText.text = $"{baseMessage}{dots}";

            dotCount++;
            if (dotCount > 3) dotCount = 0;

            yield return new WaitForSeconds(animationSpeed);
        }
    }

    // Call this from AuthManager when PlayFab is done
    public void HideLoading()
    {
        gameObject.SetActive(false);
    }
}
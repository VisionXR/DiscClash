using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanelView : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIInputDataSO uiInputData;
    public UIOutputDataSO uiOutputData;
    public CloudDataSO cloudData;
    public DestinationDataSO destinationData;
    public MyPlayerSettings playerSettings;

    [Header("UI References")]
    public TextMeshProUGUI statusText;
    public Image ConnectingImage;

    [Header("Animation Settings")]
    [SerializeField, Tooltip("Seconds between dot updates")] private float animationSpeed = 0.5f;
    [SerializeField, Tooltip("Maximum number of dots to show")] private int maxDots = 3;
    [SerializeField, Tooltip("Degrees per second for connecting image rotation")] private float rotationSpeed = 180f;
    [SerializeField, Tooltip("If true rotates clockwise")] private bool rotateClockwise = true;

    [Header("UI Elements")]
    public GameObject RetryButton;
    public GameObject PlayOfflineBtn;

    // local variables
    private Coroutine animationCoroutine;
    private string topMessage, bottomMessage;

    private void Awake()
    {
        if (maxDots < 1) maxDots = 3;
        if (animationSpeed <= 0f) animationSpeed = 0.5f;
        if (rotationSpeed < 0f) rotationSpeed = Mathf.Abs(rotationSpeed);

        // default messages
        topMessage = "Fetching Data";
        bottomMessage = "Please Wait";
    }

    private void OnEnable()
    {
        // start the combined animation coroutine
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimateTwoLinePulseAndRotate());

        cloudData.FetchSuccessEvent += OnLoginFetchSuccess;
        cloudData.FetchFailureEvent += OnLoginFetchFailure;

        RetryButton.SetActive(false);
        PlayOfflineBtn.SetActive(false);
    }

    private void OnDisable()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        // stop rotating and optionally reset rotation
        if (ConnectingImage != null)
            ConnectingImage.rectTransform.localRotation = Quaternion.identity;

        cloudData.FetchSuccessEvent -= OnLoginFetchSuccess;
        cloudData.FetchFailureEvent -= OnLoginFetchFailure;
    }

    // Called when LoginFetchManager reports success
    private void OnLoginFetchSuccess()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        // reset rotation on success
        if (ConnectingImage != null)
            ConnectingImage.rectTransform.localRotation = Quaternion.identity;

        playerSettings.SetLoginType(LoginType.Google);
        uiInputData.ShowDestination(destinationData.currentDestination);

        gameObject.SetActive(false);
    }

    // Called when LoginFetchManager reports failure
    private void OnLoginFetchFailure()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        // show error messages and enable retry controls
        topMessage = "Something Went Wrong";
        bottomMessage = " Retry or play offline";
        statusText.text = $"{topMessage}\n{bottomMessage}";

        RetryButton.SetActive(true);
        PlayOfflineBtn.SetActive(true);
    }

    // Combined animation: two-line text with ping-pong dots and continuous rotation of the ConnectingImage.
    private IEnumerator AnimateTwoLinePulseAndRotate()
    {
        int dotCount = 0;
        bool increasing = true;

        // Safety: ensure default messages are set
        if (string.IsNullOrEmpty(topMessage)) topMessage = "Fetching Data";
        if (string.IsNullOrEmpty(bottomMessage)) bottomMessage = "Please Wait";

        // Use a high-frequency loop to keep rotation smooth while updating dots at animationSpeed intervals.
        float elapsedSinceDotUpdate = 0f;
        while (true)
        {
            // rotate every frame
            float sign = rotateClockwise ? -1f : 1f; // negative z rotates clockwise visually in UI
            if (ConnectingImage != null)
            {
                ConnectingImage.rectTransform.Rotate(0f, 0f, sign * rotationSpeed * Time.deltaTime);
            }

            // update dot timer
            elapsedSinceDotUpdate += Time.deltaTime;
            if (elapsedSinceDotUpdate >= animationSpeed)
            {
                elapsedSinceDotUpdate = 0f;

                string dots = new string('.', dotCount);
                statusText.text = $"{topMessage}\n{bottomMessage}{dots}";

                // ping-pong dot logic
                if (increasing)
                {
                    dotCount++;
                    if (dotCount > maxDots)
                    {
                        dotCount = Mathf.Max(0, maxDots - 1);
                        increasing = false;
                    }
                }
                else
                {
                    dotCount--;
                    if (dotCount < 0)
                    {
                        dotCount = Mathf.Min(1, maxDots);
                        increasing = true;
                    }
                }
            }

            yield return null;
        }
    }

    public void RetryBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();

        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimateTwoLinePulseAndRotate());
        cloudData.Retry();

        RetryButton.SetActive(false);
        PlayOfflineBtn.SetActive(false);
        Debug.Log("Retry clicked");
    }

    public void PlayOfflineBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();

        playerSettings.SetLoginType(LoginType.Guest);
        uiInputData.ShowDestination(destinationData.homeDestination);
        gameObject.SetActive(false);
        Debug.Log("Play Offline Clicked");
    }
}
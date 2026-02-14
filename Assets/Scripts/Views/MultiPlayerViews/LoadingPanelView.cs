using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingPanelView : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIInputDataSO uiInputData;
    public UIOutputDataSO uiOutputData;
    public CloudDataSO cloudData;
    public DestinationDataSO destinationData;
    public MyPlayerSettings playerSettings;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Messages")]
    [SerializeField] private string topMessage = "Fetching Data";
    [SerializeField] private string bottomMessage = "Please Wait";

    [Header("Animation Settings")]
    [SerializeField, Tooltip("Seconds between dot updates")] private float animationSpeed = 0.5f;
    [SerializeField, Tooltip("Maximum number of dots to show")] private int maxDots = 3;

    [Header("UI Elements")]
    public GameObject RetryButton;
    public GameObject PlayOfflineBtn;

    private Coroutine animationCoroutine;

    private void Awake()
    {
        if (maxDots < 1) maxDots = 3;
        if (animationSpeed <= 0f) animationSpeed = 0.5f;
    }

    private void OnEnable()
    {
        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimateTwoLinePulse());

        cloudData.FetchSuccessEvent += OnLoginFetchSuccess;
        cloudData.FetchFailureEvent += OnLoginFetchFailure;

        RetryButton.SetActive(false);
        PlayOfflineBtn.SetActive(false);
    }

    private void OnDisable()
    {
      

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

        topMessage = "Something Went Wrong";
        bottomMessage = " Retru or play offline";
        statusText.text = $"{topMessage}\n{bottomMessage}";

        RetryButton.SetActive(true);
        PlayOfflineBtn.SetActive(true);
        // UI's retry button should call loginFetchManager.RetryFetch() or StartLoginAndFetchCoins()
    }

    private IEnumerator AnimateTwoLinePulse()
    {
        int dotCount = 0;
        bool increasing = true;

        while (true)
        {
            string dots = new string('.', dotCount);
            statusText.text = $"{topMessage}\n{bottomMessage}{dots}";

            // advance dotCount in a ping-pong fashion: 0..maxDots..0
            if (increasing)
            {
                dotCount++;
                if (dotCount > maxDots)
                {
                    // switch direction and step back so maxDots is visible once
                    dotCount = Mathf.Max(0, maxDots - 1);
                    increasing = false;
                }
            }
            else
            {
                dotCount--;
                if (dotCount < 0)
                {
                    dotCount = Mathf.Min(1, maxDots); // start increasing again
                    increasing = true;
                }
            }

            yield return new WaitForSeconds(animationSpeed);
        }
    }

    // Public API to change messages at runtime (optional)
    public void SetMessages(string top, string bottom)
    {
        topMessage = top ?? topMessage;
        bottomMessage = bottom ?? bottomMessage;
    }

    public void RetryBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();

        if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(AnimateTwoLinePulse());
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
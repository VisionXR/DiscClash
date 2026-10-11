using com.VisionXR.ModelClasses;
using com.VisionXR.HelperClasses;
using TMPro;
using PlayFab;
using System;
using System.Collections;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class LoginFetchManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public CloudDataSO cloudData;
        public UIDataSO uiInputData;
        public DestinationDataSO destinationData;

        // G02: Explicit recovery view references are wired in RealCarrom3DUI.
        public GameObject recoveryPanel;
        public TMP_Text recoveryText;
        public MyPlayerSettings playerSettings;
        public DeepLinkManager deepLinkManager;
        public CloudManager cloudManager;
        public UIInputDataSO gameUIInput;
        private bool awaitingLogin;

        [Header("Retry Settings")]
        public int maxAttempts = 3;
        public float retryDelaySeconds = 1f;
        public bool autoRetry = true;


        // internal state
        private int fetchAttempt;
        private bool awaitingFetch;
        private Coroutine retryCoroutine;

        private void OnEnable()
        {

            cloudData.PlayFabLoginSuccessEvent += OnPlayFabLoginSuccess;
            cloudData.PlayFabLoginFailureEvent += OnPlayFabLoginFailure;
            // G02: Starting authentication after offline play restores the online lifecycle.
            cloudData.LoginToGoogleEvent += BeginLogin;
            cloudData.GuestLoginEvent += BeginLogin;
            cloudData.EditorLoginEvent += BeginLogin;

        }

        private void OnDisable()
        {

            cloudData.PlayFabLoginSuccessEvent -= OnPlayFabLoginSuccess;
            cloudData.PlayFabLoginFailureEvent -= OnPlayFabLoginFailure;
            // G02: Cancel retries when this manager is disabled.
            cloudData.LoginToGoogleEvent -= BeginLogin;
            cloudData.GuestLoginEvent -= BeginLogin;
            cloudData.EditorLoginEvent -= BeginLogin;
            StopAllCoroutines();
            retryCoroutine = null;
            awaitingFetch = false;

        }


        // Internal callbacks -------------------------------------------------

        // G02: New login attempts leave offline mode before asynchronous authentication completes.
        private void BeginLogin()
        {
            // G02/G03: Superseding login attempts must cancel old fetches and scheduled retries.
            if (retryCoroutine != null) StopCoroutine(retryCoroutine);
            retryCoroutine = null;
            cloudManager.CancelLoad();
            awaitingFetch = false;
            cloudData.DataLoaded(false);
            awaitingLogin = true;
            cloudData.isOfflineSession = false;
            fetchAttempt = 0;
        }

        private void OnPlayFabLoginSuccess()
        {
            if (cloudData.isOfflineSession) return;
            awaitingLogin = false;
            fetchAttempt = 0;
            FetchPlayerData();
        }

        private void FetchPlayerData()
        {
            // G02: Bound automatic retries and avoid overlapping fetches.
            if (awaitingFetch || cloudData.isOfflineSession) return;
            awaitingFetch = true;
            fetchAttempt++;
            cloudData.LoadPlayerData(OnFetchSuccessInternal, OnFetchFailureInternal);
        }

        private void OnPlayFabLoginFailure()
        {
            // G02: Failed authentication must offer Retry and Play Offline rather than only logging.
            awaitingLogin = false;
            if (!cloudData.isOfflineSession) ShowRecovery("Login failed. Retry or play offline.");
        }

        private void OnFetchSuccessInternal()
        {
            awaitingFetch = false;
            HideRecovery();
            cloudData.FetchSuccess();
        }

        private void OnFetchFailureInternal()
        {
            // G02/G03: Failed/timed-out fetches retry a bounded number of times, then expose recovery.
            awaitingFetch = false;
            if (cloudData.isOfflineSession) return;
            if (autoRetry && fetchAttempt < Mathf.Max(1, maxAttempts)) ScheduleRetry(FetchPlayerData);
            else
            {
                cloudData.FetchFailure();
                ShowRecovery("Could not load player data. Retry or play offline.");
            }
        }

        public void RetryBtnClicked()
        {
            // G02: Wired Retry repeats the failed operation; a completed login needs only a fresh fetch.
            if (awaitingFetch || awaitingLogin) return;
            if (retryCoroutine != null) StopCoroutine(retryCoroutine);
            retryCoroutine = null;
            cloudData.isOfflineSession = false;
            fetchAttempt = 0;
            if (recoveryText != null) recoveryText.text = "Retrying...";
            if (PlayFabClientAPI.IsClientLoggedIn()) FetchPlayerData();
            else if (Application.isEditor) cloudData.EditorLogin();
            else if (playerSettings.loginType == LoginType.Guest) cloudData.GuestLogin();
            else cloudData.LoginToGoogle();
        }

        public void PlayOfflineBtnClicked()
        {
            // G02: Cancel pending fetch/retry work and create a local guest identity without a server login.
            StopAllCoroutines();
            retryCoroutine = null;
            awaitingFetch = false;
            awaitingLogin = false;
            cloudManager.CancelLoad();
            cloudData.isOfflineSession = true;
            cloudData.DataLoaded(false);
            if (string.IsNullOrEmpty(playerSettings.MyId))
                playerSettings.SetUserNameAndId("Guest", SystemInfo.deviceUniqueIdentifier);
            // G02: Leave any active game before returning to offline Home.
            gameUIInput.GoToHome();
            deepLinkManager.isLink = false;
            HideRecovery();
            uiInputData.uiManager.ChangeState("Link", false);
            uiInputData.uiManager.GoToState(StateName.HomeState);
        }

        private void ShowRecovery(string message)
        {
            if (recoveryText != null) recoveryText.text = message;
            if (recoveryPanel != null) recoveryPanel.GetComponent<PanelOnOff>().TurnOnPanel();
        }

        private void HideRecovery()
        {
            if (recoveryPanel != null) recoveryPanel.GetComponent<PanelOnOff>().TurnOffPanel();
        }

        private void ScheduleRetry(Action action)
        {
            if (retryCoroutine != null)
            {
                StopCoroutine(retryCoroutine);
                retryCoroutine = null;
            }
            retryCoroutine = StartCoroutine(RetryCoroutine(action));
        }

        private IEnumerator RetryCoroutine(Action action)
        {
            yield return new WaitForSeconds(retryDelaySeconds);
            // G02: Clear the handle before an action can synchronously schedule another retry.
            retryCoroutine = null;
            action?.Invoke();
        }
    }
}
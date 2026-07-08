using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class LoginFetchManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public CloudDataSO cloudData;
        public UIInputDataSO uiInputData;
        public DestinationDataSO destinationData;

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

            cloudData.StartFetchEvent += StartLoginAndFetchCoins;
            cloudData.RetryEvent += RetryFetch;

            cloudData.PlayFabLoginSuccessEvent += OnPlayFabLoginSuccess;
            cloudData.PlayFabLoginFailureEvent += OnPlayFabLoginFailure;

        }

        private void OnDisable()
        {
            cloudData.StartFetchEvent -= StartLoginAndFetchCoins;
            cloudData.RetryEvent -= RetryFetch;

            cloudData.PlayFabLoginSuccessEvent -= OnPlayFabLoginSuccess;
            cloudData.PlayFabLoginFailureEvent -= OnPlayFabLoginFailure;

        }

        // Public API ---------------------------------------------------------

        // Call from UI or GameManager to start the flow.
        public void StartLoginAndFetchCoins()
        {
         
            fetchAttempt = 0;
            awaitingFetch = true;

            // Kick off platform-specific login
            if (Application.isEditor)
                cloudData.EditorLogin();
            else
                cloudData.LoginToGoogle();
        }

        // Manual retry button should call this (UI)
        public void RetryFetch()
        {
            if (awaitingFetch)
            {
                Debug.Log("[LoginFetchManager] Already awaiting fetch, ignoring RetryFetch.");
                return;
            }

            // restart full flow with configured settings
            StartLoginAndFetchCoins();
        }

        // Internal callbacks -------------------------------------------------

        private void OnPlayFabLoginSuccess()
        {
            if (!awaitingFetch)
            {
                // if not part of an explicit StartLoginAndFetchCoins flow, forward existing behaviour
                cloudData.FetchCoins(OnFetchSuccessInternal, OnFetchFailureInternal);
                return;
            }

            AttemptFetch();
        }

        private void OnPlayFabLoginFailure()
        {
            Debug.LogWarning("[LoginFetchManager] PlayFab login failed.");

            if (!awaitingFetch) return;

            fetchAttempt++;

            if (autoRetry && fetchAttempt < maxAttempts)
            {
                ScheduleRetry(() =>
                {
                    Debug.Log($"[LoginFetchManager] Retrying login (attempt {fetchAttempt + 1})");
                    if (Application.isEditor)
                        cloudData.EditorLogin();
                    else
                        cloudData.LoginToGoogle();
                });
            }
            else
            {
                awaitingFetch = false;
                cloudData.FetchFailure();
            }
        }

        private void AttemptFetch()
        {
            fetchAttempt++;
            Debug.Log($"[LoginFetchManager] Fetch attempt {fetchAttempt} / {maxAttempts}");

            // cloudData.FetchCoins will trigger CloudManager.FetchUserCoins -> PlayFab call
            cloudData.FetchCoins(OnFetchSuccessInternal, OnFetchFailureInternal);
        }

        private void OnFetchSuccessInternal()
        {
            Debug.Log("[LoginFetchManager] Fetch success.");
            awaitingFetch = false;
            fetchAttempt = 0;

            cloudData.FetchSuccess();
        }

        private void OnFetchFailureInternal()
        {
            Debug.LogWarning("[LoginFetchManager] Fetch failed.");
           

            if (autoRetry && fetchAttempt < maxAttempts)
            {
                ScheduleRetry(() =>
                {
                    Debug.Log("[LoginFetchManager] Auto retrying fetch.");
                    AttemptFetch();
                });
            }
            else
            {
                awaitingFetch = false;
                cloudData.FetchFailure();
            }
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
            action?.Invoke();
            retryCoroutine = null;
        }
    }
}
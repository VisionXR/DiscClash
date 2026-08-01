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
        public UIDataSO uiInputData;
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

            cloudData.PlayFabLoginSuccessEvent += OnPlayFabLoginSuccess;
            cloudData.PlayFabLoginFailureEvent += OnPlayFabLoginFailure;

        }

        private void OnDisable()
        {

            cloudData.PlayFabLoginSuccessEvent -= OnPlayFabLoginSuccess;
            cloudData.PlayFabLoginFailureEvent -= OnPlayFabLoginFailure;

        }


        // Internal callbacks -------------------------------------------------

        private void OnPlayFabLoginSuccess()
        {

            // if not part of an explicit StartLoginAndFetchCoins flow, forward existing behaviour
            cloudData.LoadPlayerData(OnFetchSuccessInternal, OnFetchFailureInternal);

        }

        private void OnPlayFabLoginFailure()
        {
            Debug.Log("[LoginFetchManager] PlayFab login failed.");

        }


        private void OnFetchSuccessInternal()
        {

            cloudData.FetchSuccess();
        }

        private void OnFetchFailureInternal()
        {
            Debug.Log("[LoginFetchManager] Fetch failed.");

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
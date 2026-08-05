using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class DestinationPanelView : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public DestinationDataSO destinationData;
        public UIInputDataSO uiInputData;
        public UIDataSO uiData;

        [Header("UI Elements")]
        public TMP_Text statusText;
        public GameObject LinkExpiredBtn;
        public GameObject rotationImage;
        public GameObject RetryButton;
        public GameObject HomeButton;

        [Header("local Settings")]
        public Destination currentDestination;
        public float rotationSpeed = 90f; // Degrees per second
        public string joinLobbyState;

        // Events
        public Action DestinationSuccessEvent;
        public Action DestinationFailEvent;


        // local variables
        private Coroutine connectionRoutine = null;
        private Coroutine rotationRoutine = null;
       

        private bool canIConnect = false;

        private void OnEnable()
        {
            DestinationSuccessEvent += OnDestinationSuccess;
            DestinationFailEvent += OnDestinationFail;
            if(canIConnect)
            {
                ConnectToDestination(currentDestination);
                canIConnect = false;
            }
        }

        private void OnDisable()
        {
            DestinationSuccessEvent -= OnDestinationSuccess;
            DestinationFailEvent -= OnDestinationFail;
        }

        private void Initialise()
        {
            statusText.text = "";     
            HomeButton.SetActive(false);
            RetryButton.SetActive(false);
        }

        public void SetDestination(Destination d)
        {
            canIConnect = true;
            currentDestination = d;
        }

    
        public void ConnectToDestination(Destination d)
        {
            Initialise();
            currentDestination = d;
            if (connectionRoutine != null)
            {
                StopCoroutine(connectionRoutine);
                StopCoroutine(rotationRoutine);
            }

            string time = currentDestination.time;

            if (string.IsNullOrEmpty(time))
            {
                connectionRoutine = StartCoroutine(ConnectingToDestination());
                rotationRoutine = StartCoroutine(RotateImage());
                destinationData.ConnectToDestination(currentDestination, DestinationSuccessEvent, DestinationFailEvent);
            }
            else
            {
                try
                {
                    // 1. Parse the string using the exact pattern you saved it with
                    // CultureInfo.InvariantCulture prevents issues with regional device formats
                    DateTime linkTime = DateTime.ParseExact(time, "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

                    // 2. Get the current UTC time to compare against
                    DateTime currentTime = DateTime.UtcNow;

                    // 3. Calculate the difference
                    TimeSpan timeDifference = currentTime - linkTime;

                    // 4. Check if the link is older than 15 minutes OR if the link time is somehow in the future
                    if (timeDifference.TotalMinutes > 15 || timeDifference.TotalMinutes < 0)
                    {
                        Debug.LogWarning($"Link expired! It was created {timeDifference.TotalMinutes:F1} minutes ago.");
                        statusText.text = "Link Expired ...";
                        LinkExpiredBtn.SetActive(true);
                        // TODO: Call your UI manager here to show an "expired link" pop-up screen
                        // uiData.uiManager.ShowPopup("This invite link has expired. Please ask for a new one.");
                    }
                    else
                    {
                        if (connectionRoutine == null)
                        {
                            connectionRoutine = StartCoroutine(ConnectingToDestination());
                            rotationRoutine = StartCoroutine(RotateImage());
                        }
                        // The link is valid and within the 15-minute window!
                        Debug.Log($"Link is valid. Only {timeDifference.TotalMinutes:F1} minutes old. Connecting...");
                        destinationData.ConnectToDestination(currentDestination, DestinationSuccessEvent, DestinationFailEvent);
                    }
                }
                catch (FormatException)
                {
                    // If someone tampered with the URL parameter and it's no longer a valid date string
                    Debug.LogError($"Invalid time format in URL: '{time}'. Failed to parse.");
                    // Treat as expired/invalid link
                }
            }
           
        }

        public void RetryBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            HomeButton.SetActive(false);
            RetryButton.SetActive(false);
            ConnectToDestination(currentDestination);
        }

        public void HomeButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            if (connectionRoutine != null)
            {
                StopCoroutine(connectionRoutine);
                StopCoroutine(rotationRoutine);
                connectionRoutine = null;
            }

            LinkExpiredBtn.SetActive(false);
            HomeButton.SetActive(false);
            RetryButton.SetActive(false);
            uiInputData.ExitGame();
            uiData.uiManager.ChangeState("SinglePlayer", false);
            uiData.uiManager.ChangeState("MultiPlayer", false);
            uiData.uiManager.ChangeState("Home", true);
            uiData.uiManager.ResetAllBools();
        }

        private void OnDestinationSuccess()
        {
            Debug.Log("Destination change successful.");

            if (connectionRoutine != null)
            {
                StopCoroutine(connectionRoutine);
                StopCoroutine(rotationRoutine);
                connectionRoutine = null;
            }
            // Additional logic for successful destination change can be added here.
           if (currentDestination.gameType == GameType.PlayWithFriends)
            {
                uiData.uiManager.ChangeState(joinLobbyState, true);
            }

        }

        private void OnDestinationFail()
        {
           

            if (connectionRoutine != null)
            {
                StopCoroutine(connectionRoutine);
                StopCoroutine(rotationRoutine);
                connectionRoutine = null;
            }

            statusText.text = "Failed to connect to destination. Please try again.";
            HomeButton.SetActive(true);
            RetryButton.SetActive(true);

            // Additional logic for failed destination change can be added here.
        }




        private IEnumerator ConnectingToDestination()
        {
            while (true)
            {
                statusText.text = "Connecting.";
                yield return new WaitForSeconds(0.5f);
                statusText.text = "Connecting..";
                yield return new WaitForSeconds(0.5f);
                statusText.text = "Connecting...";
                yield return new WaitForSeconds(0.5f);
                statusText.text = "Connecting....";
                yield return new WaitForSeconds(0.5f);
                statusText.text = "Connecting.....";
                yield return new WaitForSeconds(0.5f);
                statusText.text = "Connecting......";
            }
        }
        private IEnumerator RotateImage()
        {
            while (true)
            {
                rotationImage.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}

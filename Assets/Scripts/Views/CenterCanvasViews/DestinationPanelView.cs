using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
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
       


        private void OnEnable()
        {
            DestinationSuccessEvent += OnDestinationSuccess;
            DestinationFailEvent += OnDestinationFail;
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

    
        public void ConnectToDestination(Destination d)
        {
            Initialise();
            currentDestination = d;
            if (connectionRoutine != null)
            {
                StopCoroutine(connectionRoutine);
                StopCoroutine(rotationRoutine);
            }
            connectionRoutine = StartCoroutine(ConnectingToDestination());
            rotationRoutine = StartCoroutine(RotateImage());
            destinationData.ConnectToDestination(currentDestination, DestinationSuccessEvent, DestinationFailEvent);
           
        }

        public void RetryBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
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
            uiInputData.GoToHome();
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
           if ((currentDestination.gameType == GameType.OnlineMultiPlayer || currentDestination.gameType == GameType.PlayWithFriends))
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

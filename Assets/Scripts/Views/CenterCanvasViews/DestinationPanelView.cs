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
        public GameObject RetryButton;
        public GameObject HomeButton;

        public string joinLobbyState;

        // Events
        public Action DestinationSuccessEvent;
        public Action DestinationFailEvent;


        // local variables
        private Coroutine connectingCoroutine;
        public Destination currentDestination;


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
            if (connectingCoroutine != null)
            {
                StopCoroutine(connectingCoroutine);
            }
            connectingCoroutine = StartCoroutine(ConnectingToDestination());
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
            if (connectingCoroutine != null)
            {
                StopCoroutine(connectingCoroutine);
                connectingCoroutine = null;
            }
            uiInputData.GoToHome();
        }

        private void OnDestinationSuccess()
        {
            Debug.Log("Destination change successful.");

            if (connectingCoroutine != null)
            {
                StopCoroutine(connectingCoroutine);
                connectingCoroutine = null;
            }
            // Additional logic for successful destination change can be added here.
           if ((currentDestination.gameType == GameType.OnlineMultiPlayer || currentDestination.gameType == GameType.PlayWithFriends))
            {
                uiData.uiManager.ChangeState(joinLobbyState, true);
            }

        }

        private void OnDestinationFail()
        {
           

            if (connectingCoroutine != null)
            {
                StopCoroutine(connectingCoroutine);
                connectingCoroutine = null;
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
    }
}

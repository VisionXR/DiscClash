using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class ChangeDestination : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public DestinationDataSO destinationData;
        public UIInputDataSO uiInputData;

        [Header("UI Elements")]
        public TMP_Text statusText;
        public GameObject RetryButton;
        public GameObject HomeButton;

        [Header("Panel Objects")]
        public GameObject ScorePanel2Players;
        public GameObject ScorePanel4Players;
        public GameObject waitingPanel2Players;
        public GameObject waitingPanel4Players;
      

        // Events
        public Action DestinationSuccessEvent;
        public Action DestinationFailEvent;


        // local variables
        private Coroutine connectingCoroutine;
        private Destination currentDestination;


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

            if (currentDestination.gameType == GameType.SinglePlayer)
            {
                if (currentDestination.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
                {
                    ScorePanel2Players.SetActive(true);

                }
                else
                {

                    ScorePanel4Players.SetActive(true);
                }
            }
            else if ((currentDestination.gameType == GameType.OnlineMultiPlayer || currentDestination.gameType == GameType.PlayWithFriends))
            {
                if (currentDestination.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
                {
                    waitingPanel2Players.SetActive(true);

                }
                else
                {

                    waitingPanel4Players.SetActive(true);
                }
            }

            gameObject.SetActive(false);
        }

        private void OnDestinationFail()
        {
            Debug.LogError("Destination change failed.");

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

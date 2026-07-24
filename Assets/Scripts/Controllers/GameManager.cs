using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public InputDataSO inputData;
        public CloudDataSO cloudData;
        public DestinationDataSO destinationData;
        public GameDataSO gameData;
        

        [Header("Game Objects")]
        public GameObject SinglePlayerManager;
        public GameObject MultiPlayerManager;
        public GameObject TutorialManager;

        // Events
        public Action OnCoinFetchSuccessEvent;
        public Action OnCoinFetchFailureEvent;

        public Action OnDestinationSuccessEvent;
        public Action OnDestinationFailureEvent;

        private void OnEnable()
        {
            uiInputData.StartSinglePlayerGameEvent += StartSinglePlayer;
            uiInputData.StartMultiPlayerGameEvent += StartMultiPlayer;
            uiInputData.StartTutorialEvent += StartTutorial;

            uiInputData.ExitGameEvent += StopGame;
            uiInputData.HomeEvent += StopGame;

        }

        private void OnDisable()
        {
            uiInputData.StartSinglePlayerGameEvent -= StartSinglePlayer;
            uiInputData.StartMultiPlayerGameEvent -= StartMultiPlayer;
            uiInputData.StartTutorialEvent -= StartTutorial;

            uiInputData.ExitGameEvent += StopGame;
            uiInputData.HomeEvent += StopGame;

        }

    
        private void StartSinglePlayer()
        {
            ResetManagers();
            SinglePlayerManager.SetActive(true);

            int id = 1;
            if(gameData.firstTurnId == 1)
            {
                id = 2;
            }

            SinglePlayerManager.GetComponent<SinglePlayerGameManager>().StartGame(id);
        }

        private void StartMultiPlayer()
        {
            ResetManagers();
            MultiPlayerManager.SetActive(true);
        }


        private void StartTutorial()
        {
            ResetManagers();
            TutorialManager.SetActive(true);
          //  TutorialManager.GetComponent<TutorialManager>().StartTutorial();
        }

        private void StopGame()
        {
            ResetManagers();
        }

        private void ResetManagers()
        {
            SinglePlayerManager.SetActive(false);
            MultiPlayerManager.SetActive(false);
            TutorialManager.SetActive(false);

            gameData.SetFirstTurnId(-1);
        }                           

    }
}

                    
                            
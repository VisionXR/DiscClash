using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public InputDataSO inputData;
        public OculusDataSO oculusData;

        [Header("Game Objects")]
        public GameObject SinglePlayerManager;
        public GameObject MultiPlayerManager;
        public GameObject TutorialManager;
        public GameObject TrickShotsManager;


        private void OnEnable()
        {
            uiOutputData.StartSinglePlayerGameEvent += StartSinglePlayer;
            uiOutputData.StartMultiPlayerGameEvent += StartMultiPlayer;
            uiOutputData.StartTutorialEvent += StartTutorial;
            uiOutputData.StartFTUEEvent += StartFTUE;
            uiOutputData.StartTrickShotsEvent += StartTrickShots;
          

            uiOutputData.ExitGameEvent += StopGame;
            uiOutputData.HomeEvent += StopGame;
        }

        private void OnDisable()
        {
            uiOutputData.StartSinglePlayerGameEvent -= StartSinglePlayer; 
            uiOutputData.StartMultiPlayerGameEvent -= StartMultiPlayer;
            uiOutputData.StartTutorialEvent -= StartTutorial;
            uiOutputData.StartTrickShotsEvent -= StartTrickShots;
            uiOutputData.StartFTUEEvent -= StartFTUE;

            uiOutputData.ExitGameEvent += StopGame;
            uiOutputData.HomeEvent += StopGame;
        }


        private void StartSinglePlayer()
        {
            ResetManagers();
            SinglePlayerManager.SetActive(true);
        }

        private void StartMultiPlayer()
        {
            ResetManagers();
            MultiPlayerManager.SetActive(true);
        }


        private void StartTrickShots()
        {
            ResetManagers();
            TrickShotsManager.SetActive(true);
        }
        private void StartTutorial()
        {
            ResetManagers();
            TutorialManager.SetActive(true);
            TutorialManager.GetComponent<TutorialManager>().StartTutorial();
        }

        private void StartFTUE(HelperClasses.Destination destination)
        {
            ResetManagers();
            TutorialManager.SetActive(true);
            TutorialManager.GetComponent<TutorialManager>().StartFTUE(destination);
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
            TrickShotsManager.SetActive(false);
        }                           

    }
}

                    
                            
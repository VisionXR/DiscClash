using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace com.VisionXR.Controllers
{
    public class TutorialManager : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public TutorialDataSO tutorialData;
        public InputDataSO inputData;
        public CoinDataSO coinData;
        public StrikerDataSO strikerData;
        public CamPositionSO camPositionData;
        public BoardPropertiesSO boardProperties;
        public MyPlayerSettings myPlayerSettings;

        // An array of Scriptable Objects for each tutorial step
        public TutorialInput tutorialInput;
        public TutorialStep[] tutorialStepsController;
        public TutorialStep[] tutorialStepsHand;
        public int currentStepIndex = 0;
        public TutorialStep currentStep;

        // Reference for striker ,glow striker, coin and holes
        [Header("Objects for turtorial")]
        public TutorialCanvasUIManager tutorialCanvasUIManager;
        public GameObject Board;
        public GameObject Striker;
        public GameObject GlowStriker;
        public GameObject GlowArrow;
        public GameObject BlackCoin;
        public GameObject WhiteCoin;
        public GameObject RedCoin;
        public bool isCoinPocked = false;


        public IStrikerMovement strikerMovement;
        public Destination NewDestination;
        private bool isFTUE = false;


        private void OnEnable()
        {
            uiOutputData.ExitGameEvent += SkipButtonClicked;
            uiOutputData.SetIsPlaying(true);
       
        }

        private void OnDisable()
        {
            uiOutputData.ExitGameEvent -= SkipButtonClicked;
            uiOutputData.SetIsPlaying(false);
        }

        public void StartFTUE(Destination d)
        {
            isFTUE = true;
            NewDestination = d;
            StartTutorial();
        }

        public void StartTutorial()
        {
         
            currentStepIndex = 0;
            Board.SetActive(true);
            boardProperties.TurnOnHoles();

            tutorialCanvasUIManager.Reset();


            strikerMovement = Striker.GetComponent<IStrikerMovement>();
            strikerMovement.SetStrikerID(1);

            Invoke("DisplayCurrentStep", 1f);
            AudioManager.instance.backgroundMusicAS.mute = true;
            tutorialData.CheckPositionEvent += OnPositionLocked;
            tutorialData.CheckAimEvent += OnAimingLocked;
            Striker.GetComponent<StrikerShooting>().StrikeStartedEvent += StrikeStarted;
            Striker.GetComponent<StrikerShooting>().StrikeFinishedEvent += StrikeFinished;
            coinData.CoinpocketedUntoHoleEvent += OnCoinFellIntoHole;
           

            camPositionData.SetCamPosition(1);
            tutorialData.ResetVariables();

            tutorialInput.RegisterEvents();

        }

        private void EndTutorial()
        {
           
            tutorialData.CheckPositionEvent -= OnPositionLocked;
            tutorialData.CheckAimEvent -= OnAimingLocked;     
            coinData.CoinpocketedUntoHoleEvent -= OnCoinFellIntoHole;
            Striker.GetComponent<StrikerShooting>().StrikeStartedEvent -= StrikeStarted;
            Striker.GetComponent<StrikerShooting>().StrikeFinishedEvent -= StrikeFinished;
            tutorialInput.DeRegisterEvents();
        }

        private void DisplayCurrentStep()
        {

            Striker.SetActive(false);
            BlackCoin.SetActive(false);
            WhiteCoin.SetActive(false);
            RedCoin.SetActive(false);
            // ... your existing code to display the step ...

            if (!inputData.isHandTrackingActive)
            {
                currentStep = tutorialStepsController[currentStepIndex];
            }
            else
            {
                currentStep = tutorialStepsHand[currentStepIndex];
            }

            tutorialCanvasUIManager.StepText.text = "Step : " + currentStep.stepNumber + " / "+tutorialStepsController.Length;
            tutorialCanvasUIManager.ContentText.text = currentStep.stepText;

            if (currentStep.stepAudio != null)
            {
                tutorialCanvasUIManager.audioSource.clip = currentStep.stepAudio;
                tutorialCanvasUIManager.audioSource.Play();
            }

            if (currentStep.stepVideo != null)
            {
               
                tutorialCanvasUIManager.videoPlayer.clip = currentStep.stepVideo;
                tutorialCanvasUIManager.videoPlayer.Play();
            }

            if (currentStep.interactiveStepType == InteractiveStepType.None)
            {
                StartCoroutine(WaitForStepToComplete(currentStep.stepAudio.length));
            }
            else
            {
                inputData.ActivateInput();
                switch (currentStep.interactiveStepType)
                {
                    case InteractiveStepType.Positioning:
                        SetUpPositioning();
                        break;
                    case InteractiveStepType.Aiming:
                        SetUpAiming();
                        break;
                    case InteractiveStepType.Striking:
                        SetUpStriking();
                        break;
                    case InteractiveStepType.Display:
                        SetUpDisplay();
                        break;
                }
            }
        }
        private IEnumerator WaitForStepToComplete(float t)
        {

            yield return new WaitForSeconds(t);
            tutorialCanvasUIManager.SuccessFailureText.text = "";
            tutorialCanvasUIManager.NextButton.SetActive(true);

            if (currentStepIndex == tutorialStepsController.Length - 1)
            {
                tutorialCanvasUIManager.NextButton.SetActive(false);
                tutorialCanvasUIManager.PlayButton.SetActive(true);
            }
        }
        private void SetUpPositioning()
        {
            // Enable user's striker, place a glowing striker

            Striker.SetActive(true);

            strikerMovement.ResetStriker();
            tutorialData.SetCanIPosition(true);
           
            GlowStriker.SetActive(true);
            GlowStriker.transform.position = currentStep.strikerPosition;
            inputData.ActivateInput();

        }

        private void SetUpAiming()
        {
            // Enable user's striker, place a glowing striker with an arrow direction
            strikerMovement.ResetStriker();
            tutorialData.SetCanIPosition(true);
            tutorialData.SetCanIAim(true);
            Striker.SetActive(true);
            GlowStriker.SetActive(true);
            GlowStriker.transform.position = currentStep.aimingStrikerPosition;
            GlowStriker.transform.eulerAngles = currentStep.aimingStrikerRotation;
            GlowArrow.SetActive(true);
            inputData.ActivateInput();
           
        }

        private void SetUpStriking()
        {
            // Enable user's striker, place a glowing striker with an arrow direction
            strikerMovement.ResetStriker();
            tutorialData.SetCanIPosition(true);
            tutorialData.SetCanIAim(true);
            tutorialData.SetCanIFire(true);
            Striker.SetActive(true);

            GlowStriker.SetActive(true);
            GlowStriker.transform.position = currentStep.strikingStrikerPosition;
            GlowStriker.transform.eulerAngles = currentStep.strikingStrikerRotation;
            GlowArrow.SetActive(true);
            BlackCoin.SetActive(true);
            BlackCoin.transform.position = currentStep.coinPosition;
            BlackCoin.transform.rotation = Quaternion.identity;
            isCoinPocked = false;
            inputData.ActivateInput();
           
        }

        private void SetUpDisplay()
        {
            BlackCoin.transform.position = currentStep.coinPosition;
            Striker.SetActive(true);
            BlackCoin.SetActive(true);
            WhiteCoin.SetActive(true);
            RedCoin.SetActive(true);
            StartCoroutine(WaitForStepToComplete(currentStep.stepAudio.length));
        }


        public void OnPositionLocked()
        {
            tutorialData.SetCanIPosition(false);
            inputData.DeactivateInput();
            Striker.SetActive(false);
            GlowStriker.SetActive(false);
            // success
            if (Vector3.Distance(Striker.transform.position, GlowStriker.transform.position) < 0.05f)
            {

                tutorialCanvasUIManager.SuccessFailureText.text = currentStep.successText;
                if (currentStep.successAudio != null)
                {
                    tutorialCanvasUIManager.audioSource.clip = currentStep.successAudio;
                    tutorialCanvasUIManager.audioSource.Play();
                }
                StartCoroutine(WaitForStepToComplete(1));
            }
            else  // failure try again
            {
                tutorialCanvasUIManager.SuccessFailureText.text = currentStep.failureText;

                if (currentStep.failureAudio != null)
                {
                    tutorialCanvasUIManager.audioSource.clip = currentStep.failureAudio;
                    tutorialCanvasUIManager.audioSource.Play();
                }
                Invoke("ResetCurrentStep", 2);
            }
        }

        public void OnAimingLocked()
        {
            tutorialData.SetCanIPosition(false);
            tutorialData.SetCanIAim(false);
            inputData.DeactivateInput();
            Striker.SetActive(false);
            GlowStriker.SetActive(false);
            GlowArrow.SetActive(false);
            // success
            if (Vector3.Distance(Striker.transform.position, GlowStriker.transform.position) < 0.05f && (Mathf.Abs(Striker.transform.eulerAngles.y - GlowStriker.transform.eulerAngles.y)) < 2f)
            {

                tutorialCanvasUIManager.SuccessFailureText.text = currentStep.successText;
                if (currentStep.successAudio != null)
                {
                    tutorialCanvasUIManager.audioSource.clip = currentStep.successAudio;
                    tutorialCanvasUIManager.audioSource.Play();
                }
                StartCoroutine(WaitForStepToComplete(1));
            }
            else  // failure try again
            {
                tutorialCanvasUIManager.SuccessFailureText.text = currentStep.failureText;

                if (currentStep.failureAudio != null)
                {
                    tutorialCanvasUIManager.audioSource.clip = currentStep.failureAudio;
                    tutorialCanvasUIManager.audioSource.Play();
                }
                Invoke("ResetCurrentStep", 2);
            }
        }

        private void StrikeStarted(float force,Vector3 dir)
        {
            inputData.DeactivateInput();
        }
        public void StrikeFinished()
        {
            tutorialData.SetCanIPosition(true);
            tutorialData.SetCanIAim(true);
            tutorialData.SetCanIFire(true);
            inputData.DeactivateInput();
            // success
            if (isCoinPocked)
            {
                inputData.DeactivateInput();
                tutorialCanvasUIManager.SuccessFailureText.text = currentStep.successText;
                if (currentStep.successAudio != null)
                {
                    tutorialCanvasUIManager.audioSource.clip = currentStep.successAudio;
                    tutorialCanvasUIManager.audioSource.Play();
                }
                StartCoroutine(WaitForStepToComplete(1));
            }
            else // failure
            {
                tutorialCanvasUIManager.SuccessFailureText.text = currentStep.failureText;

                if (currentStep.failureAudio != null)
                {
                    tutorialCanvasUIManager.audioSource.clip = currentStep.failureAudio;
                    tutorialCanvasUIManager.audioSource.Play();
                }
                Invoke("ResetCurrentStep", 2);
            }

            Striker.SetActive(false);
            GlowStriker.SetActive(false);
            GlowArrow.SetActive(false);          
            BlackCoin.SetActive(false);
        }

        public void OnCoinFellIntoHole(GameObject hole)
        {
            if (hole.name == currentStep.holeName)
            {
                isCoinPocked = true;
            }
        }
        private void ResetCurrentStep()
        {
            TutorialStep currentStep;
            Striker.SetActive(false);
            BlackCoin.SetActive(false);
            WhiteCoin.SetActive(false);
            RedCoin.SetActive(false);
            if (inputData.isHandTrackingActive)
            {
                currentStep = tutorialStepsController[currentStepIndex];
            }
            else
            {
                currentStep = tutorialStepsHand[currentStepIndex];
            }
            tutorialCanvasUIManager.SuccessFailureText.text = "";
            switch (currentStep.interactiveStepType)
            {
                case InteractiveStepType.Positioning:
                    SetUpPositioning();
                    break;
                case InteractiveStepType.Aiming:
                    SetUpAiming();
                    break;
                case InteractiveStepType.Striking:
                    SetUpStriking();
                    break;
            }
        }

        // Method to proceed to the next step.
        public void NextStep()
        {
            AudioManager.instance.PlayButtonClickSound();
            if (currentStepIndex < tutorialStepsController.Length - 1)
            {
                tutorialCanvasUIManager.NextButton.SetActive(false);
                currentStepIndex++;
                DisplayCurrentStep();
            }
            else if (currentStepIndex == tutorialStepsController.Length - 1)
            {
                tutorialCanvasUIManager.NextButton.SetActive(false);
                tutorialCanvasUIManager.PlayButton.SetActive(true);
            }
        }

        public void PlayButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
        
            Board.SetActive(false);
            Striker.SetActive(false);
            GlowStriker.SetActive(false);
            GlowArrow.SetActive(false);

            BlackCoin.SetActive(false);
            WhiteCoin.SetActive(false);
            RedCoin.SetActive(false);

            tutorialData.SetCanIPosition(false);
            tutorialData.SetCanIAim(false);
            tutorialData.SetCanIFire(false);

            AudioManager.instance.backgroundMusicAS.mute = false;
            inputData.DeactivateInput();

            EndTutorial();
            uiOutputData.EndTutorial();
          

            if (isFTUE)
            {
                isFTUE = false;
                uiInputData.ConnectToDestination(NewDestination);
            }

            gameObject.SetActive(false);
        }

        public void SkipButtonClicked()
        {
            
            AudioManager.instance.PlayButtonClickSound();
     
            Board.SetActive(false);
            Striker.SetActive(false);
            GlowStriker.SetActive(false);
            GlowArrow.SetActive(false);

            BlackCoin.SetActive(false);
            WhiteCoin.SetActive(false);
            RedCoin.SetActive(false);

            tutorialData.SetCanIPosition(false);
            tutorialData.SetCanIAim(false);
            tutorialData.SetCanIFire(false);

            AudioManager.instance.backgroundMusicAS.mute = false;
            inputData.DeactivateInput();

            EndTutorial();
            uiOutputData.EndTutorial();
         

            if (isFTUE)
            {
                isFTUE= false;
                uiInputData.ConnectToDestination(NewDestination);
            }

            gameObject.SetActive(false);
        }
    }
}

using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class TutorialManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public List<TutorialStep> tutorialSteps;
        public TutorialDataSO tutorialData;
        public BoardDataSO boardData;
        public InputDataSO inputData;
        public CoinDataSO coinData;
        public StrikerDataSO strikerData;
        public UIDataSO uiData;


        [Header("Game Objects")]
        public TutorialPanelView tutorialPanelView;
        public GameObject tutorialBoard;
        public GameObject tutorialStriker;
        public GameObject tutorialCoin;
        public InputCanvasView inputCanvasView;

        [Header("Local Variables")]
        public Vector3 strikerInitPosition;
        public BoardManager boardManager;
        public StrikerArrow strikerArrow;
        public TutorialStep currentStep;
        private Coroutine _tutorialRoutine;
        private bool _stepCompleted;
        private bool _tutorialSkipped;
        private int _currentStepIndex = -1;
        private bool isCoinPocketed = false;

        private void OnEnable()
        {
            Reset();
            tutorialData.NextBtnClcikedEvent += NextBtnClicked;
            tutorialData.SkipBtnClcikedEvent += SkipBtnClicked;
            tutorialData.PlayBtnClickedEvent += PlayBtnClicked;

            //inputData.RotationPinchStartedEvent += PlatformRotationStarted;
            //inputData.RotationPinchEndedEvent += PlatformRotationEnded;

            //inputData.SwipeCompleteEvent += SwipeCompeted;

            //coinData.CoinPocketedEvent += CoinPocketed;

            //strikerData.StrikerStartedEvent += StrikeStarted;
            //strikerData.StrikerStoppedEvent += StrikeCompleted;


            boardManager.StartTutorial();
            tutorialBoard.SetActive(true);
            StartCoroutine(RunTutorialSteps());
        }

        private void OnDisable()
        {
            tutorialData.NextBtnClcikedEvent -= NextBtnClicked;
            tutorialData.SkipBtnClcikedEvent -= SkipBtnClicked;
            tutorialData.PlayBtnClickedEvent -= PlayBtnClicked;


            //inputData.RotationPinchStartedEvent -= PlatformRotationStarted;
            //inputData.RotationPinchEndedEvent -= PlatformRotationEnded;

            //inputData.SwipeCompleteEvent -= SwipeCompeted;

            //coinData.CoinPocketedEvent -= CoinPocketed;
            //strikerData.StrikerStoppedEvent -= StrikeCompleted;
            //strikerData.StrikerStartedEvent -= StrikeStarted;


            boardManager.EndTutorial();
        }

        private void Reset()
        {
            if (_tutorialRoutine != null)
            {
                StopCoroutine(_tutorialRoutine);
                _tutorialRoutine = null;
            }

            tutorialBoard.SetActive(false);
            tutorialStriker.SetActive(false);

            tutorialCoin.SetActive(false);
            inputData.DisableInput();
            tutorialData.canIAim = false;
            tutorialData.canIFire = false;
            isCoinPocketed = false;
            _tutorialSkipped = false;
            _stepCompleted = false;
            _currentStepIndex = -1;
        }

        private void CoinPocketed(GameObject coin)
        {
            tutorialCoin.GetComponent<Rigidbody>().isKinematic = true;
            isCoinPocketed = true;
        }

        // Call this to start running the steps
        private void PlayBtnClicked()
        {
            Reset();
            uiData.uiManager.ChangeState("Tutorial", false);
            gameObject.SetActive(false);
        }

        private void NextBtnClicked()
        {
            // Force-advance the current step
            _stepCompleted = true;
        }

        private void SkipBtnClicked()
        {

            Reset();
            uiData.uiManager.ChangeState("Tutorial", false);
            gameObject.SetActive(false);
        }

        private IEnumerator RunTutorialSteps()
        {
            yield return new WaitForSeconds(1);


            for (int i = 0; i < tutorialSteps.Count; i++)
            {
                if (_tutorialSkipped)
                {
                    break;
                }

                _currentStepIndex = i;
                _stepCompleted = false;

                currentStep = tutorialSteps[i];


                // 1. Check if the audio clip actually EXISTS in Unity's memory
                float audioDuration = 10f;

                // Using explicit false check against true null/destroyed state
                if (tutorialSteps[i].stepAudio)
                {
                    audioDuration = tutorialSteps[i].stepAudio.length;
                }

                tutorialData.ShowTutorialStep(
                    i + 1,
                    tutorialSteps[i].stepText,
                    tutorialSteps[i].stepAudio,
                    tutorialSteps[i].interactiveStepType,
                    audioDuration
                );


                if (currentStep.interactiveStepType == InteractiveStepType.Positioning)
                {
                   
                    inputData.EnableInput();
                }

                else if (currentStep.interactiveStepType == InteractiveStepType.Aiming)
                {
                    
                    tutorialStriker.SetActive(true);
                    tutorialStriker.transform.localPosition = strikerInitPosition;
                    tutorialStriker.transform.localEulerAngles = Vector3.zero;


                    strikerArrow.TurnOnArrow();
                    tutorialData.canIAim = true;
                }

                else if (currentStep.interactiveStepType == InteractiveStepType.Striking)
                {

                    isCoinPocketed = false;
                    strikerArrow.TurnOnArrow();
                    tutorialStriker.SetActive(true);
                    tutorialStriker.transform.localPosition = strikerInitPosition;
                    tutorialStriker.transform.localEulerAngles = Vector3.zero;

                    inputData.EnableInput();
                    tutorialData.canIAim = true;
                    tutorialData.canIFire = true;
                    tutorialCoin.SetActive(true);

                    inputCanvasView.TurnOn();

                    tutorialCoin.GetComponent<Rigidbody>().isKinematic = false;
          
                    tutorialStriker.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    tutorialStriker.transform.localPosition = strikerInitPosition;
                    tutorialStriker.transform.localEulerAngles = Vector3.zero;

                    tutorialCoin.GetComponent<Rigidbody>().isKinematic = false;
                    tutorialCoin.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    tutorialCoin.transform.localRotation = Quaternion.identity;
                    tutorialCoin.transform.localPosition = currentStep.coinPosition;
                }

                else
                {
                    tutorialStriker.SetActive(false);
                }


                // Wait until this step is marked as completed or tutorial is skipped
                yield return new WaitUntil(() => _stepCompleted || _tutorialSkipped);

                // tutorialSteps[i].EndStep();
            }

            _currentStepIndex = -1;
            _tutorialRoutine = null;


        }


        private void PlatformRotationStarted(Vector2 input)
        {
            if (currentStep != null && currentStep.interactiveStepType == InteractiveStepType.Positioning)
            {
                tutorialPanelView.ResetObjects();
            }
        }

        private void PlatformRotationEnded(Vector2 input)
        {
            if (currentStep != null && currentStep.interactiveStepType == InteractiveStepType.Positioning)
            {

                tutorialData.ShowTutorialStepSuccess(currentStep.successText, currentStep.successAudio);

            }
        }


        private void SwipeCompeted(float val)
        {

            if (currentStep != null && currentStep.interactiveStepType == InteractiveStepType.Aiming)
            {

                // Aimed properly
                tutorialData.ShowTutorialStepSuccess(currentStep.successText, currentStep.successAudio);
                tutorialStriker.SetActive(false);

                inputData.DisableInput();
                tutorialData.canIAim = false;

            }
        }

        private void StrikeStarted()
        {

            inputCanvasView.TurnOff();
        }

        private void StrikeCompleted()
        {
            if (currentStep != null && currentStep.interactiveStepType == InteractiveStepType.Striking)
            {
                if (isCoinPocketed)
                {
                    // Aimed properly
                    tutorialData.ShowTutorialStepSuccess(currentStep.successText, currentStep.successAudio);

                    tutorialStriker.transform.localEulerAngles = Vector3.zero;
                    tutorialStriker.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    tutorialStriker.transform.localPosition = strikerInitPosition;
                    tutorialStriker.SetActive(false);


                    tutorialCoin.GetComponent<Rigidbody>().isKinematic = false;
              
                    tutorialCoin.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    tutorialCoin.transform.localPosition = currentStep.coinPosition;
                    tutorialCoin.transform.localEulerAngles = Vector3.zero;
                    tutorialCoin.SetActive(false);


                    inputData.DisableInput();
                    tutorialData.canIAim = false;
                    tutorialData.canIFire = false;
                }
                else
                {
                    inputData.EnableInput();
                    strikerArrow.TurnOnArrow();

                    tutorialData.ShowTutorialStepFailed(currentStep.failureText, currentStep.failureAudio);

                    tutorialCoin.GetComponent<Rigidbody>().isKinematic = false;
               
                    tutorialCoin.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    tutorialCoin.transform.localPosition = currentStep.coinPosition;
                    tutorialCoin.transform.localEulerAngles = Vector3.zero;

         
                    tutorialStriker.transform.localEulerAngles = Vector3.zero;
                    tutorialStriker.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    tutorialStriker.GetComponent<Rigidbody>().isKinematic = false;
                    tutorialStriker.transform.localPosition = strikerInitPosition;

                    inputCanvasView.TurnOn();

                }

            }

        }

    }
}
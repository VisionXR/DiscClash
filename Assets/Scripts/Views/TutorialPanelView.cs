using com.VisionXR.ModelClasses;
using System.Collections;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class TutorialPanelView : MonoBehaviour
    {

        [Header("Scriptable Objects")]
       
        public TutorialDataSO tutorialData;

        [Header("UI Objects")]
        public TMP_Text StepNumberText;
        public TMP_Text SuccessFailurerText;
        public TMP_Text ContentText;
        public AudioSource audioSource;

        public GameObject NextBtn;
        public GameObject PlayBtn;

        [Header("Animations Objects")]
        public GameObject CentralUI;
        public GameObject BoardRotationObject;
        public GameObject AimObject;
        public GameObject PocketCoinObject;
        public Animator swipeAnimator;


        private void OnEnable()
        {
                  
            Reset();
            tutorialData.ShowTutorialStepEvent += ShowTutorialStep;
            tutorialData.ShowTutorialStepSuccessEvent += ShowTutorialStepSuccess;
            tutorialData.ShowTutorialStepFailedEvent += ShowTutorialStepFailed;
        }

        private void OnDisable()
        {
            tutorialData.ShowTutorialStepEvent -= ShowTutorialStep;
            tutorialData.ShowTutorialStepSuccessEvent -= ShowTutorialStepSuccess;
            tutorialData.ShowTutorialStepFailedEvent -= ShowTutorialStepFailed;
        }

        private void Reset()
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }

            ContentText.text = "";
            SuccessFailurerText.text = "";
            NextBtn.SetActive(false);
            PlayBtn.SetActive(false);
        }

        private void ShowTutorialStepFailed(string content, AudioClip clip)
        {
            SuccessFailurerText.text = content;
            ContentText.text = "";
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        private void ShowTutorialStepSuccess(string content, AudioClip clip)
        {
            ResetObjects();
            CentralUI.SetActive(true);
            SuccessFailurerText.text = content;
            ContentText.text = "";

            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }

            NextBtn.SetActive(true);
        }

        private void ShowTutorialStep(int stepNumber, string contentText, AudioClip audioClip,  InteractiveStepType stepType)
        {
            StepNumberText.text = "Step " + stepNumber + "/" + tutorialData.totalSteps;
            SuccessFailurerText.text = "";
            ContentText.text = contentText;
            NextBtn.SetActive(false);
            PlayBtn.SetActive(false);
            ResetObjects();
            ResetStates();


            if (stepType == InteractiveStepType.None)
            {
                CentralUI.SetActive(true);

                if (stepNumber == 1)
                {
                    StartCoroutine(WaitAndShowNextBtn(5f));
                }
                else if (stepNumber == 5)
                {
                    StartCoroutine(WaitAndShowPlayBtn(5f));
                }
            }
            else
            {
                CentralUI.SetActive(false);

            }


            if(stepType == InteractiveStepType.Positioning)
            {
               BoardRotationObject.SetActive(true);
               swipeAnimator.SetBool("BoardRotation", true);
               //swipeAnimator.SetBool("BoardHandAnimation", true);
            }

            if (stepType == InteractiveStepType.Aiming)
            {
                AimObject.SetActive(true);
                swipeAnimator.SetBool("LeftText", true);    
                //swipeAnimator.SetBool("LeftHand", true);    
            }

            if (stepType == InteractiveStepType.Striking)
            {
                PocketCoinObject.SetActive(true);
                swipeAnimator.SetBool("PocketCoin", true);  
            }


            if (audioClip != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }

        }

        private IEnumerator WaitAndShowNextBtn(float time)
        {
            yield return new WaitForSeconds(time);
            NextBtn.SetActive(true);
        }

        private IEnumerator WaitAndShowPlayBtn(float time)
        {
            yield return new WaitForSeconds(time);
            PlayBtn.SetActive(true);
        }

        public void NextBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            tutorialData.NextBtnClcikedEvent?.Invoke();
            SuccessFailurerText.text = "";
            NextBtn.SetActive(false);
        }

        public void PlayBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            tutorialData.PlayBtnClickedEvent?.Invoke();
            PlayBtn.SetActive(false);
        }

        public void SkipBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            tutorialData.SkipBtnClcikedEvent?.Invoke();
        }


        public void ResetObjects()
        {
            BoardRotationObject.SetActive(false);
            AimObject.SetActive(false);
            PocketCoinObject.SetActive(false);
            CentralUI.SetActive(false);
        }

        public void ResetStates()
        {
            swipeAnimator.SetBool("BoardRotation", false);
           // swipeAnimator.SetBool("BoardHandAnimation", false);
            swipeAnimator.SetBool("LeftText", false);
           // swipeAnimator.SetBool("LeftHand", false);
            swipeAnimator.SetBool("RightText", false);
            swipeAnimator.SetBool("RightHand", false);
            swipeAnimator.SetBool("PocketCoin", false);
        }

    }
}

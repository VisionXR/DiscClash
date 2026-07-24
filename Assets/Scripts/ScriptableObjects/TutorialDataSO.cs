using System;
using UnityEngine;
using UnityEngine.Video;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "TutorialDataSO", menuName = "ScriptableObjects/TutorialDataSO", order = 1)]
    public class TutorialDataSO : ScriptableObject
    {
        // variables

        public bool canIPosition;
        public bool canIAim;
        public bool canIFire;
        public int totalSteps = 5;

        // Events

        public Action CheckPosiitonEvent;
        public Action CheckAimEvent;
        public Action CheckStrikeEvent;


        public Action<int, string, AudioClip, InteractiveStepType> ShowTutorialStepEvent;
        public Action<string, AudioClip> ShowTutorialStepSuccessEvent;
        public Action<string, AudioClip> ShowTutorialStepFailedEvent;
        public Action NextBtnClcikedEvent;
        public Action SkipBtnClcikedEvent;
        public Action PlayBtnClickedEvent;

        public Action ShowNextBtnEvent;

        // Methods

        public void ResetVariables()
        {

            canIPosition = false;
            canIAim = false;
            canIFire = false;
        }

        public void ShowTutorialStep(int stepNumber, string contentText, AudioClip audioClip, InteractiveStepType stepType, float time)
        {
            ShowTutorialStepEvent?.Invoke(stepNumber, contentText, audioClip, stepType);
        }

        public void ShowTutorialStepSuccess(string contentText, AudioClip audioClip)
        {
            ShowTutorialStepSuccessEvent?.Invoke(contentText, audioClip);
        }

        public void ShowTutorialStepFailed(string contentText, AudioClip audioClip)
        {
            ShowTutorialStepFailedEvent?.Invoke(contentText, audioClip);
        }

        public void SetCanIPosition(bool value)
        {
            canIPosition = value;
        }

        public void SetCanIAim(bool value)
        {
            canIAim = value;
        }

        public void SetCanIFire(bool value)
        {
            canIFire = value;
        }

        public void CheckStrikerPosiiton()
        {
            if (canIPosition)
            {
                CheckPosiitonEvent?.Invoke();
            }
        }

        public void CheckAim()
        {
            if (canIAim)
            {
                CheckAimEvent?.Invoke();
            }
        }

        public void CheckStrike()
        {
            if (canIFire)
            {
                CheckStrikeEvent?.Invoke();
            }
        }

        public void ShowNextButton()
        {
            ShowNextBtnEvent?.Invoke();
        }
    }
}

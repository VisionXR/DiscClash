using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIDataSO uiData;
        public Animator uiController;
        public StateName currentStateName;
        public StateName previousStateName;

        [Header("All Canvas Views")]
        public MainCanvasView mainCanvasView;

        [Header("Canvas Objects")]
        public List<GameObject> allCanvases;


        private Coroutine hideRoutine;
        


        private void Start()
        {
          
          
            uiData.SetUIMachine(this);
            uiController.enabled = true;

        }

        private void OnEnable()
        {
            ResetAllBools();
        }

        public void SetCurrentStateName(StateName stateName)
        {
            currentStateName = stateName;
        }

        public void SetPreviousStateName(StateName stateName)
        {
            previousStateName = stateName;
        }

        public void ChangeState(string stateVariable,bool value)
        {
            uiController.SetBool(stateVariable, value);
           
        }

        public void GoToState(StateName newStateName)
        {
          
            string name = Enum.GetName(typeof(StateName),newStateName);
            uiData.uiManager.uiController.Play(name);
        }

        private IEnumerator WaitAndReset()
        {
            yield return new WaitForSeconds(uiData.disableTime);
            // Iterate through all parameters in the Animator Controller
            foreach (AnimatorControllerParameter parameter in uiController.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool && parameter.name != "GameType")
                {
                    uiController.SetBool(parameter.name, false);
                }
            }

        }
        /// <summary>
        /// Loops through all parameters and sets every Boolean to false.
        /// </summary>
        public void ResetAllBools()
        {
            StartCoroutine(WaitAndReset());
        }

        public void ShowCanvas(int id)
        {
            if(hideRoutine != null)
            {
               
                StopCoroutine(hideRoutine);
                hideRoutine = null;
            }

            allCanvases[id].SetActive(true);
        }

        public void HideCanvas(int id)
        {
            if (hideRoutine == null)
            {
               hideRoutine =  StartCoroutine(WaitAndHide(id));
            }
        }

        private IEnumerator WaitAndHide(int id)
        {
            yield return new WaitForSeconds(uiData.disableTime);
            allCanvases[id].SetActive(false);
            hideRoutine = null;
        }


        private void ResetCanvases()
        {     
            foreach(GameObject canvas in allCanvases)
            {
                canvas.SetActive(false);
            }
        }

    }
}

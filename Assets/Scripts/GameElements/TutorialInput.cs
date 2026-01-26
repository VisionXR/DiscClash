using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;


namespace com.VisionXR.GameElements
{

    public class TutorialInput : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public InputDataSO inputData;
        public TutorialDataSO tutorialData;

        [Header("Game Objects")]
        public GameObject striker;
        private IStrikerMovement strikerMovement;

        public void RegisterEvents()
        {
            strikerMovement = striker.GetComponent<IStrikerMovement>();
         
   
        }

        public void DeRegisterEvents()
        {
          
       
        }



        private void OnTriggerButtonClicked()
        {
            if (tutorialData.canIAim && ! tutorialData.canIFire)
            {
               
                tutorialData.CheckAim();
            }
        }

        private void OnGrabButtonClicked()
        {
            if (tutorialData.canIPosition && !tutorialData.canIAim)
            {
               
                tutorialData.CheckPosition();
            }
        }

        private void OnControllerMoved(Vector3 vector, Transform transform)
        {
            if (tutorialData.canIPosition)
            {
                
            }
        }


        private void OnControllerRotated(float value)
        {
            if (tutorialData.canIAim)
            {
             
            }
           
        }

        private void OnHandRotated(Vector3 pos)
        {
            if (tutorialData.canIAim)
            {
                 
            }
        }

        private void Swiped(SwipeDirection direction)
        {
            if (tutorialData.canIPosition)
            {
            
                
            }
        }

        private void KeyboardRotated(SwipeDirection direction)
        {
            if (tutorialData.canIAim)
            {
               
            }
        }

        private void FireStrikerWithForce(float value)
        {
            if (tutorialData.canIFire)
            {
               
            }
        }
    }
}

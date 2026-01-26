using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;


namespace com.VisionXR.GameElements
{

    public class TrickShotsInput: MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public InputDataSO inputData;
      

        [Header("Game Objects")]
       public TrickShotManager trickShotManager;

        public void OnEnable()
        {
           
          
        }

        public void OnDisable()
        {
                 

        }
        private void OnControllerMoved(Vector3 vector, Transform transform)
        {

            
            
        }
        private void OnControllerRotated(float value)
        {

           
                  
        }
        private void OnHandRotated(Vector3 pos)
        {
          
        }

        private void Swiped(SwipeDirection direction)
        {         
            
        }
        private void KeyboardRotated(SwipeDirection direction)
        {
           
        }

        private void FireStrikerWithForce(float value)
        {
           
        }
    }
}

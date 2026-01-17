using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class CamPositionManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public CamPositionSO camPositionData;
        public BoardPropertiesSO boardProperties;
        public InputDataSO inputData;

        [Header("Game Objects")]
        public GameObject cameraRig;
        public GameObject standingRig;
        public GameObject AllCanvases;
      

        [Header("Local Variables")]
        public float MovementSpeed = 0.005f;
        public float LeftEndPoint = -0.5f;
        public float RightEndPoint = 0.5f;
        public float TopEndPoint = 0.3f;
        public float BottomEndPoint = -0.2f;
        public float ControllerRotationSpeed = 1f; // degrees per input unit
        public float HandRotationSpeed = 10f; // degrees per input unit

        private void OnEnable()
        {
            camPositionData.SetCamPositionEvent += ChangeCamPosition;
            camPositionData.MoveCamUpDownEvent += MoveCamUpDown;
            camPositionData.MoveCamLeftRightEvent += MoveCamLeftRight;
            camPositionData.RotateCamEvent += RotateCamWorldY;
        }

        private void OnDisable()
        {
            camPositionData.SetCamPositionEvent -= ChangeCamPosition;
            camPositionData.MoveCamUpDownEvent -= MoveCamUpDown;
            camPositionData.MoveCamLeftRightEvent -= MoveCamLeftRight;
            camPositionData.RotateCamEvent -= RotateCamWorldY;
        }

        private void MoveCamLeftRight(int id,float value)
        {
            Vector3 centerPosition = boardProperties.GetPlayerPosition(id).position;

            centerPosition = new Vector3(centerPosition.x, cameraRig.transform.position.y, centerPosition.z);

            // Calculate the potential new position
            Vector3 potentialNewPosition = cameraRig.transform.position + standingRig.transform.right * value * MovementSpeed;

            // Calculate the displacement from the center position
            float displacementFromCenter = Vector3.Dot(potentialNewPosition - centerPosition, standingRig.transform.right);

            // Clamp the displacement
            displacementFromCenter = Mathf.Clamp(displacementFromCenter, LeftEndPoint, RightEndPoint);

            // Set the new position based on the clamped displacement
            cameraRig.transform.position = centerPosition + standingRig.transform.right * displacementFromCenter;

            standingRig.transform.position = cameraRig.transform.position;
        }

        private void ChangeCamPosition(int id)
        {

            Debug.Log("Changing Camera Position to Player ID: " + id);

            cameraRig.transform.position = boardProperties.GetPlayerPosition(id).position;
            cameraRig.transform.rotation = boardProperties.GetPlayerPosition(id).rotation;


        }

        public void MoveCamUpDown(int id,float value)
        {
            Vector3 topLimit = boardProperties.GetPlayerPosition(id).position + new Vector3(0, TopEndPoint, 0);
            Vector3 bottomLimit = boardProperties.GetPlayerPosition(id).position + new Vector3(0, BottomEndPoint, 0);
            float newY = cameraRig.transform.position.y + value * MovementSpeed;
            if (newY < topLimit.y && newY > bottomLimit.y)
            {
                cameraRig.transform.position = new Vector3(cameraRig.transform.position.x, newY, cameraRig.transform.position.z);
            }
        }

        // Rotates the cameraRig around the world Y axis about the player's center position.
        // 'value' is an input scalar (e.g. joystick delta). RotationSpeed (degrees per unit)
        // determines how many degrees are applied per call.
        public void RotateCamWorldY(int id, float value)
        {
            if (cameraRig == null || standingRig == null || boardProperties == null)
            {
                return;
            }
            // Calculate rotation angle in degrees
            float angle = 0;
            
            if(inputData.isHandTrackingActive)
            {
                angle = value * HandRotationSpeed;
            }
            else
            {
                angle = value * ControllerRotationSpeed;
            }
        
           // Rotate the cameraRig around the world Y axis through the center position
            cameraRig.transform.Rotate(Vector3.up, angle);          
        }


    }
}

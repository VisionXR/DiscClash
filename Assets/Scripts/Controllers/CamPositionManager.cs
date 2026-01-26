using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class CamPositionManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public CamPositionSO camPositionData;
        public BoardPropertiesSO boardProperties;
        public InputDataSO inputData;
        public PlayersDataSO playerData;

        [Header("Game Objects")]
        public GameObject cameraRig;
        public Transform OriginalPos;

        [Header("Rotation Settings")]
        [Tooltip("Maximum yaw (left/right) offset in degrees from the center rotation.")]
        public float maxYawDegrees = 45f;
        [Tooltip("Maximum pitch (up/down) offset in degrees from the center rotation.")]
        public float maxPitchDegrees = 45f;
        [Tooltip("Degrees to change per swipe.")]
        public float rotationStepDegrees = 15f;
        [Tooltip("If > 0, camera rotation will be smoothed. Higher == faster.")]
        public float rotationLerpSpeed = 0f;

        // Internal state
        private Quaternion _centerRotation = Quaternion.identity;
        private float _currentYawOffset = 0f;   // left/right offset in degrees
        private float _currentPitchOffset = 0f; // up/down offset in degrees
        private Quaternion _targetRotation = Quaternion.identity;

        private void OnEnable()
        {
            camPositionData.SetCamPositionEvent += ChangeCamPosition;
            camPositionData.RotateCamEvent += ChangeCamRotation;
            camPositionData.RecenterEvent += Recenter;
        }

        private void OnDisable()
        {
            camPositionData.SetCamPositionEvent -= ChangeCamPosition;
            camPositionData.RotateCamEvent -= ChangeCamRotation;
            camPositionData.RecenterEvent -= Recenter;
        }

        private void ChangeCamPosition(int id)
        {
            // Move camera rig to the player's canonical position/rotation and reset local offsets.
            cameraRig.transform.position = boardProperties.GetPlayerPosition(id).position;
            cameraRig.transform.rotation = boardProperties.GetPlayerPosition(id).rotation;

            // Store center rotation and reset offsets/targets
            _centerRotation = cameraRig.transform.rotation;
            _currentYawOffset = 0f;
            _currentPitchOffset = 0f;
            _targetRotation = _centerRotation;
        }

        private void ChangeCamRotation(int id, SwipeDirection direction)
        {
            // Ensure we are anchored to the center rotation for this player
            // (if ChangeCamPosition was called earlier it already set _centerRotation).
            // Modify offsets according to swipe and clamp to limits.
            switch (direction)
            {
                case SwipeDirection.LEFT:
                    _currentYawOffset = Mathf.Clamp(_currentYawOffset - rotationStepDegrees, -maxYawDegrees, maxYawDegrees);
                    break;
                case SwipeDirection.RIGHT:
                    _currentYawOffset = Mathf.Clamp(_currentYawOffset + rotationStepDegrees, -maxYawDegrees, maxYawDegrees);
                    break;
                case SwipeDirection.UP:
                    _currentPitchOffset = Mathf.Clamp(_currentPitchOffset + rotationStepDegrees, -maxPitchDegrees, maxPitchDegrees);
                    break;
                case SwipeDirection.DOWN:
                    _currentPitchOffset = Mathf.Clamp(_currentPitchOffset - rotationStepDegrees, -maxPitchDegrees, maxPitchDegrees);
                    break;
            }

            // Compute world-space axes based on the stored center rotation
            Vector3 worldUp = _centerRotation * Vector3.up;     // camera's local up in world space
            Vector3 worldRight = _centerRotation * Vector3.right; // camera's local right in world space

            // Build rotation that first applies pitch (around camera right), then yaw (around camera up),
            // both in world-space axes relative to the center rotation.
            Quaternion pitchQ = Quaternion.AngleAxis(_currentPitchOffset, worldRight);
            Quaternion yawQ = Quaternion.AngleAxis(_currentYawOffset, worldUp);

            // Apply yaw & pitch relative to the center rotation.
            _targetRotation = (yawQ * pitchQ) * _centerRotation;

            // Apply immediately or smoothly depending on rotationLerpSpeed
            if (rotationLerpSpeed > 0f)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothRotateToTarget());
            }
            else
            {
                cameraRig.transform.rotation = _targetRotation;
            }
        }

        private System.Collections.IEnumerator SmoothRotateToTarget()
        {
            while (Quaternion.Angle(cameraRig.transform.rotation, _targetRotation) > 0.01f)
            {
                cameraRig.transform.rotation = Quaternion.Slerp(cameraRig.transform.rotation, _targetRotation, Time.deltaTime * rotationLerpSpeed);
                yield return null;
            }
            cameraRig.transform.rotation = _targetRotation;
        }

        private void Recenter(int id)
        {
            // Recenter to the player's canonical position & rotation and reset offsets.
            cameraRig.transform.position = boardProperties.GetPlayerPosition(id).position;
            cameraRig.transform.rotation = boardProperties.GetPlayerPosition(id).rotation;

            _centerRotation = cameraRig.transform.rotation;
            _currentYawOffset = 0f;
            _currentPitchOffset = 0f;
            _targetRotation = _centerRotation;
        }
    }
}

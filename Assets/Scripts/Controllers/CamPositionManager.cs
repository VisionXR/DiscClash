using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class CamPositionManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public CamPositionSO camPositionData;
        public BoardDataSO boardData;
        public InputDataSO inputData;
        public PlayersDataSO playerData;

        [Header("Game Objects")]
        public GameObject cameraRig;
        public Transform OriginalPos;

        [Header("Rotation Settings")]
        [Tooltip("Minimum pitch (down) offset in degrees from the center rotation.")]
        public float minPitchDegrees = -9f;
        [Tooltip("Maximum pitch (up) offset in degrees from the center rotation.")]
        public float maxPitchDegrees = 9f;
        [Tooltip("Degrees to change per swipe.")]
        public float rotationStepDegrees = 1f;
        [Tooltip("If > 0, camera rotation will be smoothed. Higher == faster.")]
        public float rotationLerpSpeed = 0f;

        // Internal state
        private Quaternion _centerRotation = Quaternion.identity;
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
            cameraRig.transform.position = boardData.GetPlayerPosition(id).position;
            cameraRig.transform.rotation = boardData.GetPlayerPosition(id).rotation;

            // Store center rotation and reset offsets/targets
            _centerRotation = cameraRig.transform.rotation;
            _currentPitchOffset = 0f;
            _targetRotation = _centerRotation;
        }

        private void ChangeCamRotation(int id, SwipeDirection direction)
        {
            // Modify pitch offset according to swipe and clamp to configured min/max.
            switch (direction)
            {
                case SwipeDirection.UP:
                    _currentPitchOffset = Mathf.Clamp(_currentPitchOffset + rotationStepDegrees, minPitchDegrees, maxPitchDegrees);
                    break;
                case SwipeDirection.DOWN:
                    _currentPitchOffset = Mathf.Clamp(_currentPitchOffset - rotationStepDegrees, minPitchDegrees, maxPitchDegrees);
                    break;
                default:
                    // LEFT/RIGHT swipes are ignored because yaw was removed.
                    break;
            }

            // Compute camera right axis in world space (pitch rotates around this)
            Vector3 worldRight = _centerRotation * Vector3.right;

            // Apply pitch relative to the center rotation.
            Quaternion pitchQ = Quaternion.AngleAxis(_currentPitchOffset, worldRight);
            _targetRotation = pitchQ * _centerRotation;

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
            cameraRig.transform.position = boardData.GetPlayerPosition(id).position;
            cameraRig.transform.rotation = boardData.GetPlayerPosition(id).rotation;

            _centerRotation = cameraRig.transform.rotation;
            _currentPitchOffset = 0f;
            _targetRotation = _centerRotation;
        }
    }
}

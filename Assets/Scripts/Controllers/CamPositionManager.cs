using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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

        [Header("Movement Settings")]
        public float leftLimit = -0.5f;
        public float rightLimit = 0.5f;

        [Header("Swipe Settings")]
        [Tooltip("Minimum horizontal movement (pixels) before a swipe is considered active.")]
        public float minSwipeDistancePixels = 50f;
        [Tooltip("Horizontal pixel distance that maps to the full left->right range (normalized -1..1).")]
        public float swipePixelsForFullRange = 300f;

        [Header("Smoothing")]
        [Tooltip("Time to smooth camera position toward the target (seconds). Small value = snappier.")]
        public float cameraSmoothTime = 0.06f;
        [Tooltip("If target is this close, snap to it and stop smoothing.")]
        public float snapEpsilon = 0.001f;

        // internal state
        private int _currentPlayerId = -1;
        private bool swipeActive;
        private Vector2 swipeStartScreen;

        // smoothing helpers
        private Vector3 _cameraVelocity = Vector3.zero;

        private void OnEnable()
        {
            camPositionData.SetCamPositionEvent += ChangeCamPosition;
            camPositionData.RecenterEvent += Recenter;

            inputData.TouchStartedEvent += TouchStarted;
            inputData.TouchContinuedEvent += TouchContinued;
            inputData.TouchEndedEvent += TouchEnded;
        }

        private void OnDisable()
        {
            camPositionData.SetCamPositionEvent -= ChangeCamPosition;
            camPositionData.RecenterEvent -= Recenter;

            inputData.TouchStartedEvent -= TouchStarted;
            inputData.TouchContinuedEvent -= TouchContinued;
            inputData.TouchEndedEvent -= TouchEnded;
        }


        private void TouchStarted(TouchZone zone, Vector2 pos)
        {
            // mirror MouseInputManager: only handle cam swipes in LEFT/RIGHT zones
            if (zone == TouchZone.MIDDLE) return;

            // begin candidate swipe
            swipeActive = true;
            swipeStartScreen = pos;
        }

        private void TouchContinued(TouchZone zone, Vector2 pos)
        {
            if (!swipeActive) return;
            if (_currentPlayerId < 0) return;
            if (boardData == null) return;

            // horizontal delta in pixels
            float deltaX = pos.x - swipeStartScreen.x;

            // if movement is less than the minimum threshold, ignore
            if (Mathf.Abs(deltaX) < minSwipeDistancePixels) return;

            // normalized in range [-1, 1] where swipePixelsForFullRange maps to full range
            float normalized = Mathf.Clamp(deltaX / swipePixelsForFullRange, -1f, 1f);

            // call MoveCam with the current player id and normalized offset
            MoveCam(_currentPlayerId, normalized);
        }


        private void TouchEnded(TouchZone zone, Vector2 pos)
        {
            // stop swipe tracking
            swipeActive = false;
        }



        private void ChangeCamPosition(int id)
        {
            // Move camera rig to the player's canonical position/rotation and reset internal state.
            var playerPos = boardData.GetPlayerPosition(id);
            if (playerPos == null) return;

            cameraRig.transform.position = playerPos.position;
            cameraRig.transform.rotation = playerPos.rotation;

            _currentPlayerId = id;
            swipeActive = false;
            _cameraVelocity = Vector3.zero;
        }

        private void Recenter(int id)
        {
            // Recenter to the player's canonical position & rotation and reset offsets.
            var playerPos = boardData.GetPlayerPosition(id);
            if (playerPos == null) return;

            cameraRig.transform.position = playerPos.position;
            cameraRig.transform.rotation = playerPos.rotation;

            _currentPlayerId = id;
            swipeActive = false;
            _cameraVelocity = Vector3.zero;
        }

        /// <summary>
        /// Move the camera laterally around the player's canonical center.
        /// normalizedOffset: -1 => full leftLimit, 0 => center, +1 => full rightLimit.
        /// Uses smoothing and clamps final position to prevent jitter at the ends.
        /// </summary>
        private void MoveCam(int id, float normalizedOffset)
        {
            if (boardData == null) return;

            var playerTransform = boardData.GetPlayerPosition(id);
            if (playerTransform == null) return;

            // Correct mapping: map normalizedOffset [-1,1] -> t [0,1]
            float t = Mathf.Clamp01((normalizedOffset + 1f) * rightLimit);

            // compute offset amount between leftLimit and rightLimit
            float offsetAmount = Mathf.Lerp(leftLimit, rightLimit, t);

            // apply offset along the player's right axis so camera follows player's orientation
            Vector3 rightAxis = playerTransform.rotation * Vector3.right;
            Vector3 targetPos = playerTransform.position + rightAxis * offsetAmount;

            // Prevent tiny oscillations at the ends by clamping offsetAmount explicitly
            // (ensure targetPos is within computed allowed range)
            Vector3 leftPos = playerTransform.position + rightAxis * leftLimit;
            Vector3 rightPos = playerTransform.position + rightAxis * rightLimit;
            // project target onto the segment [leftPos, rightPos]
            Vector3 projected = Vector3.Lerp(leftPos, rightPos, t);

            // Smoothly move camera towards projected position
            if ((cameraRig.transform.position - projected).sqrMagnitude <= snapEpsilon * snapEpsilon)
            {
                cameraRig.transform.position = projected;
                _cameraVelocity = Vector3.zero;
            }
            else
            {
                cameraRig.transform.position = Vector3.SmoothDamp(cameraRig.transform.position, projected, ref _cameraVelocity, cameraSmoothTime);
            }
        }
    }
}

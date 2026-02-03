using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
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

        [Header("Horizontal (X) Movement Settings")]
        public float leftLimit = -0.5f;
        public float rightLimit = 0.5f;

        [Header("Vertical (Y) Movement Settings")]
        public float verticalLowerLimit = -0.5f;
        public float verticalUpperLimit = 0.5f;

        [Header("Swipe Settings")]
        [Tooltip("Threshold before movement starts to avoid accidental jitters.")]
        public float minSwipeDistancePixels = 10f;
        [Tooltip("Pixel distance that maps to the full range.")]
        public float swipePixelsForFullRange = 300f;

        [Header("Smoothing")]
        public float cameraSmoothTime = 0.06f;
        public float snapEpsilon = 0.001f;

        // Internal State
        private int _currentPlayerId = -1;
        private bool swipeActive;
        private Vector2 swipeStartScreen;
        private Vector2 _lastTouchScreen;

        // Track normalized positions independently [-1 to 1]
        // 0 = Center, -1 = Lower/Left Limit, 1 = Upper/Right Limit
        private float _currentXOffsetT = 0f;
        private float _currentYOffsetT = 0f;

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
            if (zone == TouchZone.MIDDLE) return;
            swipeActive = true;
            swipeStartScreen = pos;
            _lastTouchScreen = pos;
        }

        private void TouchContinued(TouchZone zone, Vector2 pos)
        {
            if (!swipeActive || _currentPlayerId < 0 || boardData == null) return;

            // Calculate movement since the last frame
            float deltaX = pos.x - _lastTouchScreen.x;
            float deltaY = pos.y - _lastTouchScreen.y;

            // Total distance from start to respect the minimum threshold
            float totalDist = Vector2.Distance(pos, swipeStartScreen);
            if (totalDist < minSwipeDistancePixels) return;

            // Update both X and Y offsets simultaneously
            // (delta / fullRange) * 2 converts pixel delta to the [-1, 1] normalized space
            float stepX = (deltaX / swipePixelsForFullRange) * 2f;
            float stepY = (deltaY / swipePixelsForFullRange) * 2f;

            _currentXOffsetT = Mathf.Clamp(_currentXOffsetT + stepX, -1f, 1f);
            _currentYOffsetT = Mathf.Clamp(_currentYOffsetT + stepY, -1f, 1f);

            ApplyPosition();
            _lastTouchScreen = pos;
        }

        private void TouchEnded(TouchZone zone, Vector2 pos)
        {
            swipeActive = false;
        }

        private void ChangeCamPosition(int id) => ResetToPlayer(id);
        private void Recenter(int id) => ResetToPlayer(id);

        private void ResetToPlayer(int id)
        {
            var playerPos = boardData.GetPlayerPosition(id);
            if (playerPos == null) return;

            _currentPlayerId = id;
            _currentXOffsetT = 0f;
            _currentYOffsetT = 0f;

            cameraRig.transform.position = playerPos.position;
            cameraRig.transform.rotation = playerPos.rotation;

            swipeActive = false;
            _cameraVelocity = Vector3.zero;
        }

        /// <summary>
        /// Combines stored X and Y offsets to position the camera relative to the player.
        /// </summary>
        private void ApplyPosition()
        {
            var playerTransform = boardData.GetPlayerPosition(_currentPlayerId);
            if (playerTransform == null) return;

            // Map normalized T [-1, 1] to actual local units defined by limits
            float horizontalValue = _currentXOffsetT > 0
                ? _currentXOffsetT * rightLimit
                : _currentXOffsetT * Mathf.Abs(leftLimit);

            float verticalValue = _currentYOffsetT > 0
                ? _currentYOffsetT * verticalUpperLimit
                : _currentYOffsetT * Mathf.Abs(verticalLowerLimit);

            // Calculate target: Base Position + (Right * X) + (Up * Y)
            Vector3 targetPosition = playerTransform.position
                                     + (playerTransform.right * horizontalValue)
                                     + (playerTransform.up * verticalValue);

            // Smoothly move the camera rig to the 2D offset position
            if ((cameraRig.transform.position - targetPosition).sqrMagnitude <= snapEpsilon * snapEpsilon)
            {
                cameraRig.transform.position = targetPosition;
                _cameraVelocity = Vector3.zero;
            }
            else
            {
                cameraRig.transform.position = Vector3.SmoothDamp(
                    cameraRig.transform.position,
                    targetPosition,
                    ref _cameraVelocity,
                    cameraSmoothTime
                );
            }
        }
    }
}
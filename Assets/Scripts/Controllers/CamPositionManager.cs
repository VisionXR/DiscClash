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
        private Vector2 _lastTouchScreen;

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

            // begin candidate swipe: record both start and last positions
            swipeActive = true;
            swipeStartScreen = pos;
            _lastTouchScreen = pos;
        }

        private void TouchContinued(TouchZone zone, Vector2 pos)
        {
            if (!swipeActive) return;
            if (_currentPlayerId < 0) return;
            if (boardData == null) return;

            // total horizontal movement since touch start (pixels)
            float totalDeltaX = pos.x - swipeStartScreen.x;

            // don't start applying movement until user has moved beyond threshold
            if (Mathf.Abs(totalDeltaX) < minSwipeDistancePixels)
            {
                // update last touch so that future deltas are measured from current finger position,
                // but still require initial threshold to be crossed to start moving the camera.
                _lastTouchScreen = pos;
                return;
            }

            // incremental horizontal delta in pixels since last frame
            float deltaX = pos.x - _lastTouchScreen.x;

            // normalized delta in range [-1, 1] where swipePixelsForFullRange maps to full range
            float normalizedDelta = Mathf.Clamp(deltaX / swipePixelsForFullRange, -1f, 1f);

            // apply relative camera movement based on incremental swipe
            MoveCamRelative(_currentPlayerId, normalizedDelta);

            // update last touch for the next incremental calculation
            _lastTouchScreen = pos;
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
        /// Move the camera relative to its current position along the player's left-right track.
        /// normalizedDelta is a fraction of the full left->right range (e.g. 1.0 == full width).
        /// </summary>
        private void MoveCamRelative(int id, float normalizedDelta)
        {
            if (boardData == null) return;

            var playerTransform = boardData.GetPlayerPosition(id);
            if (playerTransform == null) return;

            // Define the local track relative to the player
            Vector3 rightAxis = playerTransform.right;
            Vector3 leftBoundPos = playerTransform.position + (rightAxis * leftLimit);
            Vector3 rightBoundPos = playerTransform.position + (rightAxis * rightLimit);

            Vector3 trackVector = rightBoundPos - leftBoundPos;
            float trackLenSq = trackVector.sqrMagnitude;

            // find current normalized t of the camera along the track [0,1]
            float currentT = 0.5f;
            if (trackLenSq > Mathf.Epsilon)
            {
                Vector3 toCam = cameraRig.transform.position - leftBoundPos;
                currentT = Mathf.Clamp01(Vector3.Dot(toCam, trackVector) / trackLenSq);
            }

            // normalizedDelta already represents fraction of full range (-1..1), so add it directly
            float newT = Mathf.Clamp01(currentT + normalizedDelta);

            Vector3 targetDestination = Vector3.Lerp(leftBoundPos, rightBoundPos, newT);

            // 4. Smooth movement logic (same as before)
            float distanceSq = (cameraRig.transform.position - targetDestination).sqrMagnitude;

            if (distanceSq <= snapEpsilon * snapEpsilon)
            {
                cameraRig.transform.position = targetDestination;
                _cameraVelocity = Vector3.zero;
            }
            else
            {
                cameraRig.transform.position = Vector3.SmoothDamp(
                    cameraRig.transform.position,
                    targetDestination,
                    ref _cameraVelocity,
                    cameraSmoothTime
                );
            }
        }

        // kept for compatibility if other code expects an absolute move method
        private void MoveCam(int id, float normalizedOffset)
        {
            if (boardData == null) return;

            var playerTransform = boardData.GetPlayerPosition(id);
            if (playerTransform == null) return;

            // 1. Correctly map normalizedOffset [-1, 1] to a [0, 1] range
            // -1 becomes 0, 0 becomes 0.5, 1 becomes 1.
            float t = (normalizedOffset + 1f) * 0.5f;
            t = Mathf.Clamp01(t);

            // 2. Define the local track relative to the player
            Vector3 rightAxis = playerTransform.right; // Shortcut for playerTransform.rotation * Vector3.right

            Vector3 leftBoundPos = playerTransform.position + (rightAxis * leftLimit);
            Vector3 rightBoundPos = playerTransform.position + (rightAxis * rightLimit);

            // 3. The precise target point on that line segment
            Vector3 targetDestination = Vector3.Lerp(leftBoundPos, rightBoundPos, t);

            // 4. Smooth movement logic
            float distanceSq = (cameraRig.transform.position - targetDestination).sqrMagnitude;

            if (distanceSq <= snapEpsilon * snapEpsilon)
            {
                cameraRig.transform.position = targetDestination;
                _cameraVelocity = Vector3.zero;
            }
            else
            {
                cameraRig.transform.position = Vector3.SmoothDamp(
                    cameraRig.transform.position,
                    targetDestination,
                    ref _cameraVelocity,
                    cameraSmoothTime
                );
            }
        }
    }
}

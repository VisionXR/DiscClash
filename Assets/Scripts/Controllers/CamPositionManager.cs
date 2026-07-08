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
      
        }

        private void OnDisable()
        {
            camPositionData.SetCamPositionEvent -= ChangeCamPosition;
            camPositionData.RecenterEvent -= Recenter;
    
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

      
    }
}
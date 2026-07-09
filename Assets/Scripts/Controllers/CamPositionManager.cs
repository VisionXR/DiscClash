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



        private void OnEnable()
        {
            camPositionData.SetCamPositionFrontViewEvent += SetCamPositionFrontView;
            camPositionData.SetCamPositionTopViewEvent += ResetToTopView;
            camPositionData.RecenterEvent += Recenter;
      
        }

        private void OnDisable()
        {
            camPositionData.SetCamPositionFrontViewEvent -= SetCamPositionFrontView;
            camPositionData.SetCamPositionTopViewEvent -= ResetToTopView;
            camPositionData.RecenterEvent -= Recenter;
    
        }

        private void SetCamPositionFrontView(int id) => ResetToPlayer(id);
        private void Recenter(int id) => ResetToPlayer(id);

        private void ResetToPlayer(int id)
        {
            var playerPos = boardData.GetPlayerPositionFrontView(id);
            if (playerPos == null) return;


            cameraRig.transform.position = playerPos.position;
            cameraRig.transform.rotation = playerPos.rotation;

        }

        private void ResetToTopView(int id)
        {
            var playerPos = boardData.GetPlayerPositionTopView(id);
            if (playerPos == null) return;


            cameraRig.transform.position = playerPos.position;
            cameraRig.transform.rotation = playerPos.rotation;
        }

      
    }
}
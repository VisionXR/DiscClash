
using com.VisionXR.ModelClasses;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace com.VisionXR.GameElements
{

    public class StrikerMovement : MonoBehaviour, IStrikerMovement
    {

        [Header("Scriptable Objects")]
        public PlayersDataSO playersData;
        public StrikerDataSO strikerData;
        public BoardDataSO boardData;


        [Header("Local Variables")]
        public float currentProgress = 0.5f;
        public int strikerId = 1;
        public Rigidbody strikerRigidbody;
        public SplineContainer strikerSpline;
        public GameObject strikerRotation;

        // local
        public float yawThresholdDegrees = 1f;
        public Vector3 fixedCenterPoint;
        private float _lastAppliedYaw;
      
        // ---------------- Helpers ----------------

        void OnEnable()
        {
            if (strikerRotation != null)
            {
                transform.rotation = strikerRotation.transform.rotation;
            }
        }


        public void SetStrikerID(int id)
        {

            strikerId = id;
            gameObject.name = "Striker" + id;
            strikerRigidbody = GetComponent<Rigidbody>();
            StartCoroutine(WaitAndSet());
        }

        private IEnumerator WaitAndSet()
        {
            yield return new WaitForSeconds(0.1f);
            
            fixedCenterPoint = boardData.GetPlayerPositionFrontView(strikerId).position;
            strikerSpline = boardData.GetStrikerStrip(strikerId);
            strikerRotation = boardData.GetStrikerRotations(strikerId);
            ResetStriker();

        }

        public int GetStrikerId()
        {
            return strikerId;
        }

        /// <summary>
        /// Moves striker strictly using Spline evaluation for position and direction.
        /// </summary>
        public void MoveStriker(float normalisedValue)
        {
            if (strikerSpline == null) return;

            // 1. Calculate the proposed progression on the spline
            float proposedProgress = currentProgress + normalisedValue;
            proposedProgress = Mathf.Clamp(proposedProgress, 0.01f, 0.99f);

            // 2. Determine step direction on the spline based on input delta sign
            // If normalisedValue > 0, we are moving forward along the spline track (+t).
            // If normalisedValue < 0, we are moving backward along the spline track (-t).
            float stepDirection = Mathf.Sign(normalisedValue);

            // 3. Find a clear position by progressing down the spline path
            float finalProgress = FindStrikerNextPositionOnSpline(proposedProgress, stepDirection);

            // 4. Sample the absolute final safe position from the spline
            float3 finalLocalPos;
            float3 localTangent;
            float3 localUp;
            strikerSpline.Evaluate(finalProgress, out finalLocalPos, out localTangent, out localUp);

            // 5. Apply the safe location and cache the successful progress marker
            transform.position = finalLocalPos;
            currentProgress = finalProgress;
        }

        public float FindStrikerNextPositionOnSpline(float startProgress, float stepDirection)
        {
            float currentProgressCheck = startProgress;
            float radius = boardData.GetStrikerRadius();

            // Adjust step resolution: How much spline 't' progress roughly equals half a radius?
            // Since spline lengths vary, a fine step size works best (e.g., 1% or 2% steps)
            float splineStepSize = 0.015f * stepDirection;
            int safetyCounter = 0;

            while (true)
            {
                // Sample the actual position at this specific curve coordinate
                float3 evalLocalPos;
                float3 tangent;
                float3 up;
                strikerSpline.Evaluate(currentProgressCheck, out evalLocalPos, out tangent, out up);

                bool isBlocked = false;
                Collider[] cols = Physics.OverlapSphere(evalLocalPos, radius + 0.01f);

                foreach (Collider c in cols)
                {
                    if (c == null) continue;
                    if (c.CompareTag("White") || c.CompareTag("Red") || c.CompareTag("Black"))
                    {
                        isBlocked = true;
                        break;
                    }
                }

                // If the spot along the curve is empty, this progress point is safe!
                if (!isBlocked) return currentProgressCheck;

                // Move further down or backward along the track following the exact curve geometry
                currentProgressCheck += splineStepSize;

                // Keep the search bound within the track limits
                if (currentProgressCheck < 0.01f || currentProgressCheck > 0.99f || ++safetyCounter > 10)
                {
                    // If no clear space is found nearby, freeze progress by returning the current location
                    return currentProgress;
                }
            }
        }

        /// <summary>
        /// Safely sets the striker's position for an AI shot, ensuring the spline progression variable is synchronized.
        /// </summary>
        /// <summary>
        /// Safely sets the striker's position for an AI shot, avoiding coins by moving inward towards the center or outward based on progress.
        /// </summary>
        public Vector3 TeleportStrikerToSplineProgress(float progressValue)
        {
            if (strikerSpline == null) return transform.position;

            // 1. Clamp the baseline requested target progress
            float proposedProgress = Mathf.Clamp(progressValue, 0.01f, 0.99f);

            // 2. Determine step direction if a coin is hit:
            // If the target is on the right half (> 0.5), nudge it left/downwards (-1).
            // If the target is on the left half (<= 0.5), nudge it right/upwards (+1).
            float stepDirection = (proposedProgress > 0.5f) ? -1f : 1f;

            // 3. Reuse your reliable overlapping logic to find a valid progress marker along the curve
            float finalSafeProgress = FindStrikerNextPositionOnSpline(proposedProgress, stepDirection);

            // 4. Sample the final coordinates from the curve using our safe progress index
            float3 safeLocalPos;
            float3 localTangent;
            float3 localUp;
            strikerSpline.Evaluate(finalSafeProgress, out safeLocalPos, out localTangent, out localUp);

            // 5. Explicitly synchronize the internal tracking variable so future player inputs are seamless
            currentProgress = finalSafeProgress;

            // 6. Return the finalized 3D coordinate vector to your calling script/AI coroutine
            return (Vector3)safeLocalPos;
        }
        public void AimStriker(Vector3 direction)
        {
                     
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        public void AimStriker(float deltaAngle)
        {
            // Apply the relative change around the Y-axis (Vector3.up)
            transform.Rotate(Vector3.up, deltaAngle);
        }



        public void ResetStriker()
        {
            if (strikerSpline == null) return;

            currentProgress = 0f;
            // Reset to the middle of the spline (t = 0.5)
            MoveStriker(0.5f);
           
            strikerRigidbody.linearVelocity = Vector3.zero;
            strikerRigidbody.angularVelocity = Vector3.zero;

            if (strikerRotation != null)
            {
                transform.rotation = strikerRotation.transform.rotation;
            }
        }

    }
}




using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
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
            
            fixedCenterPoint = boardData.GetPlayerPosition(strikerId).position;
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

            normalisedValue = currentProgress+ normalisedValue;

            normalisedValue = Mathf.Clamp(normalisedValue, 0.01f, 0.99f);

            // 1. Evaluate Position and Tangent (Direction) from Spline
            // float3 is used by the Spline package; we convert to Vector3
            float3 localPos;
            float3 localTangent;
            float3 localUp;

            strikerSpline.Evaluate(normalisedValue, out localPos, out localTangent, out localUp);

            // 2. Determine Nudge Direction
            // We use the tangent (the line of the spline) to nudge the striker left or right along the track
            // if it hits a coin.
            Vector3 nudgeDir = (normalisedValue > 0.5f) ? -localTangent : localTangent;

            nudgeDir = nudgeDir.normalized;

            // 3. Apply position with collision check
            transform.position = FindStrikerNextPosition(localPos, nudgeDir);

            currentProgress = normalisedValue;
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

        //public Vector3 FindStrikerNextPosition(Vector3 evalPos, Vector3 dir)
        //{
        //    float radius = boardData.GetStrikerRadius();

        //    // 1. Perform a single check at the proposed evaluation position
        //    // (Slight buffer added to radius to prevent micro-clipping)
        //    Collider[] cols = Physics.OverlapSphere(evalPos, radius + 0.01f);

        //    foreach (Collider c in cols)
        //    {
        //        if (c == null) continue;

        //        // 2. If it hits any coin, the position is blocked. Reject the move immediately.
        //        if (c.CompareTag("White") || c.CompareTag("Red") || c.CompareTag("Black"))
        //        {
        //            // Returning the current transform position instead of 'evalPos' 
        //            // forces the striker to stay right where it currently is.
        //            return transform.position;
        //        }
        //    }

        //    // 3. No coins detected, the position is completely clear.
        //    return evalPos;
        //}

        public Vector3 FindStrikerNextPosition(Vector3 evalPos, Vector3 dir)
        {
            Vector3 currentCheckPos = evalPos;
            float radius = boardData.GetStrikerRadius();
            int safetyCounter = 0;
            while (true)
            {
                bool isBlocked = false;
                Collider[] cols = Physics.OverlapSphere(currentCheckPos, radius + 0.01f);
                foreach (Collider c in cols)
                {
                    if (c == null) continue;
                    if (c.CompareTag("White") || c.CompareTag("Red") || c.CompareTag("Black"))
                    {
                        isBlocked = true;
                        break;
                    }
                }
                if (!isBlocked) break;

                // Move slightly along the spline direction to find a gap

                currentCheckPos += dir * (radius / 2f);
                if (++safetyCounter > 10)
                {
                    return evalPos; // Return original if no spot found

                }

            }

            return currentCheckPos;

        }
        public void ResetStriker()
        {
            if (strikerSpline == null) return;

            currentProgress = 0f;
            // Reset to the middle of the spline (t = 0.5)
            MoveStriker(0.5f);
           
            strikerRigidbody.linearVelocity = Vector3.zero;
            strikerRigidbody.angularVelocity = Vector3.zero;
        }

    }
}




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
        public int strikerId = 1;
        public List<Transform> strikerPositions = new List<Transform>();
        public Rigidbody strikerRigidbody;
        public SplineContainer strikerSpline;

        // local
        public float yawThresholdDegrees = 1f;
        public Vector3 fixedCenterPoint;
        private float _lastAppliedYaw;
      
        // ---------------- Helpers ----------------

        private bool HasValidStrikerPositions(int requiredCount = 4)
        {
            if (strikerPositions == null) return false;
            if (strikerPositions.Count < requiredCount) return false;
            for (int i = 0; i < strikerPositions.Count; i++)
            {
                if (strikerPositions[i] == null) return false;
            }
            return true;
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
            strikerPositions = boardData.GetStrikerPosition(strikerId);
            fixedCenterPoint = boardData.GetPlayerPosition(strikerId).position;
            strikerSpline = boardData.GetStrikerStrip(strikerId);
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

            // 1. Evaluate Position and Tangent (Direction) from Spline
            // float3 is used by the Spline package; we convert to Vector3
            float3 localPos;
            float3 localTangent;
            float3 localUp;

            strikerSpline.Evaluate(normalisedValue, out localPos, out localTangent, out localUp);


            Debug.Log("Local Pos is " + localPos.ToString());

            Vector3 worldPos = strikerSpline.transform.TransformPoint(localPos);
            // Tangent is the forward direction of the spline at that point
            Vector3 worldTangent = strikerSpline.transform.TransformDirection(localTangent).normalized;

            Debug.Log("World Pos is " + worldPos.ToString());

            // 2. Determine Nudge Direction
            // We use the tangent (the line of the spline) to nudge the striker left or right along the track
            // if it hits a coin.
            Vector3 nudgeDir = (normalisedValue > 0.5f) ? -localTangent : localTangent;

            nudgeDir = nudgeDir.normalized;

            Debug.Log("Nudge Dir is " + nudgeDir.ToString());

            // 3. Apply position with collision check
            transform.position = FindStrikerNextPosition(localPos, nudgeDir);

            transform.rotation = strikerPositions[2].transform.rotation;

            // 4. Match rotation to spline forward if needed (optional)
            // transform.rotation = Quaternion.LookRotation(worldTangent, Vector3.up);
        }

        public void AimStriker(Vector3 direction)
        {
            if (!HasValidStrikerPositions(3))
            {
                Debug.LogWarning($"[StrikerMovement] AimStriker aborted - invalid strikerPositions for strikerId={strikerId}");
                return;
            }

            transform.rotation = strikerPositions[2].transform.rotation;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.eulerAngles = VectorUtility.RoundPositionUpto3Decimals(transform.eulerAngles);
        }


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
                currentCheckPos += dir * (radius / 10f);

                if (++safetyCounter > 100)
                {
                    return evalPos; // Return original if no spot found
                }
            }

            // Optional: Clamp to Spline Bounds
            // Since we aren't using strikerPositions, we ensure the new position 
            // hasn't drifted too far from the original evaluated spline point.
            if (Vector3.Distance(evalPos, currentCheckPos) > radius * 2f)
                return evalPos;

            return currentCheckPos;
        }

        public void ResetStriker()
        {
            if (strikerSpline == null) return;

            // Reset to the middle of the spline (t = 0.5)
         //   MoveStriker(0.5f);

            strikerRigidbody.linearVelocity = Vector3.zero;
            strikerRigidbody.angularVelocity = Vector3.zero;
        }

    }
}



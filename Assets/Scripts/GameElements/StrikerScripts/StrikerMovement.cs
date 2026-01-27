using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private bool SafeEnsureRigidbody()
        {
            if (strikerRigidbody == null)
            {
                strikerRigidbody = GetComponent<Rigidbody>();
            }
            return strikerRigidbody != null;
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
            ResetStriker();

        }

        public int GetStrikerId()
        {
            return strikerId;
        }

        public void MoveStriker(float normalisedValue)
        {
            if (!HasValidStrikerPositions(2))
            {
                Debug.LogWarning($"[StrikerMovement] MoveStriker aborted - invalid strikerPositions for strikerId={strikerId}");
                return;
            }

            // Ensure normalized is within [0,1]
            float t = normalisedValue;

            // Interpolate between first and last striker anchor positions
            Vector3 start = strikerPositions[0].position;
            Vector3 end = strikerPositions[strikerPositions.Count - 1].position;
            Vector3 finalpos = Vector3.Lerp(start, end, t);

            // Choose direction to nudge striker to a valid non-overlapping position
            float leftDistance = Vector3.Distance(finalpos, start);
            float rightDistance = Vector3.Distance(finalpos, end);

            if (leftDistance > rightDistance)
            {
                transform.position = FindStrikerNextPosition(finalpos, -strikerPositions[0].right);
            }
            else
            {
                transform.position = FindStrikerNextPosition(finalpos, strikerPositions[0].right);
            }
        }
        public void MoveStriker(SwipeDirection swipeDirection)
        {
            if (!HasValidStrikerPositions(1))
            {
                Debug.LogWarning($"[StrikerMovement] MoveStriker(Swipe) aborted - invalid strikerPositions for strikerId={strikerId}");
                return;
            }

            Vector3 finalPos;

            if (swipeDirection == SwipeDirection.LEFT)
            {
                finalPos = FindStrikerNextPosition(transform.position, -strikerPositions[0].transform.right);
            }
            else
            {
                finalPos = FindStrikerNextPosition(transform.position, strikerPositions[0].transform.right);
            }

            if (strikerId == 1 || strikerId == 2)
            {
              
                float lower = Mathf.Min(strikerPositions[0].transform.position.x, strikerPositions[strikerPositions.Count - 1].transform.position.x);
                float upper = Mathf.Max(strikerPositions[0].transform.position.x, strikerPositions[strikerPositions.Count - 1].transform.position.x);
                if (finalPos.x >= lower && finalPos.x <= upper)
                {
                    Vector3 finalpos = new Vector3(finalPos.x, strikerPositions[0].transform.position.y, strikerPositions[0].transform.position.z);
                    transform.position = VectorUtility.RoundPositionUpto3Decimals(finalpos);
                }
            }
            else
            {
                float lower = Mathf.Min(strikerPositions[0].transform.position.z, strikerPositions[strikerPositions.Count - 1].transform.position.z);
                float upper = Mathf.Max(strikerPositions[0].transform.position.z, strikerPositions[strikerPositions.Count - 1].transform.position.z);
                if (finalPos.z >= lower && finalPos.z <= upper)
                {
                    Vector3 finalpos = new Vector3(strikerPositions[0].transform.position.x, strikerPositions[0].transform.position.y, finalPos.z);
                    transform.position = VectorUtility.RoundPositionUpto3Decimals(finalpos);
                }
            }

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


        public Vector3 FindStrikerNextPosition(Vector3 finalPos, Vector3 dir)
        {
            Vector3 newPosition = finalPos;
            if (boardData == null)
            {
                Debug.LogWarning("[StrikerMovement] boardData is null in FindStrikerNextPosition.");
                return finalPos;
            }
            bool isThisCorrectPosition;
            int safetyCounter = 0;
            while (true)
            {
                newPosition += dir * boardData.GetStrikerRadius() / 10;
                isThisCorrectPosition = true;
                Collider[] cols = Physics.OverlapSphere(newPosition, boardData.GetStrikerRadius() + 0.01f);   
                foreach (Collider c in cols)
                {
                    if (c == null) continue;
                    if (c.gameObject.tag == "White" || c.gameObject.tag == "Red" || c.gameObject.tag == "Black")
                    {
                        isThisCorrectPosition = false;
                        break;
                    }

                }
                if (isThisCorrectPosition)
                {
                    break;
                }

                // safety to avoid infinite loop in unexpected cases
                if (++safetyCounter > 1000)
                {
                    Debug.LogWarning("[StrikerMovement] FindStrikerNextPosition reached safety limit.");
                    break;
                }
            }
            finalPos = newPosition;

            if (!HasValidStrikerPositions(1))
            {
                return finalPos;
            }

            if (strikerId == 1 || strikerId == 2)
            {
                float lower = Mathf.Min(strikerPositions[0].transform.position.x, strikerPositions[strikerPositions.Count-1].transform.position.x);
                float upper = Mathf.Max(strikerPositions[0].transform.position.x, strikerPositions[strikerPositions.Count - 1].transform.position.x);
                if (finalPos.x >= lower && finalPos.x <= upper)
                {
                    newPosition = new Vector3(finalPos.x, strikerPositions[0].transform.position.y, strikerPositions[0].transform.position.z);
                }
            }
            else
            {
                float lower = Mathf.Min(strikerPositions[0].transform.position.z, strikerPositions[strikerPositions.Count - 1].transform.position.z);
                float upper = Mathf.Max(strikerPositions[0].transform.position.z, strikerPositions[strikerPositions.Count - 1].transform.position.z);
                if (finalPos.z >= lower && finalPos.z <= upper)
                {
                    newPosition = new Vector3(strikerPositions[0].transform.position.x, strikerPositions[0].transform.position.y, finalPos.z);
                }
            }
            return newPosition;
        }

        public void ResetStriker()
        {
            if (!HasValidStrikerPositions(4))
            {
                Debug.LogWarning($"[StrikerMovement] ResetStriker aborted - invalid strikerPositions for strikerId={strikerId}");
                return;
            }

            // safe ensure rigidbody
            SafeEnsureRigidbody();

            // Use positions/rotation safely
            transform.position = FindStrikerNextPosition(strikerPositions[3].transform.position, strikerPositions[3].transform.right);
            transform.rotation = strikerPositions[3].transform.rotation;

            if (strikerRigidbody != null)
            {
                // keep original semantics (project uses linearVelocity/angularVelocity)
#pragma warning disable CS0618
                try
                {
                    strikerRigidbody.linearVelocity = Vector3.zero;
                    strikerRigidbody.angularVelocity = Vector3.zero;
                }
                catch
                {
                    // fallback to standard Unity API if linearVelocity is not present
                    strikerRigidbody.velocity = Vector3.zero;
                    strikerRigidbody.angularVelocity = Vector3.zero;
                }
#pragma warning restore CS0618
            }
        }

    }
}



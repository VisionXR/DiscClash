using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace com.VisionXR.GameElements
{

    public class StrikerMovement : MonoBehaviour, IStrikerMovement
    {

        [Header("Scriptable Objects")]
        public PlayersDataSO playersData;
        public StrikerDataSO strikerData;
        public BoardPropertiesSO boardProperties;


        [Header("Local Variables")]
        [SerializeField] private float val = 1;
        [SerializeField] public int strikerId = 1;
        [SerializeField] private List<Transform> StrikerPositions;
        private Rigidbody strikerRigidbody;



        // local
        public float yawThresholdDegrees = 1f;
        private float _lastAppliedYaw;
        private Vector3 fixedCenterPoint;     
        
        public void SetStrikerID(int id)
        {

            strikerId = id;
            gameObject.name = "Striker" + id;
            strikerRigidbody = GetComponent<Rigidbody>();
            StartCoroutine(SetStriker(id));
          
        }

        private IEnumerator SetStriker(int id)
        {
            yield return new WaitForSeconds(0.1f);
          
            StrikerPositions = boardProperties.GetStrikerPosition(id);
            fixedCenterPoint = boardProperties.GetPlayerPosition(strikerId).position;
            ResetStriker();

        }

        public int GetStrikerId()
        {
            return strikerId;
        }

        public void MoveStriker(float val)
        {

            // Validate inputs
            if (StrikerPositions == null || StrikerPositions.Count < 2)
            {
                Debug.LogError($"StrikerMovement.MoveStriker: invalid StrikerPositions for strikerId={strikerId}. Need at least 2 positions.");
                return;
            }

            // Ensure normalized is within [0,1]
            float t = val;

            // Interpolate between first and last striker anchor positions
            Vector3 start = StrikerPositions[0].position;
            Vector3 end = StrikerPositions[StrikerPositions.Count - 1].position;
            Vector3 finalpos = Vector3.Lerp(start, end, t);

            // Choose direction to nudge striker to a valid non-overlapping position
            float leftDistance = Vector3.Distance(finalpos, start);
            float rightDistance = Vector3.Distance(finalpos, end);

            if (leftDistance > rightDistance)
            {
                transform.position = FindStrikerNextPosition(finalpos, -StrikerPositions[0].right);
            }
            else
            {
                transform.position = FindStrikerNextPosition(finalpos, StrikerPositions[0].right);
            }
        }
        public void MoveStriker(SwipeDirection swipeDirection)
        {
            Vector3 finalPos;

            if (swipeDirection == SwipeDirection.LEFT)
            {
                finalPos = FindStrikerNextPosition(transform.position, -StrikerPositions[0].transform.right);
            }
            else
            {
                finalPos = FindStrikerNextPosition(transform.position, StrikerPositions[0].transform.right);
            }

            if (strikerId == 1 || strikerId == 2)
            {
              
                float lower = Mathf.Min(StrikerPositions[0].transform.position.x, StrikerPositions[StrikerPositions.Count - 1].transform.position.x);
                float upper = Mathf.Max(StrikerPositions[0].transform.position.x, StrikerPositions[StrikerPositions.Count - 1].transform.position.x);
                if (finalPos.x >= lower && finalPos.x <= upper)
                {
                    Vector3 finalpos = new Vector3(finalPos.x, StrikerPositions[0].transform.position.y, StrikerPositions[0].transform.position.z);
                    transform.position = VectorUtility.RoundPositionUpto3Decimals(finalpos);
                }
            }
            else
            {
                float lower = Mathf.Min(StrikerPositions[0].transform.position.z, StrikerPositions[StrikerPositions.Count - 1].transform.position.z);
                float upper = Mathf.Max(StrikerPositions[0].transform.position.z, StrikerPositions[StrikerPositions.Count - 1].transform.position.z);
                if (finalPos.z >= lower && finalPos.z <= upper)
                {
                    Vector3 finalpos = new Vector3(StrikerPositions[0].transform.position.x, StrikerPositions[0].transform.position.y, finalPos.z);
                    transform.position = VectorUtility.RoundPositionUpto3Decimals(finalpos);
                }
            }

        }


        public void AimStriker(Vector3 direction)
        {
            transform.rotation = StrikerPositions[2].transform.rotation;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.eulerAngles = VectorUtility.RoundPositionUpto3Decimals(transform.eulerAngles);
        }


        public Vector3 FindStrikerNextPosition(Vector3 finalPos, Vector3 dir)
        {
            Vector3 newPosition = finalPos;
            bool isThisCorrectPosition;
            while (true)
            {
                newPosition += dir * boardProperties.GetStrikerRadius() / 10;
                isThisCorrectPosition = true;
                Collider[] cols = Physics.OverlapSphere(newPosition, boardProperties.GetStrikerRadius() + 0.01f);   
                foreach (Collider c in cols)
                {
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
            }
            finalPos = newPosition;
            if (strikerId == 1 || strikerId == 2)
            {
                float lower = Mathf.Min(StrikerPositions[0].transform.position.x, StrikerPositions[StrikerPositions.Count-1].transform.position.x);
                float upper = Mathf.Max(StrikerPositions[0].transform.position.x, StrikerPositions[StrikerPositions.Count - 1].transform.position.x);
                if (finalPos.x >= lower && finalPos.x <= upper)
                {
                    newPosition = new Vector3(finalPos.x, StrikerPositions[0].transform.position.y, StrikerPositions[0].transform.position.z);
                }
            }
            else
            {
                float lower = Mathf.Min(StrikerPositions[0].transform.position.z, StrikerPositions[StrikerPositions.Count - 1].transform.position.z);
                float upper = Mathf.Max(StrikerPositions[0].transform.position.z, StrikerPositions[StrikerPositions.Count - 1].transform.position.z);
                if (finalPos.z >= lower && finalPos.z <= upper)
                {
                    newPosition = new Vector3(StrikerPositions[0].transform.position.x, StrikerPositions[0].transform.position.y, finalPos.z);
                }
            }
            return newPosition;
        }

        public void ResetStriker()
        {
            transform.position = FindStrikerNextPosition(StrikerPositions[3].transform.position, StrikerPositions[3].transform.right);
            transform.rotation = StrikerPositions[3].transform.rotation;

            strikerRigidbody.linearVelocity = Vector3.zero;
            strikerRigidbody.angularVelocity = Vector3.zero;
        }

    }
}



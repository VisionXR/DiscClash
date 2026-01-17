using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
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
        [SerializeField] private List<GameObject> StrikerPositions = new List<GameObject>();
        public float yawThresholdDegrees = 1f;

        // Tracks the last yaw we actually applied (to enforce threshold between updates)
        private float _lastAppliedYaw;

        // local variables
        private Vector3 fixedCenterPoint;     
        
        public void SetStrikerID(int id)
        {

            strikerId = id;
            GetPositions(id);
            fixedCenterPoint = boardProperties.GetPlayerPosition(strikerId).position;
           
            ResetStriker();

        }

        public int GetStrikerId()
        {
            return strikerId;
        }

        public void MoveStriker(Vector3 controllerPosition, Transform cameraRigTransform)
        {

            // Displacement of the camera rig from the fixed center point along the camera's right vector
            float cameraRigDisplacement = Vector3.Dot(cameraRigTransform.position - fixedCenterPoint, cameraRigTransform.right);

            // Displacement of the controller from the camera rig along the camera's right vector
            float controllerDisplacement = Vector3.Dot(controllerPosition - cameraRigTransform.position, cameraRigTransform.right);

            // Total displacement from the fixed center point
            float totalDisplacement = cameraRigDisplacement + controllerDisplacement;

            // Clamp the total displacement within the defined limits
            float clampedDisplacement = Mathf.Clamp(totalDisplacement, strikerData.LeftLimit, strikerData.RightLimit);


            // Use the clamped displacement for your existing logic
            float slope = (clampedDisplacement - strikerData.LeftLimit) / (strikerData.RightLimit - strikerData.LeftLimit);
            Vector3 finalpos = Vector3.Lerp(StrikerPositions[0].transform.position, StrikerPositions[StrikerPositions.Count - 1].transform.position, slope);
            float leftDistance = Vector3.Distance(finalpos, StrikerPositions[0].transform.position);
            float rightDistance = Vector3.Distance(finalpos, StrikerPositions[StrikerPositions.Count - 1].transform.position);

            if (leftDistance > rightDistance)
            {
                transform.position = FindStrikerNextPosition(finalpos, -StrikerPositions[0].transform.right);
            }
            else
            {
                transform.position = FindStrikerNextPosition(finalpos, StrikerPositions[0].transform.right);
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

        public void AimStriker(SwipeDirection dir)
        {

            if (dir == SwipeDirection.LEFT)
            {
                transform.RotateAround(transform.position, transform.up, val);
            }
            else
            {
                transform.RotateAround(transform.position, transform.up, -val);
            }
        }

        public void AimStriker(float yAngle)
        {
            transform.rotation = StrikerPositions[2].transform.rotation;
            transform.Rotate(transform.up, yAngle);
            transform.eulerAngles = VectorUtility.RoundPositionUpto3Decimals(transform.eulerAngles);
        }

        /// <summary>
        /// Snap-rotate to the given direction's yaw only when the change exceeds the threshold.
        /// - If |delta| &gt;= threshold: snap directly to the target yaw.
        /// - Else: do nothing (ignore minor changes).
        /// Subsequent updates only occur when the new target deviates from the last applied yaw by the threshold again.
        /// </summary>
        public void RotateTo(Vector3 direction)
        {
            // Ignore invalid/zero directions
            if (direction.sqrMagnitude < 1e-8f)
            {
                return;
            }

            // Work on the XZ plane to avoid pitch/roll changes
            Vector3 flatDir = new Vector3(direction.x, 0f, direction.z);
            if (flatDir.sqrMagnitude < 1e-8f)
            {
                return;
            }
            flatDir.Normalize();

            // Compute target yaw
            float targetYaw = Mathf.Atan2(flatDir.x, flatDir.z) * Mathf.Rad2Deg;

            // Compare against the last yaw we actually applied (not the current noisy rotation)
            float deltaYawFromLast = Mathf.DeltaAngle(_lastAppliedYaw, targetYaw);
            if (Mathf.Abs(deltaYawFromLast) < yawThresholdDegrees)
            {
                // Below threshold: ignore to minimize shaking
                return;
            }

            // Snap directly to the new target yaw
            var euler = transform.eulerAngles;
            transform.eulerAngles = new Vector3(euler.x, targetYaw, euler.z);

            // Remember the last applied yaw to enforce threshold on future updates
            _lastAppliedYaw = targetYaw;
        }




        public void GetPositions(int id)
        {
            if (id == 1)
            {
                StrikerPositions = boardProperties.GetStrikerPosition(StrikerName.Striker1);
                gameObject.name = "Striker1";
            }
            else if (id == 2)
            {
                StrikerPositions = boardProperties.GetStrikerPosition(StrikerName.Striker2);
                gameObject.name = "Striker2";
            }
            else if (id == 3)
            {
                StrikerPositions = boardProperties.GetStrikerPosition(StrikerName.Striker3);
                gameObject.name = "Striker3";
            }
            else if (id == 4)
            {
                StrikerPositions = boardProperties.GetStrikerPosition(StrikerName.Striker4);
                gameObject.name = "Striker4";
            }

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
        }

    }
}



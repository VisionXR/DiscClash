using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.GameElements
{
    public class TutorialMovement : MonoBehaviour
    {
        public BoardPropertiesSO boardProperties;
        [SerializeField] private List<GameObject> StrikerPositions = new List<GameObject>();
        [SerializeField] private float val = 1;
        [SerializeField] public int strikerId = 1;
        [SerializeField] private float LeftLimit, RightLimit;
        [SerializeField] private bool isFired = false;
        [SerializeField] private float ForceLowerLimit = 0.2f;
        [SerializeField] private float ForceUpperLimit = 5;
        [SerializeField] private float StrikeForce = 2;
        public Action StrikeStarted, StrikeFinished;
        Rigidbody rb;
        public float period = 10.0f;
        private float startTime;
        private Coroutine FireRoutine;
        Vector3 fixedCenterPoint;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        private void OnEnable()
        {
            //EventManager.Swiped += MoveStriker;
            //EventManager.SwipedPosition += MoveStriker;
            //EventManager.KeyboardRotated += AimStriker;
            //EventManager.ControllerRotated += AimStriker;
            //EventManager.FireStriker += FireStriker;
            //EventManager.FireStrikerWithForce += FireStriker;


        }
        private void OnDisable()
        {
            //EventManager.Swiped -= MoveStriker;
            //EventManager.SwipedPosition -= MoveStriker;
            //EventManager.KeyboardRotated -= AimStriker;
            //EventManager.ControllerRotated -= AimStriker;
            //EventManager.FireStriker -= FireStriker;
            //EventManager.FireStrikerWithForce -= FireStriker;

        }

        public void RegisterOnlyMovement()
        {
            //EventManager.KeyboardRotated -= AimStriker;
            //EventManager.ControllerRotated -= AimStriker;
            //EventManager.FireStriker -= FireStriker;
            //EventManager.FireStrikerWithForce -= FireStriker;
        }

        public void SetStrikerId(int id)
        {
            strikerId = id;
            fixedCenterPoint = boardProperties.GetPlayerPosition(strikerId).position;
            GetPositions(id);
            ResetStriker();

        }
        public void RegisterOnlyAiming()
        {
            //    EventManager.KeyboardRotated += AimStriker;
            //    EventManager.ControllerRotated += AimStriker;
            //
        }
        public void RegisterStriking()
        {
            //EventManager.FireStriker += FireStriker;
            //EventManager.FireStrikerWithForce += FireStriker;
        }
        private void FireStriker(float force)
        {

            if (force > 0.5f && !isFired)
            {
                isFired = true;
                startTime = Time.time;
                FireRoutine = StartCoroutine(WaitAndChangeArrow());
            }

            else if (force < 0.1f && isFired)
            {
                StopCoroutine(FireRoutine);
                isFired = false;
                StrikeStarted?.Invoke();
                //   InputManager.instance.DeactivateInput();
                rb.AddForce(transform.forward * StrikeForce, ForceMode.VelocityChange);
                StartCoroutine(WaituntilStrikeFinished());
            }
        }

        private IEnumerator WaitAndChangeArrow()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                float timeSinceStart = Time.time - startTime;
                float period = 2f; // Time taken to complete one full cycle
                float t = timeSinceStart / period; // Normalized time between 0 and 1

                // Linearly interpolate between 0 and 1 and then back to 0
                float normalizedValue = Mathf.PingPong(t, 1f);

                // Map the normalized value to the desired range
                float range = ForceUpperLimit - ForceLowerLimit;
                StrikeForce = ForceLowerLimit + normalizedValue * range;

                //   EventManager.StrikeForceChanged?.Invoke(normalizedValue);
            }
        }

        private void MoveStriker(Vector3 controllerPositiom, Transform cameraRigTransform)
        {
            // Displacement of the camera rig from the fixed center point along the camera's right vector
            float cameraRigDisplacement = Vector3.Dot(cameraRigTransform.position - fixedCenterPoint, cameraRigTransform.right);

            // Displacement of the controller from the camera rig along the camera's right vector
            float controllerDisplacement = Vector3.Dot(controllerPositiom, cameraRigTransform.right);

            // Total displacement from the fixed center point
            float totalDisplacement = cameraRigDisplacement + controllerDisplacement;

            // Clamp the total displacement within the defined limits
            float clampedDisplacement = Mathf.Clamp(totalDisplacement, LeftLimit, RightLimit);


            // Use the clamped displacement for your existing logic
            float slope = (clampedDisplacement - LeftLimit) / (RightLimit - LeftLimit);
            Vector3 finalpos = Vector3.Lerp(StrikerPositions[0].transform.position, StrikerPositions[4].transform.position, slope);
            float leftDistance = Vector3.Distance(finalpos, StrikerPositions[0].transform.position);
            float rightDistance = Vector3.Distance(finalpos, StrikerPositions[4].transform.position);

            if (leftDistance > rightDistance)
            {
                transform.position = FindStrikerNextPosition(finalpos, -StrikerPositions[0].transform.right);
            }
            else
            {
                transform.position = FindStrikerNextPosition(finalpos, StrikerPositions[0].transform.right);
            }
        }
        private void MoveStriker(SwipeDirection swipeDirection)
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
                float lower = Mathf.Min(StrikerPositions[0].transform.position.x, StrikerPositions[4].transform.position.x);
                float upper = Mathf.Max(StrikerPositions[0].transform.position.x, StrikerPositions[4].transform.position.x);
                if (finalPos.x >= lower && finalPos.x <= upper)
                {
                    transform.position = new Vector3(finalPos.x, StrikerPositions[0].transform.position.y, StrikerPositions[0].transform.position.z);
                }
            }
            else
            {
                float lower = Mathf.Min(StrikerPositions[0].transform.position.z, StrikerPositions[4].transform.position.z);
                float upper = Mathf.Max(StrikerPositions[0].transform.position.z, StrikerPositions[4].transform.position.z);
                if (finalPos.z >= lower && finalPos.z <= upper)
                {
                    transform.position = new Vector3(StrikerPositions[0].transform.position.x, StrikerPositions[0].transform.position.y, finalPos.z);
                }
            }

        }
        private void AimStriker(SwipeDirection dir)
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
        private void AimStriker(float yAngle)
        {
            transform.rotation = StrikerPositions[2].transform.rotation;
            transform.Rotate(transform.up, yAngle);
        }
        private void FireStriker()
        {
            StrikeStarted?.Invoke();
            //   InputManager.instance.DeactivateInput();
            rb.AddForce(transform.forward * 4, ForceMode.VelocityChange);
            StartCoroutine(WaituntilStrikeFinished());
        }
        public void FireStriker(Vector3 direction, float force)
        {
            StrikeStarted?.Invoke();
            //  InputManager.instance.DeactivateInput();
            rb.AddForce(direction * force, ForceMode.VelocityChange);
            StartCoroutine(WaituntilStrikeFinished());
        }

        private IEnumerator WaituntilStrikeFinished()
        {
            //EventManager.Swiped -= MoveStriker;
            //EventManager.KeyboardRotated -= AimStriker;
            yield return new WaitUntil(() => rb.linearVelocity.magnitude < 0.0002f);
            yield return new WaitForSeconds(5);
            //EventManager.Swiped += MoveStriker;
            //EventManager.KeyboardRotated += AimStriker;
            StrikeFinished?.Invoke();
        }

        public List<GameObject> GetStrikerPositions()
        {
            return StrikerPositions;
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
                Collider[] cols = Physics.OverlapSphere(newPosition, boardProperties.GetStrikerRadius() + 0.005f);
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
                float lower = Mathf.Min(StrikerPositions[0].transform.position.x, StrikerPositions[4].transform.position.x);
                float upper = Mathf.Max(StrikerPositions[0].transform.position.x, StrikerPositions[4].transform.position.x);
                if (finalPos.x >= lower && finalPos.x <= upper)
                {
                    newPosition = new Vector3(finalPos.x, StrikerPositions[0].transform.position.y, StrikerPositions[0].transform.position.z);
                }
            }
            else
            {
                float lower = Mathf.Min(StrikerPositions[0].transform.position.z, StrikerPositions[4].transform.position.z);
                float upper = Mathf.Max(StrikerPositions[0].transform.position.z, StrikerPositions[4].transform.position.z);
                if (finalPos.z >= lower && finalPos.z <= upper)
                {
                    newPosition = new Vector3(StrikerPositions[0].transform.position.x, StrikerPositions[0].transform.position.y, finalPos.z);
                }
            }
            return newPosition;
        }
        public void ResetStriker()
        {
            transform.position = FindStrikerNextPosition(StrikerPositions[2].transform.position, StrikerPositions[2].transform.right);
            transform.rotation = StrikerPositions[2].transform.rotation;
        }
    }
}




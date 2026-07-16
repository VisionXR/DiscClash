using com.VisionXR.ModelClasses;
using System.Collections;
using TMPro;
using UnityEngine;

namespace com.VisionXR.GameElements
{
    public class AllCoinsRotation : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public CoinDataSO coinData;
        public InputDataSO inputData;
        public MyPlayerSettings playerSettings;

        [Header("Local Objects")]
        public GameObject RotDisplayCanvas;
        public TMP_Text displayText;
        public float amplitude = 30f; // Maximum amplitude of oscillation.
        public float frequency = 0.5f; // Frequency of oscillation in cycles per second.
        public float Duration = 5;
        private float timeElapsed = 0f;
        private bool canIRotate = false;

        // Stores the base angle offset based on player ID
        private float baseAngle = 0f;

        private void OnEnable()
        {
            coinData.ShowRotationCanvasEvent += StartRotation;
            coinData.RotateCoinsEvent += RotateCoins;
            coinData.SetAllCoinsRotationEvent += SetRotation;

            displayText.text = "Swipe here to Rotate All Coins";
        }

        private void OnDisable()
        {
            coinData.ShowRotationCanvasEvent -= StartRotation;
            coinData.RotateCoinsEvent -= RotateCoins;
            coinData.SetAllCoinsRotationEvent -= SetRotation;
        }

        public void RotateCoins(float value)
        {
            transform.Rotate(Vector3.up, value);
            coinData.AllCoinsYRotationValue = transform.eulerAngles.y;
        }

        public void SetRotation(float YRot)
        {
            transform.eulerAngles = new Vector3(0, YRot, 0);
        }

        public void StartRotation(int id)
        {
            canIRotate = true;
            timeElapsed = 0f; // Reset time elapsed in case it's called multiple times

            // Determine base starting angle based on player ID
            if (id == 2)
            {
                baseAngle = 180f;
            }
            else if (id == 3)
            {
                baseAngle = -90f;
            }
            else
            {
                baseAngle = 0f; // Default (e.g., ID 1)
            }

            // Set the initial canvas rotation to its default state before oscillation
            RotDisplayCanvas.transform.rotation = Quaternion.Euler(0f, baseAngle, 0f);
            ShowRotationCanvas();
        }

        public void ShowRotationCanvas()
        {
            RotDisplayCanvas.SetActive(true);
            StartCoroutine(WaitAndHide());
        }

        private IEnumerator WaitAndHide()
        {
            yield return new WaitForSeconds(Duration); // Use your Duration variable here instead of hardcoded 5
            canIRotate = false;
            HideRotationCanvas();
        }

        public void HideRotationCanvas()
        {
            RotDisplayCanvas.SetActive(false);
        }

        private void Update()
        {
            if (canIRotate)
            {
                timeElapsed += Time.deltaTime;

                if (timeElapsed <= Duration)
                {
                    // Calculate the oscillating offset angle
                    float oscillatingOffset = Mathf.Sin(2 * Mathf.PI * frequency * timeElapsed) * amplitude;

                    // Add the base offset to the oscillation so it rotates relative to its starting layout
                    float finalAngle = baseAngle + oscillatingOffset;

                    // Apply the accumulated rotation to the object
                    RotDisplayCanvas.transform.rotation = Quaternion.Euler(0f, finalAngle, 0f);
                }
            }
        }
    }
}
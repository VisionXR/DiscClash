using com.VisionXR.ModelClasses;
using System.Collections;
using TMPro;
using UnityEngine;


namespace com.VisionXR.GameElements
{
    public class AllCoinsRotation : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public CoinDataSO coinData;
        public InputDataSO inputData;
        public MyPlayerSettings playerSettings;
        
        [Header(" Local Objects")]      
        public GameObject RotDisplayCanvas;
        public TMP_Text displayText;
        public float amplitude = 30f; // Maximum amplitude of oscillation.
        public float frequency = 0.5f; // Frequency of oscillation in cycles per second.
        public float Duration = 5;
        private float timeElapsed = 0f;
        private bool canIRotate = false;


        private void OnEnable()
        {
            coinData.ShowRotationCanvasEvent += StartRotation;
         
            coinData.RotateCoinsEvent += RotateCoins;
            coinData.SetAllCoinsRotationEvent += SetRotation;


            if(inputData.isHandTrackingActive)
            {
                if(playerSettings.myDominantHand == HelperClasses.DominantHand.RIGHT)
                {
                    displayText.text = "Use Left Hand Up/Down MicroGesture To " +"\n"+"Rotate All Coins";
                }
                else
                {
                    displayText.text = "Use Right Hand Up/Down MicroGesture To "  +"\n"+ "Rotate All Coins";
                }
            }
            else
            {
                if (playerSettings.myDominantHand == HelperClasses.DominantHand.RIGHT)
                {
                    displayText.text = "Use Left JoyStick Up/Down To Rotate All Coins";
                }
                else
                {
                    displayText.text = "Use Right JoyStick Up/Down To Rotate All Coins";
                }
            }
        }

        private void OnDisable()
        {
            coinData.ShowRotationCanvasEvent -= StartRotation;
          
            coinData.RotateCoinsEvent -= RotateCoins;
            coinData.SetAllCoinsRotationEvent -= SetRotation;
        }

        public void RotateCoins(float value)
        {
            if (inputData.isHandTrackingActive)
            {
                transform.Rotate(Vector3.up, value*10);
            }
            else
            {
                transform.Rotate(Vector3.up, value);
            }

            coinData.AllCoinsYRotationValue = transform.eulerAngles.y;           
        }
        public void SetRotation(float YRot)
        {
            transform.eulerAngles = new Vector3(0, YRot, 0);
        }

        public void StartRotation()
        {
            canIRotate = true;
            ShowRotationCanvas();
        }
        public void ShowRotationCanvas()
        {          
            RotDisplayCanvas.SetActive(true);
            StartCoroutine(WaitAndHide());
        }

        private IEnumerator WaitAndHide()
        {
            yield return new WaitForSeconds(5);
            canIRotate = false;
            HideRotationCanvas();
        }
        public void HideRotationCanvas()
        {       
            RotDisplayCanvas.SetActive(false);
        }

        private void Update()
        {
            // Increment the time elapsed.
            if (canIRotate)
            {
                timeElapsed += Time.deltaTime;

                if (timeElapsed <= Duration)
                {
                    // Calculate the angle based on time and frequency.
                    float angle = Mathf.Sin(2 * Mathf.PI * frequency * timeElapsed) * amplitude;

                    // Apply the rotation to the object.
                    RotDisplayCanvas.transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }
            }

        }

    }
}

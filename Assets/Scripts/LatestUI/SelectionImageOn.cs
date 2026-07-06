using System.Collections;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.UI;


public class SelectionImageOn : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIDataSO uiData;
        private Image selectionImage;

        void OnEnable()
        {
            selectionImage = GetComponent<Image>();
            StartCoroutine(AnimateImageRoutine());
        }

    private void OnDisable()
    {
           
            if (selectionImage != null)
            {
                selectionImage.color = uiData.defaultColor;
                selectionImage.transform.localScale = new Vector3(0, 0, 0);
            }

    }

    private IEnumerator AnimateImageRoutine()
        {
            // 1. Initial State: White background, scale X is 0
            selectionImage.color = uiData.defaultColor;
            selectionImage.transform.localScale = new Vector3(0, 0, 0);

         
            float elapsed = 0f;

            while (elapsed < uiData.disableTime)
            {
                elapsed += Time.deltaTime;
                selectionImage.color = Color.Lerp(uiData.defaultColor, uiData.selectionColor, elapsed / uiData.disableTime);
             selectionImage.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), elapsed / uiData.disableTime);
            yield return null;
            }

            selectionImage.color = uiData.selectionColor;
        }
    }

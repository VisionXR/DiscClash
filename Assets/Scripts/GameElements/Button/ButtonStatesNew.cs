using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonStatesNew : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scritable Objects")]
    public AppDataSO appPropertiesData;

    [Header("UI Elements")]
    [SerializeField] private Image HoverImage;
    [SerializeField] private Image ActualImage;
    
    

    // local variables
    public bool isHovering = false;
    // popout/bring-forward
    [Header("Popout")]
    [Tooltip("Scale multiplier applied to ActualImage on hover")]
    public float popScale = 1.08f;
    [Tooltip("Popout lerp duration in seconds")]
    public float popDuration = 0.12f;
    private Coroutine popRoutine = null;
    private Vector3 originalScale = Vector3.one;


    void OnDisable()
    {
        if (isHovering)
        {
            isHovering = false;
            if (HoverImage != null) HoverImage.gameObject.SetActive(false);

            if (popRoutine != null) { StopCoroutine(popRoutine); popRoutine = null; }
            // reset scale if needed
            if (ActualImage != null)
            {
                ActualImage.rectTransform.localScale = originalScale;
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {

        if (!isHovering)
        {
            appPropertiesData.StartVibration();
            isHovering = true;
            if (HoverImage != null) HoverImage.gameObject.SetActive(true);
          

            // scale/pop the actual image slightly toward the user
            if (ActualImage != null)
            {
                if (originalScale == Vector3.one) originalScale = ActualImage.rectTransform.localScale;
                if (popRoutine != null) StopCoroutine(popRoutine);
                popRoutine = StartCoroutine(ScaleRoutine(ActualImage.rectTransform, originalScale * popScale, popDuration));
            }
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
        if (isHovering)
        {
            isHovering = false;
            if (HoverImage != null)
            {
                // hide hover visual after reverting scale
                HoverImage.gameObject.SetActive(false);
            }

            if (ActualImage != null)
            {
                if (popRoutine != null) StopCoroutine(popRoutine);
                popRoutine = StartCoroutine(ScaleRoutine(ActualImage.rectTransform, originalScale, popDuration));
            }
        }
       
    }

    private IEnumerator ScaleRoutine(RectTransform rt, Vector3 targetScale, float duration)
    {
        if (rt == null) yield break;
        Vector3 start = rt.localScale;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float f = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / duration));
            rt.localScale = Vector3.Lerp(start, targetScale, f);
            yield return null;
        }
        rt.localScale = targetScale;
        popRoutine = null;
    }

}

using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PanelOnOff : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIDataSO uiData;
    public List<GameObject> subPanels;
    public CanvasGroup panelCanvasGroup;


    private void OnEnable()
    {
        panelCanvasGroup.alpha = 0;
        foreach (GameObject panel in subPanels)
        {
            panel.transform.localScale = Vector3.zero;
        }
    }

    public void TurnOnPanel()
    {
      

        panelCanvasGroup.gameObject.SetActive(true);

        StartCoroutine(TurnOnAlpha());

        foreach (GameObject panel in subPanels)
        {           
            StartCoroutine(ScaleUpAnimation(panel));
        }
    }

    public void TurnOffPanel()
    {
        if(!panelCanvasGroup.gameObject.activeInHierarchy)
        {
            return;
        }

        foreach (GameObject panel in subPanels)
        {
            StartCoroutine(ScaleDownAnimation(panel));
        }

        StartCoroutine(TurnOffAlpha());

    }

    private IEnumerator TurnOnAlpha()
    {
       
        float elapsed = 0f;
        panelCanvasGroup.alpha = 0f;
        while (elapsed < uiData.disableTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / uiData.disableTime;
            panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
        panelCanvasGroup.alpha = 1f;
    }

    private IEnumerator TurnOffAlpha()
    {
        float elapsed = 0f;
        panelCanvasGroup.alpha = 1f;
        while (elapsed < uiData.disableTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / uiData.disableTime;
            panelCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
        panelCanvasGroup.alpha = 0f;
        panelCanvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator ScaleDownAnimation(GameObject panel)
    {
       
        float elapsed = 0f;
        panel.transform.localScale = Vector3.one;
        Vector3 startScale = Vector3.one;
        Vector3 targetScale = Vector3.zero;

      

        while (elapsed < uiData.disableTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / uiData.disableTime;
            panel.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
           
            yield return null;
        }

        panel.transform.localScale = targetScale;
       
     
    }

    private IEnumerator ScaleUpAnimation(GameObject panel)
    {
        yield return new WaitForSeconds(uiData.disableTime);

        float elapsed = 0f;
        panel.transform.localScale = Vector3.zero;
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;
       

        while (elapsed < uiData.disableTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / uiData.disableTime;
            panel.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
          
            yield return null;
        }

        panel.transform.localScale = targetScale;
      

    }

}

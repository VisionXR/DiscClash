using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasUIManager : MonoBehaviour
{

    [Header(" Scriptable Objects")]
    public UIOutputDataSO uiOutputData;
    public UIInputDataSO uiInputData;
    

    [Header(" Main Panel Objects")]
    public GameObject HomePanel;
    public GameObject LoginPanel;



    [Header(" All Panel Objects")]
    public List<GameObject> allPanels;

    [Header(" Home Panel Objects")]
    public List<GameObject> homePanels;


    private void OnEnable()
    {
        uiInputData.HomeEvent += ShowHomePanel;
        uiInputData.ExitGameEvent += ShowHomePanel;
        uiInputData.ShowLoginEvent += ShowLoginPanel;
    }

    private void OnDisable()
    {
        uiInputData.HomeEvent -= ShowHomePanel;
        uiInputData.ExitGameEvent -= ShowHomePanel;
        uiInputData.ShowLoginEvent -= ShowLoginPanel;
    }

    public void ShowHomePanel()
    {
       
        ResetHomePanels();
        ResetAllPanels();
        HomePanel.SetActive(true);

      
    }

    public void ShowLoginPanel()
    {
        ResetHomePanels();
        ResetAllPanels();
        LoginPanel.SetActive(true);
     
    }

    private void ResetAllPanels()
    {
        foreach (GameObject go in allPanels)
        {
            go.SetActive(false);
        }
    }

    private void ResetHomePanels()
    {
        foreach (GameObject go in homePanels)
        {
            go.SetActive(false);
        }
    }
}

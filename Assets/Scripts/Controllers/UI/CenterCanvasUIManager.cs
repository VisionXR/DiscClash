using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CenterCanvasUIManager : MonoBehaviour
{

    [Header(" Scriptable Objects")]
    public UIOutputDataSO uiOutputData;
    public UIInputDataSO uiInputData;
    public OculusDataSO oculusData;

    [Header(" Center Panel Objects")]
    public GameObject LoadingPanel;
    public GameObject DestinationFailedPanel;
    public GameObject DestinationChangePanel;

    [Header(" Main Panel Objects")]
    public List<GameObject> allPanles;

    // Actions;
    public Action OnLoadingSuccessEvent;
    public Action OnLoadingFailedEvent;


    private void OnEnable()
    {
        OnLoadingSuccessEvent += LoadingSuccess;
        OnLoadingFailedEvent += LoadingFailed;
    }


    private void OnDisable()
    {
        OnLoadingSuccessEvent -= LoadingSuccess;
        OnLoadingFailedEvent -= LoadingFailed;
    }

    private void LoadingSuccess()
    {
        ResetPanels();
        gameObject.SetActive(false);
    }

    private void LoadingFailed()
    {
        ResetPanels();
        DestinationFailedPanel.SetActive(false);
    }

    public  void ShowChangeDestinationPanel(Destination destination)
    {

        ResetPanels();
        DestinationChangePanel.SetActive(true);
        DestinationChangePanel.GetComponent<ChangeDestination>().SetDestination(destination);
    }

    public void ShowConnectDestinationPanel(Destination destination)
    {

        ResetPanels();
        LoadingPanel.SetActive(true);
        oculusData.ConnectToDestination(destination, OnLoadingSuccessEvent, OnLoadingFailedEvent);
    }


    private void ResetPanels()
    {
        foreach (GameObject go in allPanles)
        {
            go.SetActive(false);
        }
    }
}

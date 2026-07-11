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
    public PlayersDataSO playerData;
    public DestinationDataSO destinationData;


    [Header(" Center Panel Objects")]
    public GameObject LoadingPanel;
    public GameObject DestinationChangePanel;
    public GameObject GameResultPanel;
    public GameObject OtherPlayerDisconnectPanel;
    public GameObject InternetDisconnectedPanel;

    [Header(" Main Panel Objects")]
    public List<GameObject> allPanles;

    // Actions;

    public void ShowOtherPlayerDisconnection()
    {
       

    }

    public void ShowDestinationChangePanel(Destination destination)
    {
        DestinationChangePanel.SetActive(true);
        DestinationChangePanel.GetComponent<DestinationPanelView>().ConnectToDestination(destination);
    }

    public void ShowLoadingPanel()
    {
        LoadingPanel.SetActive(true);
    }


    private void ResetPanels()
    {
        foreach (GameObject go in allPanles)
        {
            go.SetActive(false);
        }
    }
}

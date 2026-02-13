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

    [Header(" Main Panel Objects")]
    public List<GameObject> allPanles;

    // Actions;



    private void OnEnable()
    {

        uiInputData.ShowGameResultEvent += ShowGameResult;
        uiInputData.OtherPlayerLeftGameEvent += ShowOtherPlayerDisconnection;

        uiInputData.HomeEvent += ResetPanels;
        uiInputData.ExitGameEvent += ResetPanels;

        uiInputData.ShowDestinationPanelEvent += ShowDestinationChangePanel;
        uiInputData.ShowLoadingPanelEvent += ShowLoadingPanel;
    }


    private void OnDisable()
    {

        uiInputData.ShowGameResultEvent -= ShowGameResult;
        uiInputData.OtherPlayerLeftGameEvent -= ShowOtherPlayerDisconnection;

        uiInputData.HomeEvent -= ResetPanels;
        uiInputData.ExitGameEvent -= ResetPanels;

        uiInputData.ShowDestinationPanelEvent -= ShowDestinationChangePanel;
        uiInputData.ShowLoadingPanelEvent -= ShowLoadingPanel;
    }


    public void ShowGameResult( GameResult result)
    {
        GameResultPanel.SetActive(true);
        GameResultPanel.GetComponent<GameResultPanelView>().ShowResult(result);
    }

    public void ShowOtherPlayerDisconnection()
    {
        if (uiOutputData.multiPlayerGameMode != MultiPlayerGameMode.P1P2vsP3P4)
        {

            OtherPlayerDisconnectPanel.SetActive(true);
           
        }
        else
        {
            if (playerData.CurrentPlayers.Count < 3)
            {
                OtherPlayerDisconnectPanel.SetActive(true);              
            }
        }

    }

    public void ShowDestinationChangePanel(Destination destination)
    {
        DestinationChangePanel.SetActive(true);
        DestinationChangePanel.GetComponent<ChangeDestination>().ConnectToDestination(destination);
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

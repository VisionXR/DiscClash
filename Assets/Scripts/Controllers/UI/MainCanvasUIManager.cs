using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasUIManager : MonoBehaviour
{

    [Header(" Scriptable Objects")]
    public UIOutputDataSO uiOutputData;
    public UIInputDataSO uiInputData;
    public OculusDataSO oculusData;


    [Header(" Main Panel Objects")]
    public GameObject TwoPlayerPanel;
    public GameObject FourPlayerPanel;
    public GameObject HomePanel;
    public GameObject MainPanel;
    public GameObject MainSettingsPanel;


    [Header(" All Panel Objects")]
    public List<GameObject> allPanels;

    [Header(" Home Panel Objects")]
    public List<GameObject> homePanels;


    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    public void ShowHome()
    {
        Debug.Log(" showing home");
        ResetHomePanels();
        HomePanel.SetActive(true);
        MainPanel.SetActive(true);
        MainSettingsPanel.SetActive(true);
    }

    public void ShowSinglePlayerPanels()
    {
        ResetAllPanels();
        if(uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
        {
            TwoPlayerPanel.SetActive(true);
        }
        else
        {
            FourPlayerPanel.SetActive(true);
        }
    }

    public void ShowMultiPlayerPanels()
    {
        ResetAllPanels();
        if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
        {
            TwoPlayerPanel.SetActive(true);
        }
        else
        {
            FourPlayerPanel.SetActive(true);
        }
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

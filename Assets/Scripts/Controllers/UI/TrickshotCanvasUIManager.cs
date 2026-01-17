using com.VisionXR.ModelClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrickshotCanvasUIManager : MonoBehaviour
{

    [Header(" Scriptable Objects")]
    public TrickShotsDataSO trickShotData;
    public UIOutputDataSO uiOutputData;
    public UIInputDataSO uiInputData;
    public OculusDataSO oculusData;


    [Header(" UI Objects")]
    public GameObject levelsPanel;
    public GameObject strikersPanel;
   

    [Header(" All Objects")]
    public List<GameObject> allPanels;



    public void ShowStartPanel()
    {
        ResetPanels();
        allPanels[0].SetActive(true);   
    }

    public void NextBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        strikersPanel.SetActive(false);
        levelsPanel.SetActive(true);
    }

    public void BackBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        uiOutputData.StopTrickShotsEvent?.Invoke();
        uiOutputData.GoToHome();
    }

    public void GoToLevelsPanel()
    {
        AudioManager.instance.PlayButtonClickSound();
        ResetPanels();
        levelsPanel.SetActive(true);
    }

    public void GoToStrikersPanel()
    {
        AudioManager.instance.PlayButtonClickSound();
        ResetPanels();
        strikersPanel.SetActive(true);
    }

    private void ResetPanels()
    {
        foreach (GameObject go in allPanels)
        {
            go.SetActive(false);
        }
    }
}

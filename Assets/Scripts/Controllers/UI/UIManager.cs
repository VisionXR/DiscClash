using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header(" Scriptable Objects")]   
    public UIOutputDataSO uiOutputData;
    public UIInputDataSO uiInputData;
    public OculusDataSO oculusData;


    [Header(" Canvas Objects")]
    public MainCanvasUIManager mainCanvasUIManager;
    public CenterCanvasUIManager centerCanvasUIManager;
    public TrickshotCanvasUIManager trickshotCanvasUIManager;
    public TutorialCanvasUIManager tutorialCanvasUIManager;
    
  


    private void OnEnable()
    {

        uiOutputData.HomeEvent += GoToHome;
        uiOutputData.ExitGameEvent += GoToHome;

        uiOutputData.StartFTUEEvent += StartFTUE;
        uiOutputData.StartTutorialEvent += StartTutorial;
        uiOutputData.StartSinglePlayerGameEvent += StartSP;
        uiOutputData.StartTrickShotsEvent += StartTrickShots;
        uiOutputData.StartMultiPlayerGameEvent += StartMP;
        uiOutputData.EndTutorialEvent += GoToHome;

        uiInputData.ConnectToDestinationEvent += ShowConnectDestinationPanel;
        uiInputData.ChangeDestinationEvent += ShowChangeDestinationPanel;
    }

    private void OnDisable()
    {

        uiOutputData.HomeEvent -= GoToHome;
        uiOutputData.ExitGameEvent -= GoToHome;

        uiOutputData.StartFTUEEvent -= StartFTUE;
        uiOutputData.StartTutorialEvent -= StartTutorial;
        uiOutputData.StartSinglePlayerGameEvent -= StartSP;
        uiOutputData.StartTrickShotsEvent -= StartTrickShots;
        uiOutputData.StartMultiPlayerGameEvent -= StartMP;
        uiOutputData.EndTutorialEvent -= GoToHome;

        uiInputData.ConnectToDestinationEvent -= ShowConnectDestinationPanel;
        uiInputData.ChangeDestinationEvent -= ShowChangeDestinationPanel;
    }

    private void StartMP()
    {
        ResetCanvases();
        mainCanvasUIManager.gameObject.SetActive(true);
        mainCanvasUIManager.ShowMultiPlayerPanels();
    }

    private void StartTrickShots()
    {
        ResetCanvases();
        trickshotCanvasUIManager.gameObject.SetActive(true);
        trickshotCanvasUIManager.ShowStartPanel();
    }

    private void StartSP()
    {
        ResetCanvases();
        mainCanvasUIManager.gameObject.SetActive(true);
        mainCanvasUIManager.ShowSinglePlayerPanels();

    }

    private void StartFTUE(Destination destination)
    {
        ResetCanvases();
        tutorialCanvasUIManager.gameObject.SetActive(true);
    }

    private void StartTutorial()
    {
        ResetCanvases();
        tutorialCanvasUIManager.gameObject.SetActive(true);
    }

    private void GoToHome()
    {
        ResetCanvases();
        mainCanvasUIManager.gameObject.SetActive(true);
        mainCanvasUIManager.ShowHome();
    }

    private void ShowChangeDestinationPanel(Destination destination)
    {
        ResetCanvases();
        centerCanvasUIManager.gameObject.SetActive(true);
        centerCanvasUIManager.ShowChangeDestinationPanel(destination);
    }

    private void ShowConnectDestinationPanel(Destination destination)
    {
        ResetCanvases();
        uiOutputData.gameType = destination.gameType;
        uiOutputData.game = destination.game;
        uiOutputData.singlePlayerGameMode = destination.singlePlayerGameMode;
        uiOutputData.multiPlayerGameMode = destination.multiPlayerGameMode;

      
        centerCanvasUIManager.gameObject.SetActive(true);
        centerCanvasUIManager.ShowConnectDestinationPanel(destination);
    }



    private void ResetCanvases()
    {
        mainCanvasUIManager.gameObject.SetActive(false);
        centerCanvasUIManager.gameObject.SetActive(false);
        trickshotCanvasUIManager.gameObject.SetActive(false);
        tutorialCanvasUIManager.gameObject.SetActive(false);       
        
    }
}

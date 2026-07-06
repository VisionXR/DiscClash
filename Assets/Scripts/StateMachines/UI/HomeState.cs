using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class HomeState : StateMachineBehaviour
{
   
    public UIDataSO uiData;
    public int mainCanvasId = 0;
    public int homePanelId = 0;

    public StateName currentStateName;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      
        if (uiData.uiManager != null)
        {
            uiData.uiManager.ShowCanvas(0);
            uiData.uiManager.mainCanvasView.ShowMainPanel(homePanelId);
            uiData.uiManager.SetCurrentStateName(currentStateName);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
               
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (uiData.uiManager != null)
        {
            uiData.uiManager.mainCanvasView.HideMainPanel(homePanelId);
            uiData.uiManager.SetPreviousStateName(currentStateName);
        }
    }

}

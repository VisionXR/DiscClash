using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class PauseState : StateMachineBehaviour
{
   
    public UIDataSO uiData;
    public int mainCanvasId = 0;
    public int poolCanvasId = 1;
    public int SnookerCanvasId = 2;
    public int pauseState = 20;
    public StateName currentStateName;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     
        if (uiData.uiManager != null)
        {
            uiData.uiManager.HideCanvas(poolCanvasId);
            uiData.uiManager.HideCanvas(SnookerCanvasId);
            uiData.uiManager.ShowCanvas(mainCanvasId);
            uiData.uiManager.mainCanvasView.ShowMainPanel(pauseState);
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
            uiData.uiManager.mainCanvasView.HideMainPanel(pauseState);
            uiData.uiManager.SetPreviousStateName(currentStateName);
        }
    }


}

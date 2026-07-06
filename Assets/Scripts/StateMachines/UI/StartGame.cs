using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class StartGame : StateMachineBehaviour
{
   
    public UIDataSO uiData;
    public int mainCanvasId = 0;
    public int poolCanvasId = 1;
    public int SnookerCanvasId = 2;
    public StateName currentStateName;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      
        if (uiData.uiManager != null)
        {
            uiData.uiManager.HideCanvas(0);

            //if(uiData.currentGameMode == GameMode.Pool)
            //{
            //    uiData.uiManager.ShowCanvas(poolCanvasId);
            //    uiData.uiManager.poolCanvasView.TurnOn();
            //}
            //else if(uiData.currentGameMode == GameMode.Snooker)
            //{
            //    uiData.uiManager.ShowCanvas(SnookerCanvasId);
            //    uiData.uiManager.snookerCanvasView.TurnOn();
            //}

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
            //if (uiData.currentGameMode == GameMode.Pool)
            //{
            //    uiData.uiManager.HideCanvas(poolCanvasId);
            //    uiData.uiManager.poolCanvasView.TurnOff();
            //}
            //else if (uiData.currentGameMode == GameMode.Snooker)
            //{
            //    uiData.uiManager.HideCanvas(SnookerCanvasId);
            //    uiData.uiManager.snookerCanvasView.TurnOff();
            //}

            uiData.uiManager.SetPreviousStateName(currentStateName);
        }
    }


}

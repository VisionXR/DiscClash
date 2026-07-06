using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class GenericState : StateMachineBehaviour
{
    public UIDataSO uiData;
    public int nextPanelId = 1;
    public StateName currentStateName;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     
        if (uiData.uiManager != null)
        {

           // uiData.uiManager.mainCanvasView.ShowMainPanel(nextPanelId);
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

          //  uiData.uiManager.mainCanvasView.HideMainPanel(nextPanelId);
            uiData.uiManager.SetPreviousStateName(currentStateName);
        }
    }

}

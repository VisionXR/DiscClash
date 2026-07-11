using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class StartGame : StateMachineBehaviour
{
    public UIOutputDataSO uIOutputData;
    public UIDataSO uiData;
    public int mainCanvasId = 0;
    public int scoreCanvas2Players = 1;
    public int scoreCanvas4Players = 2;
    public StateName currentStateName;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      
        if (uiData.uiManager != null)
        {
            uiData.uiManager.HideCanvas(0);

            if (uIOutputData.gameMode == GameMode.PvsAI || uIOutputData.gameMode == GameMode.P1vsP2)
            {
                uiData.uiManager.ShowCanvas(scoreCanvas2Players);
                uiData.uiManager.scorePanel2Player.TurnOn();
            }
            else 
            {
                uiData.uiManager.ShowCanvas(scoreCanvas4Players);
                uiData.uiManager.scorePanel4Player.TurnOn();
            }

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
            if (uIOutputData.gameMode == GameMode.PvsAI || uIOutputData.gameMode == GameMode.P1vsP2)
            {
                uiData.uiManager.HideCanvas(scoreCanvas2Players);
                uiData.uiManager.scorePanel2Player.TurnOff();
            }
            else
            {
                uiData.uiManager.HideCanvas(scoreCanvas4Players);
                uiData.uiManager.scorePanel4Player.TurnOff();
            }

            

            uiData.uiManager.SetPreviousStateName(currentStateName);
        }
    }


}

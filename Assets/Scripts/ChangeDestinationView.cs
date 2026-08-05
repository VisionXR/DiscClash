using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class ChangeDestinationView : MonoBehaviour
    {
        [Header("Scriptable Objects")] 
        public UIInputDataSO uIInputData;
        public UIDataSO uiData;

        [Header(" Local ")]
        public Destination newDestination;
        public DestinationPanelView destinationPanelView;


        public void SetDestination(Destination destination)
        {
            newDestination = destination;
        }

        public void JoinBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
          
            uiData.uiManager.ChangeState("SinglePlayer", false);
            uiData.uiManager.ChangeState("MultiPlayer", false);
            uiData.uiManager.ChangeState("JoinedLobby", false);
            uiData.uiManager.ResetAllBools();
            uIInputData.ExitGame();

       
            uiData.uiManager.GoToState(StateName.MPDestinationState);
            destinationPanelView.ConnectToDestination(newDestination);

        }

        public void ResumeBtnClicked()
        {

            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.GoToState(uiData.uiManager.previousStateName);
        }

    }
}

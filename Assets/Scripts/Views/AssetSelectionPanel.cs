using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class AssetSelectionPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public MyPlayerSettings playerSettings;
        public StrikerDataSO strikerData;
        public CoinDataSO coinData;
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public UIDataSO uiData;
        public DestinationDataSO destinationData;

        [Header("Local Objects")]
        public Destination destination;
        public string singlePlayerState;

 

        public void BoardBtnClicked()
        {

        }

        public void StrikerBtnClicked()
        {

        }

        public void CoinsBtnClciked()
        {

        }

        public void NextBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            destination.gameMode = uiOutputData.gameMode;
            destination.gameType = uiOutputData.gameType;
            destination.challenge = uiOutputData.challenge;
            destination.difficulty = uiOutputData.aIDifficulty;

            if(destination.gameType == GameType.VsCPU)
            {
                uiData.uiManager.ChangeState(singlePlayerState, true);
                destinationData.ConnectToDestination(destination,null,null);
            }
           
         
        }

        public void BackBtnClicked()
        {
            

        }
    }
}


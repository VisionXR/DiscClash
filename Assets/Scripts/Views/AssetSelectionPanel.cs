using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
        public Image currentBoardImage;
        public Image currentStrikerImage;
        public Image currentCoinsImage;
        public List<Sprite> coinUIImages;
        public List<Sprite> boardUIImages;
        public List<Sprite> strikerUIImages;

        [Header("States ")]
        public DestinationPanelView destinationPanelView;
        public Destination destination;
        public string singlePlayerState;
        public string createRoomState;
        public string currentState;
        public string boardsState;
        public string coinsState;
        public string strikersState;


        private void OnEnable()
        {
            currentBoardImage.sprite = boardUIImages[uiOutputData.MyBoardId];
            currentCoinsImage.sprite = coinUIImages[uiOutputData.MyCoinsId];
            currentStrikerImage.sprite = strikerUIImages[uiOutputData.MyStrikerId];

            destination.roomName = "";

        }

        public void BoardBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
          
            uiData.uiManager.ChangeState(boardsState, true);
        }

        public void StrikerBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(strikersState, true);
        }

        public void CoinsBtnClciked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(coinsState, true);
        }

        public void NextBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            destination.singlePlayerGameMode = uiOutputData.singlePlayerGameMode;
            destination.multiPlayerGameMode = uiOutputData.multiPlayerGameMode;
            destination.gameType = uiOutputData.gameType;
            destination.challenge = uiOutputData.challenge;
            destination.difficulty = uiOutputData.aIDifficulty;

            if (destination.gameType == GameType.VsCPU)
            {
                uiData.uiManager.ChangeState(singlePlayerState, true);
                destinationData.ConnectToDestination(destination, null, null);
            }
            else if (destination.gameType == GameType.PlayWithFriends)
            {
                uiData.uiManager.ChangeState(createRoomState, true);
                StartCoroutine(Connect(destination));
            }
        }

        private IEnumerator Connect(Destination d)
        {
            yield return new WaitForSeconds(uiData.disableTime);
            destinationPanelView.ConnectToDestination(destination);
        }

        public void BackBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(currentState, false);

        }


    }
}


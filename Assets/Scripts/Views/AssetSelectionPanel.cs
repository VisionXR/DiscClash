using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        public TMP_Text boardNameText;
        public TMP_Text NoOfHolesText;

        public Image currentStrikerImage;
        public TMP_Text strikerNameText;
        public Slider aimSlider;
        public Slider powerSlider;

        public Image currentCoinsImage;
        public TMP_Text coinColorText;
        public TMP_Text coinShapeText;

        [Header("Assets Data")]
        public List<CoinAssetData> allCoinsData;
        public List<BoardAssetData> allBoardsData;
        public List<StrikerAssetData> allStrikersData;

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
            currentBoardImage.sprite = allBoardsData[uiOutputData.MyBoardId].boardImage;
            boardNameText.text = "Shape : "+ allBoardsData[uiOutputData.MyBoardId].shape;
            NoOfHolesText.text = "Holes : "+ allBoardsData[uiOutputData.MyBoardId].holes;

            currentCoinsImage.sprite = allCoinsData[uiOutputData.MyCoinsId].coinImage;
            coinColorText.text = "Color : "+ allCoinsData[uiOutputData.MyCoinsId].color;
            coinShapeText.text = "Shape : "+ allCoinsData[uiOutputData.MyCoinsId].shape;


            currentStrikerImage.sprite = allStrikersData[uiOutputData.MyStrikerId].strikerImage;
            strikerNameText.text = "Name : "+ allStrikersData[uiOutputData.MyStrikerId].name;
            aimSlider.value = allStrikersData[uiOutputData.MyStrikerId].aim;
            powerSlider.value = allStrikersData[uiOutputData.MyStrikerId].power;


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


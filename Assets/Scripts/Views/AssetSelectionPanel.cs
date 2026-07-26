using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
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
        public DestinationPanelView destinationPanelView;
        public List<GameObject> coinSelectedImages;
        public List<GameObject> boardSelectedImages;
        public List<GameObject> strikerSelectedImages;
        public Destination destination;
        public string singlePlayerState;
        public string createRoomState;



        private void OnEnable()
        {
            ResetBoardImages();
            ResetStrikerImages();
            ResetCoinImages();
            boardSelectedImages[uiOutputData.MyBoardId].SetActive(true);
            strikerSelectedImages[uiOutputData.MyStrikerId].SetActive(true);
            coinSelectedImages[uiOutputData.MyCoinsId].SetActive(true);
        }

        public void BoardBtnClicked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetBoardImages();
            boardSelectedImages[id].SetActive(true);
            uiOutputData.SetMyBoardId(id);
        }

        public void StrikerBtnClicked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetStrikerImages();
            strikerSelectedImages[id].SetActive(true);
            uiOutputData.SetMyStrikerId(id);
        }

        public void CoinsBtnClciked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetCoinImages();
            coinSelectedImages[id].SetActive(true);
            uiOutputData.SetMyCoinsId(id);
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
            

        }

        private void ResetBoardImages()
        {
            foreach (var item in boardSelectedImages)
            {
                item.SetActive(false);
            }

        }

        private void ResetStrikerImages()
        {
            foreach (var item in strikerSelectedImages)
            {
                item.SetActive(false);
            }
        }

        private void ResetCoinImages()
        {
            foreach (var item in coinSelectedImages)
            {
                item.SetActive(false);
            }
        }
    }
}


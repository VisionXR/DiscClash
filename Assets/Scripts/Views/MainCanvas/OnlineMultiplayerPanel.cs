using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{

    public class OnlineMultiplayerPanel : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public MyPlayerSettings myplayerSettings;
        public AppDataSO appData;
        public UIDataSO uiData;

        [Header("Panels")]
        public List<GameObject> GameModeSelectedImages;
        public List<GameObject> RoomSelectedImages;

        public string assetsState;
        public string joinRoomState;
        public string vsFriendsState;

        private void OnEnable()
        {
            ResetGameModeImages();
            ResetRoomImages();

            uiData.uiManager.ChangeState("Home", false);

            if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2 && uiOutputData.challenge == Challenge.BlackAndWhite)
            {
                GameModeSelectedImages[0].SetActive(true);
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2 && uiOutputData.challenge == Challenge.FreeStyle)
            {
                GameModeSelectedImages[1].SetActive(true);
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1AIvsP2AI && uiOutputData.challenge == Challenge.BlackAndWhite)
            {
                GameModeSelectedImages[2].SetActive(true);
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1AIvsP2AI && uiOutputData.challenge == Challenge.FreeStyle)
            {
                GameModeSelectedImages[3].SetActive(true);
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1P2vsAI && uiOutputData.challenge == Challenge.BlackAndWhite)
            {
                GameModeSelectedImages[4].SetActive(true);
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1P2vsAI && uiOutputData.challenge == Challenge.FreeStyle)
            {
                GameModeSelectedImages[5].SetActive(true);
            }

            if (uiOutputData.roomJoinType == RoomJoinType.Create)
            {
                RoomSelectedImages[0].SetActive(true);
            }
            else if (uiOutputData.roomJoinType == RoomJoinType.Join)
            {
                RoomSelectedImages[1].SetActive(true);
            }

        }


        public void P1VsP2_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetMultiPlayerGameMode(MultiPlayerGameMode.P1vsP2);
            uiOutputData.SetChallenge(Challenge.BlackAndWhite);
            GameModeSelectedImages[0].SetActive(true);

        }

        public void P1VsP2_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetMultiPlayerGameMode(MultiPlayerGameMode.P1vsP2);
            uiOutputData.SetChallenge(Challenge.FreeStyle);
            GameModeSelectedImages[1].SetActive(true);

        }

        public void PAIvsAIAI_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetMultiPlayerGameMode(MultiPlayerGameMode.P1AIvsP2AI);
            uiOutputData.SetChallenge(Challenge.BlackAndWhite);
        
            GameModeSelectedImages[2].SetActive(true);


        }

        public void PAIvsAIAI_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetMultiPlayerGameMode(MultiPlayerGameMode.P1AIvsP2AI);
            uiOutputData.SetChallenge(Challenge.FreeStyle);
          
            GameModeSelectedImages[3].SetActive(true);
        }

        public void P1P2vsAI_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetMultiPlayerGameMode(MultiPlayerGameMode.P1P2vsAI);
            uiOutputData.SetChallenge(Challenge.BlackAndWhite);
      
            GameModeSelectedImages[4].SetActive(true);


        }

        public void P1P2vsAI_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetMultiPlayerGameMode(MultiPlayerGameMode.P1P2vsAI);
            uiOutputData.SetChallenge(Challenge.FreeStyle);
       
            GameModeSelectedImages[5].SetActive(true);
        }

        public void CreateRoomBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetRoomImages();
            uiOutputData.SetRoomJoinType(RoomJoinType.Create);
            RoomSelectedImages[0].SetActive(true);
        }

        public void JoinRoomBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetRoomImages();
            uiOutputData.SetRoomJoinType(RoomJoinType.Join);
            RoomSelectedImages[1].SetActive(true);
        }



        public void NextBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            if (uiOutputData.roomJoinType == RoomJoinType.Create)
            {
                uiData.uiManager.ChangeState(assetsState, true);
            }
            else
            {
                uiData.uiManager.ChangeState(joinRoomState, true);
            }
        }

        public void BackButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(vsFriendsState, false);
        }

        private void ResetGameModeImages()
        {
            foreach (GameObject img in GameModeSelectedImages)
            {
                img.SetActive(false);
            }
        }

        private void ResetRoomImages()
        {
            foreach (GameObject img in RoomSelectedImages)
            {
                img.SetActive(false);
            }
        }

    }



}


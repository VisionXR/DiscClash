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

            if(uiOutputData.matchType == MatchType.Tournament)
            {
                if(uiOutputData.challenge == Challenge.BlackAndWhite)
                {
                    uiOutputData.challenge = Challenge.BWTournament;
                }
                if (uiOutputData.challenge == Challenge.FreeStyle)
                {
                    uiOutputData.challenge = Challenge.FSTournament;
                }
            }

            if (uiOutputData.matchType == MatchType.Single)
            {
                if (uiOutputData.challenge == Challenge.BWTournament)
                {
                    uiOutputData.challenge = Challenge.BlackAndWhite;
                }
                if (uiOutputData.challenge == Challenge.FSTournament)
                {
                    uiOutputData.challenge = Challenge.FreeStyle;
                }
            }



            uiData.uiManager.ChangeState("Home", false);        

            if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)          
            {
                if (uiOutputData.challenge == Challenge.BlackAndWhite || uiOutputData.challenge == Challenge.BWTournament)
                {
                    GameModeSelectedImages[0].SetActive(true);
                }
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
            {
                if (uiOutputData.challenge == Challenge.FreeStyle || uiOutputData.challenge == Challenge.FSTournament)
                {
                    GameModeSelectedImages[1].SetActive(true);
                }
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1AIvsP2AI )
            {
                if (uiOutputData.challenge == Challenge.BlackAndWhite || uiOutputData.challenge == Challenge.BWTournament)
                {
                    GameModeSelectedImages[2].SetActive(true);
                }
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1AIvsP2AI )
            {
                if (uiOutputData.challenge == Challenge.FreeStyle || uiOutputData.challenge == Challenge.FSTournament)
                {
                    GameModeSelectedImages[3].SetActive(true);
                }
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1P2vsAI )
            {
                if (uiOutputData.challenge == Challenge.BlackAndWhite || uiOutputData.challenge == Challenge.BWTournament)
                {
                    GameModeSelectedImages[4].SetActive(true);
                }
            }
            else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1P2vsAI )
            {
                if (uiOutputData.challenge == Challenge.FreeStyle || uiOutputData.challenge == Challenge.FSTournament)
                {
                    GameModeSelectedImages[5].SetActive(true);
                }
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

            if (uiOutputData.matchType == MatchType.Single)
            {
                uiOutputData.SetChallenge(Challenge.BlackAndWhite);
            }
            else
            {
                uiOutputData.SetChallenge(Challenge.BWTournament);
            }
            GameModeSelectedImages[0].SetActive(true);

        }

        public void P1VsP2_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetMultiPlayerGameMode(MultiPlayerGameMode.P1vsP2);

            if (uiOutputData.matchType == MatchType.Single)
            {
                uiOutputData.SetChallenge(Challenge.FreeStyle);
            }
            else
            {
                uiOutputData.SetChallenge(Challenge.FSTournament);
            }
            GameModeSelectedImages[1].SetActive(true);

        }

        public void PAIvsAIAI_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetMultiPlayerGameMode(MultiPlayerGameMode.P1AIvsP2AI);
            if (uiOutputData.matchType == MatchType.Single)
            {
                uiOutputData.SetChallenge(Challenge.BlackAndWhite);
            }
            else
            {
                uiOutputData.SetChallenge(Challenge.BWTournament);
            }

            GameModeSelectedImages[2].SetActive(true);


        }

        public void PAIvsAIAI_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetMultiPlayerGameMode(MultiPlayerGameMode.P1AIvsP2AI);

            if (uiOutputData.matchType == MatchType.Single)
            {
                uiOutputData.SetChallenge(Challenge.FreeStyle);
            }
            else
            {
                uiOutputData.SetChallenge(Challenge.FSTournament);
            }

            GameModeSelectedImages[3].SetActive(true);
        }

        public void P1P2vsAI_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetMultiPlayerGameMode(MultiPlayerGameMode.P1P2vsAI);
            if (uiOutputData.matchType == MatchType.Single)
            {
                uiOutputData.SetChallenge(Challenge.BlackAndWhite);
            }
            else
            {
                uiOutputData.SetChallenge(Challenge.BWTournament);
            }

            GameModeSelectedImages[4].SetActive(true);


        }

        public void P1P2vsAI_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetMultiPlayerGameMode(MultiPlayerGameMode.P1P2vsAI);

            if (uiOutputData.matchType == MatchType.Single)
            {
                uiOutputData.SetChallenge(Challenge.FreeStyle);
            }
            else
            {
                uiOutputData.SetChallenge(Challenge.FSTournament);
            }

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


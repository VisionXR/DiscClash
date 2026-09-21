using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{

    public class vsCpuPanel : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public UIOutputDataSO uiOutputData;
        public UIInputDataSO uiInputData;
        public MyPlayerSettings myplayerSettings;
        public AppDataSO appData;
        public UIDataSO uiData;


        [Header("Panels")]
        public List<GameObject> GameModeSelectedImages;
        public List<GameObject> DifficultySelectedImages;

        public string vsCpuState;
        public string assetsState;

        private void OnEnable()
        {
            uiData.uiManager.ChangeState("Home", false);
     

            ResetGameModeImages();
            ResetDifficultyImages();

            if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
            {
                if (uiOutputData.challenge == Challenge.BlackAndWhite || uiOutputData.challenge == Challenge.BWTournament)
                {
                    GameModeSelectedImages[0].SetActive(true);
                }
            }
            else if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI)
            {
                if (uiOutputData.challenge == Challenge.FreeStyle || uiOutputData.challenge == Challenge.FSTournament)
                {
                    GameModeSelectedImages[1].SetActive(true);
                }
            }
            else if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PAIvsAI)
            {
                if (uiOutputData.challenge == Challenge.BlackAndWhite || uiOutputData.challenge == Challenge.BWTournament)
                {
                    GameModeSelectedImages[2].SetActive(true);
                }
            }
            else if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PAIvsAI )
            {
                if (uiOutputData.challenge == Challenge.FreeStyle || uiOutputData.challenge == Challenge.FSTournament)
                {
                    GameModeSelectedImages[3].SetActive(true);
                }
            }


            if (uiOutputData.aIDifficulty == AIDifficulty.Easy)
            {
                DifficultySelectedImages[0].SetActive(true);
            }
            else if (uiOutputData.aIDifficulty == AIDifficulty.Medium)
            {
                DifficultySelectedImages[1].SetActive(true);
            }
            else if (uiOutputData.aIDifficulty == AIDifficulty.Hard)
            {
                DifficultySelectedImages[2].SetActive(true);
            }
        }


        public void PvsAI_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetSinglePlayerGameMode(SinglePlayerGameMode.PvsAI);
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

        public void PvsAI_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetSinglePlayerGameMode(SinglePlayerGameMode.PvsAI);
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
            uiOutputData.SetSinglePlayerGameMode(SinglePlayerGameMode.PAIvsAI);
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
            uiOutputData.SetSinglePlayerGameMode(SinglePlayerGameMode.PAIvsAI);
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

        public void EasyBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetDifficultyImages();
            uiOutputData.SetAIDifficulty(AIDifficulty.Easy);
            DifficultySelectedImages[0].SetActive(true);

        }

        public void MediumBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetDifficultyImages();
            uiOutputData.SetAIDifficulty(AIDifficulty.Medium);
            DifficultySelectedImages[1].SetActive(true);
        }

        public void HardBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetDifficultyImages();
            uiOutputData.SetAIDifficulty(AIDifficulty.Hard);
            DifficultySelectedImages[2].SetActive(true);
        }

        public void NextBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(assetsState, true);
        }

        public void BackButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(vsCpuState, false);
        }

        private void ResetGameModeImages()
        {
            foreach(GameObject img in GameModeSelectedImages)
            {
                img.SetActive(false);
            }
        }

        private void ResetDifficultyImages()
        {
            foreach (GameObject img in DifficultySelectedImages)
            {
                img.SetActive(false);
            }
        }

    }
}

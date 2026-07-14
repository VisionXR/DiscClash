using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using NUnit.Framework;
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

            if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI && uiOutputData.challenge == Challenge.BlackAndWhite)
            {
                GameModeSelectedImages[0].SetActive(true);
            }
            else if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PvsAI && uiOutputData.challenge == Challenge.FreeStyle)
            {
                GameModeSelectedImages[1].SetActive(true);
            }
            else if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PAIvsAI && uiOutputData.challenge == Challenge.BlackAndWhite)
            {
                GameModeSelectedImages[2].SetActive(true);
            }
            else if (uiOutputData.singlePlayerGameMode == SinglePlayerGameMode.PAIvsAI && uiOutputData.challenge == Challenge.FreeStyle)
            {
                GameModeSelectedImages[3].SetActive(true);
            }

            if(uiOutputData.aIDifficulty == AIDifficulty.Easy)
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
            uiOutputData.SetChallenge(Challenge.BlackAndWhite);
            GameModeSelectedImages[0].SetActive(true);

        }

        public void PvsAI_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetSinglePlayerGameMode(SinglePlayerGameMode.PvsAI);
            uiOutputData.SetChallenge(Challenge.FreeStyle);
            GameModeSelectedImages[1].SetActive(true);

        }

        public void PAIvsAIAI_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetSinglePlayerGameMode(SinglePlayerGameMode.PAIvsAI);
            uiOutputData.SetChallenge(Challenge.BlackAndWhite);
            GameModeSelectedImages[2].SetActive(true);


        }

        public void PAIvsAIAI_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetGameModeImages();
            uiOutputData.SetSinglePlayerGameMode(SinglePlayerGameMode.PAIvsAI);
            uiOutputData.SetChallenge(Challenge.FreeStyle);
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

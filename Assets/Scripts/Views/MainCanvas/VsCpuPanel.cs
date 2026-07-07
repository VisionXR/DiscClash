using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
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
        public string vsCpuState;
        public string assetsState;


        public void PvsAI_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            uiOutputData.SetGameMode(GameMode.PvsAI);
            uiOutputData.SetChallenge(Challenge.BlackAndWhite);
            uiOutputData.SetAIDifficulty(AIDifficulty.Hard);
           

        }

        public void PvsAI_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.SetGameMode(GameMode.PvsAI);
            uiOutputData.SetChallenge(Challenge.FreeStyle);
            uiOutputData.SetAIDifficulty(AIDifficulty.Hard);
            

        }

        public void PAIvsAIAI_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
    
            uiOutputData.SetGameMode(GameMode.PAIvsAI);
            uiOutputData.SetChallenge(Challenge.BlackAndWhite);
            uiOutputData.SetAIDifficulty(AIDifficulty.Hard);
      

        }

        public void PAIvsAIAI_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            uiOutputData.SetGameMode(GameMode.PAIvsAI);
            uiOutputData.SetChallenge(Challenge.FreeStyle);
    
            

        }

        public void EasyBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.SetAIDifficulty(AIDifficulty.Easy);
        }

        public void MediumBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.SetAIDifficulty(AIDifficulty.Medium);
        }

        public void HardBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiOutputData.SetAIDifficulty(AIDifficulty.Hard);
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



    }
}

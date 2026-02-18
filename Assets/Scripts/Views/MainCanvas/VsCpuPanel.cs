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
       

        [Header("Panels")]
        public GameObject HomePanel;
        public GameObject AssetSelectionPanel;


        public void PvsAI_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            AssetSelectionPanel.SetActive(true);
            uiOutputData.SetEntryFee(50);
            uiOutputData.SetGameMode(GameMode.PvsAI);
            uiOutputData.SetChallenge(Challenge.BlackAndWhite);
            uiOutputData.SetAIDifficulty(AIDifficulty.Hard);
            gameObject.SetActive(false);

        }

        public void PvsAI_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            AssetSelectionPanel.SetActive(true);
            uiOutputData.SetEntryFee(50);
            uiOutputData.SetGameMode(GameMode.PvsAI);
            uiOutputData.SetChallenge(Challenge.FreeStyle);
            uiOutputData.SetAIDifficulty(AIDifficulty.Hard);
            gameObject.SetActive(false);

        }

        public void PAIvsAIAI_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            AssetSelectionPanel.SetActive(true);
            uiOutputData.SetEntryFee(75);
            uiOutputData.SetGameMode(GameMode.PAIvsAI);
            uiOutputData.SetChallenge(Challenge.BlackAndWhite);
            uiOutputData.SetAIDifficulty(AIDifficulty.Hard);
            gameObject.SetActive(false);

        }

        public void PAIvsAIAI_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            AssetSelectionPanel.SetActive(true);
            uiOutputData.SetEntryFee(75);
            uiOutputData.SetGameMode(GameMode.PAIvsAI);
            uiOutputData.SetChallenge(Challenge.FreeStyle);
            uiOutputData.SetAIDifficulty(AIDifficulty.Hard);
            gameObject.SetActive(false);

        }

        public void BackButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            HomePanel.SetActive(true);
           
            gameObject.SetActive(false);
        }



    }
}

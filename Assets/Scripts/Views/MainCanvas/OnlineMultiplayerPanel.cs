using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
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

        public string vsFriendsState;

        public void P1vsP2_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
          
            uiOutputData.SetGameMode(GameMode.P1vsP2);
            uiOutputData.SetChallenge(Challenge.BlackAndWhite);


        }

        public void P1vsP2_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
           
            uiOutputData.SetGameMode(GameMode.P1vsP2);
            uiOutputData.SetChallenge(Challenge.FreeStyle);


        }

        public void P1P2vsAI_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            uiOutputData.SetGameMode(GameMode.P1P2vsAI);
            uiOutputData.SetChallenge(Challenge.BlackAndWhite);


        }

        public void P1P2vsAI_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            uiOutputData.SetGameMode(GameMode.P1P2vsAI);
            uiOutputData.SetChallenge(Challenge.FreeStyle);
     

        }

        public void P1AIvsP2AI_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();

            uiOutputData.SetGameMode(GameMode.P1AIvsP2AI);
            uiOutputData.SetChallenge(Challenge.BlackAndWhite);
    

        }

        public void P1AIvsP2AI_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
           
            uiOutputData.SetGameMode(GameMode.P1AIvsP2AI);
            uiOutputData.SetChallenge(Challenge.FreeStyle);
                  

        }

        public void P1P2vsP3P4_BW_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
           
            uiOutputData.SetGameMode(GameMode.P1P2vsP3P4);
            uiOutputData.SetChallenge(Challenge.BlackAndWhite);


        }

        public void P1P2vsP3P4_FS_BtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
           
            uiOutputData.SetGameMode(GameMode.P1P2vsP3P4);
            uiOutputData.SetChallenge(Challenge.FreeStyle);


        }


        public void BackButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(vsFriendsState, false);
        }



    }
}

using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;
using UnityEngine.UI;

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
        public GameObject MainPanel;


        public void StartSinglePlayerClicked()
        {          
            AudioManager.instance.PlayButtonClickSound();    
  
        }

        public void BackButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            MainPanel.SetActive(true);
           
            gameObject.SetActive(false);
        }



    }
}

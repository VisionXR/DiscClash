using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class Player1SettingsPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public CamPositionSO camData;
        public PlayersDataSO playerData;


        [Header("Panel Objects")]
        public GameObject CenterCanvas;
        public GameObject MainCanvas;
        public GameObject ExitPanel;


        public void OnExitButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            CenterCanvas.SetActive(true);
            ExitPanel.SetActive(true);
        }

        public void RecenterButtonClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            Player mainPlayer = playerData.GetMainPlayer();
            if(mainPlayer != null)
            {
                camData.Recenter(mainPlayer.myId);
            }
            
        }

    }
}

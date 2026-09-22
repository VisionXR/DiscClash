using com.VisionXR.Controllers;
using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class PlayerNamePanelView : MonoBehaviour
    {
        public MyPlayerSettings playerSettings;
        public TMP_InputField nameIF;
        public PanelOnOff myPanel;
        public AuthManager authManager;

        private void OnEnable()
        {
            nameIF.text = playerSettings.MyName;
        }

        public void OkBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            authManager.SetPlayFabDisplayName(nameIF.text);
            myPanel.TurnOffPanel();
        }

        public void CloseBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            myPanel.TurnOffPanel();
        }
    }
}

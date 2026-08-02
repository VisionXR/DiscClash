using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class DeleteAccountPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
     
        public MyPlayerSettings userData;

        [Header("Loca Objects")]
        public PanelOnOff deleteAccountPanel;
        public void YesBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            userData.DeleteAccount();
            deleteAccountPanel.TurnOffPanel();
        }

        public void NoBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            deleteAccountPanel.TurnOffPanel();
        }
    }
}

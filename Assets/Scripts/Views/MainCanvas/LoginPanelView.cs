using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{

    public class LoginPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public CloudDataSO cloudData;
        public MyPlayerSettings playerSettings;
        public UIDataSO uIData;

        public void LoginWithGoogleBtnClciked()
        {
            AudioManager.instance.PlayButtonClickSound();
            playerSettings.SetLoginType(LoginType.Google);

            if (Application.isEditor)
            {
                cloudData.EditorLogin();
            }
            else
            {
                cloudData.LoginToGoogle();
            }

        }

        public void GuestLoginBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            playerSettings.SetLoginType(LoginType.Guest);
            cloudData.GuestLogin();

        }
    }
}

using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class LoginPanelView : MonoBehaviour
{
    [Header("Scriptable Objects")]

    public CloudDataSO cloudData;
    public MyPlayerSettings playerSettings;

    public void LoginWithGoogleBtnClciked()
    {
        AudioManager.instance.PlayButtonClickSound();
        playerSettings.SetLoginType(LoginType.Google);
        cloudData.LoginToGoogle();
        gameObject.SetActive(false);
    }

    public void GuestLoginBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        playerSettings.SetLoginType(LoginType.Guest);
        cloudData.GuestLogin();
        gameObject.SetActive(false);
    }
}

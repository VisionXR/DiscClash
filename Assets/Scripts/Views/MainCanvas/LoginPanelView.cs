using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class LoginPanelView : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIInputDataSO uIInputData;
    public CloudDataSO cloudData;
    public MyPlayerSettings playerSettings;

    public void LoginWithGoogleBtnClciked()
    {
        AudioManager.instance.PlayButtonClickSound();
        playerSettings.SetLoginType(LoginType.Google);
        uIInputData.ShowLoadingPanel();

        
        if (Application.isEditor)
        {
            cloudData.EditorLogin();
        }
        else
        {
            cloudData.LoginToGoogle();
        }
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

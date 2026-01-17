using UnityEngine;

public class AboutUsPanel : MonoBehaviour
{

    [Header(" Game Objects")]
    public GameObject MainPanel;
    public GameObject HomePanel;

    [Header(" Url ")]
    public string carromURL;
    public string realityRulerURL;
    public string handPuzzlesURL;
    public string metaGroupURL;

    public void BackButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        MainPanel.SetActive(true);
        HomePanel.SetActive(true);
        gameObject.SetActive(false);
    }


    public void CarromButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        OpenOculusStorePDPAndroid(carromURL);
    }

    public void JoinGroupBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        Application.OpenURL(metaGroupURL);
    }

    public void RealityRulerButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        OpenOculusStorePDPAndroid(realityRulerURL);
    }

    public void HandPuzzlesButtonClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        OpenOculusStorePDPAndroid(handPuzzlesURL);
    }

    private static void OpenOculusStorePDPAndroid(string targetAppID)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject i = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", "com.oculus.vrshell");
        i.Call<AndroidJavaObject>("setClassName", "com.oculus.vrshell", "com.oculus.vrshell.MainActivity");
        i.Call<AndroidJavaObject>("setAction", "android.intent.action.VIEW");
        i.Call<AndroidJavaObject>("putExtra", "uri", "/item/" + targetAppID);
        i.Call<AndroidJavaObject>("putExtra", "intent_data", "systemux://store");
        currentActivity.Call("startActivity", i);
    }
}

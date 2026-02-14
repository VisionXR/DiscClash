using com.VisionXR.ModelClasses;
using UnityEngine;

public class InternetDisconnectedPanel : MonoBehaviour
{
    public UIInputDataSO uiInputData;


    public void RetryBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
    }

    public void PlayAgainBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
    }
}

using com.VisionXR.ModelClasses;
using UnityEngine;

public class RoomJoinFailedPanel : MonoBehaviour
{
    public UIInputDataSO uiInputData;
    public void OkBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        uiInputData.GoToHome();
        gameObject.SetActive(false);
    }
}

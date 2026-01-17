using com.VisionXR.ModelClasses;
using UnityEngine;

public class RoomJoinFailedPanel : MonoBehaviour
{
    public UIOutputDataSO uiOutputData;
    public void OkBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        uiOutputData.GoToHome();
        gameObject.SetActive(false);
    }
}

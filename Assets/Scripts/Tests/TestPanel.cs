using UnityEngine;

public class TestPanel : MonoBehaviour
{
    [Header("Cams")]
    public Camera leftCamera;
    public Camera rightCamera;



    public void StartAntialiasing()
    {
        AudioManager.instance.PlayButtonClickSound();
        leftCamera.allowMSAA = true;
        rightCamera.allowMSAA = true;

        
    }


    public void StartPostProcess()
    {
        AudioManager.instance.PlayButtonClickSound();
 

    }

    public void StopAntialiasing()
    {
        AudioManager.instance.PlayButtonClickSound();
        leftCamera.allowMSAA = false;
        rightCamera.allowMSAA = false;
    }

    public void StopPostProcess()
    {
        AudioManager.instance.PlayButtonClickSound();


    }

}

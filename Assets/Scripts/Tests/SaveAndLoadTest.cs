using com.VisionXR.ModelClasses;
using UnityEngine;

public class SaveAndLoadTest : MonoBehaviour
{
    public MyPlayerSettings playerSettings;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            playerSettings.SaveSettings();
            Debug.Log(" Saved ");
        }
    }
}

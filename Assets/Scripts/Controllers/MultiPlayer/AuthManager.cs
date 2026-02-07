using com.VisionXR.ModelClasses;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
   public MyPlayerSettings playerSettings;


    private void Start()
    {

        playerSettings.SetUserNameAndId("Player_" + Random.Range(1000, 9999), (ulong)Random.Range(1000, 9999));
    }
}

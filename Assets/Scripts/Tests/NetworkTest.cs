using com.VisionXR.ModelClasses;
using UnityEngine;

public class NetworkTest : MonoBehaviour
{
     
     public string roomName;
     public string message;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            //networkData.CreateRoomEvent?.Invoke(roomName);
            //enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            //networkData.JoinRoomEvent?.Invoke(roomName);
            //enabled = false;
        }

    }
}

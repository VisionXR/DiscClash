using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class NotificationTesting : MonoBehaviour
{
    public MultiPlayerGameMode gameMode;
    public NotificationDataSO notificationDataSO;


    // Update is called once per frame
    void Update()
    {
         if(Input.GetKeyDown(KeyCode.N))
        {
            NotificationMessage notificationMessage = new NotificationMessage();
            notificationMessage.playerName = "Vikram";
            notificationMessage.roomName = "MyDreamRoom";
            notificationMessage.multiPlayerGameMode = gameMode;
            notificationDataSO.NotificationReceived(notificationMessage);
        }
    }
}

using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;
using Application = UnityEngine.Application;

public class DestinationManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIOutputDataSO uIOutputData;



    public  void SetDestination(Destination d)
    {

        if(Application.isEditor)
        {
            return;
        }

        string destinationApiName = "";

        if(d.gameType == GameType.SinglePlayer)
        {
            destinationApiName = Enum.GetName(typeof(SinglePlayerGameMode), d.singlePlayerGameMode) + "_"+Enum.GetName(typeof(Game), d.game);
        }
        else if (d.gameType == GameType.MultiPlayer)
        {
            destinationApiName = Enum.GetName(typeof(MultiPlayerGameMode), d.multiPlayerGameMode) + "_" + Enum.GetName(typeof(Game), d.game);
        }
        else if (d.gameType == GameType.Tutorial)
        {
            destinationApiName = "Tutorial";
        }
        else if (d.gameType == GameType.TrickShots)
        {
            destinationApiName = "TrickShots";
        }

      
    }

    public void EncodeRoom(ServerRegion region, string roomName)
    {

    }

}

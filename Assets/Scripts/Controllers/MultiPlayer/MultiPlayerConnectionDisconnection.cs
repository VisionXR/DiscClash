using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiPlayerConnectionDisconnection : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public PlayersDataSO playersData;

    [Header(" States ")]
    public DataManager dataManager;
    public List<bool> isPlayerReady;
    public bool isPlayerInGame;

    [Header(" UI Panels ")]
    public WaitingPanel waitingPanel;



    private void OnEnable()
    {
        
        playersData.PlayerJoinedEvent += PlayerJoined;
        playersData.PlayerLeftEvent += PlayerLeft;

    }

    private void OnDisable()
    {
        playersData.PlayerJoinedEvent -= PlayerJoined;
        playersData.PlayerLeftEvent -= PlayerLeft;
        
    }


    private void PlayerJoined(Player player)
    {
        waitingPanel.SetName(player.myId, player.name);
        waitingPanel.SetStatus(player.myId, "Joined");
        waitingPanel.SetImage(player.myId, player.GetMyImage());
    }

    private void PlayerLeft(Player player)
    {
      
    }

 
    public void EndGame()
    {
       
    }

    public void LaunchInvitePanel()
    {
        
        AudioManager.instance.PlayButtonClickSound();
      
    }

}

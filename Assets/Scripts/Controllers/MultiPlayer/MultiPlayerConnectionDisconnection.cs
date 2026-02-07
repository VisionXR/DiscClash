using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiPlayerConnectionDisconnection : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public PlayersDataSO playersData;
    public UIInputDataSO uiInputData;
    public InputDataSO inputData;

    [Header(" States ")]
    public DataManager dataManager;
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
        isPlayerInGame = false;
        playersData.PlayerJoinedEvent -= PlayerJoined;
        playersData.PlayerLeftEvent -= PlayerLeft;
        
    }

    private void PlayerJoined(Player player)
    {
        waitingPanel.SetName(player.myId, player.myName);
        waitingPanel.SetStatus(player.myId, "Joined");
        waitingPanel.SetImage(player.myId, player.GetMyImage());
    }

    private void PlayerLeft(Player player)
    {
        if (isPlayerInGame)
        {
            uiInputData.OtherPlayerLeft();
            inputData.DeactivateInput();
            isPlayerInGame = false;
        }
        else
        {

        }
    }

 
    public void EndGame()
    {
       isPlayerInGame = false;
    }

    public void StartGame()
    {
        isPlayerInGame = true;
        StartCoroutine(ShowPlayers());
    }

    private IEnumerator ShowPlayers()
    {
        yield return new WaitForSeconds(1);
        foreach (Player p in playersData.CurrentPlayers)
        {
            uiInputData.ShowPlayerDetails(p);
        }
    }

    public void LaunchInvitePanel()
    {
        
        AudioManager.instance.PlayButtonClickSound();
      
    }

}

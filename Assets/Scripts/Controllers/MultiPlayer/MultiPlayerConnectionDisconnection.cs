using com.VisionXR.Controllers;
using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
using System.Collections;
using UnityEngine;

public class MultiPlayerConnectionDisconnection : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public PlayersDataSO playersData;
    public UIInputDataSO uiInputData;
    public InputDataSO inputData;
    public UIOutputDataSO uiOutputData;

    [Header(" States ")]
    public DataManager dataManager;
    public bool isPlayerInGame;

    [Header(" UI Panels ")]
    public WaitingPanel waitingPanel2Player;
    public WaitingPanel waitingPanel4Player;



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
        if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
        {
            waitingPanel2Player.SetName(player.myId, player.myName);
            waitingPanel2Player.SetStatus(player.myId, "Joined");
            waitingPanel2Player.SetImage(player.myId, player.GetMyImage());
        }
        else
        {
            waitingPanel4Player.SetName(player.myId, player.myName);
            waitingPanel4Player.SetStatus(player.myId, "Joined");
            waitingPanel4Player.SetImage(player.myId, player.GetMyImage());
        }
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

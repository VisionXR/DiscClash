using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerConnectionDisconnection : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public PlayersDataSO playersData;
    public UIOutputDataSO uiOutputData;
    public UIInputDataSO uiInputData;
    public NetworkInputSO networkInputData;
    public NetworkOutputSO networkOutputData;
    public OculusDataSO oculusData;

    [Header(" States ")]
    public DataManager dataManager;
    public List<bool> isPlayerReady;
    public bool isPlayerInGame;



    private void OnEnable()
    {
        isPlayerInGame= false;

        playersData.PlayerJoinedEvent += PlayerJoined;
        playersData.PlayerLeftEvent += PlayerLeft;
        networkInputData.PlayerReadyEvent += OnPlayerReadyReceived;
        uiOutputData.PlayAgainEvent += PlayAgain;


        SetInitialStatus("Connecting");
        StartCoroutine(ShowPlayers());
    }

    private void OnDisable()
    {
        playersData.PlayerJoinedEvent -= PlayerJoined;
        playersData.PlayerLeftEvent -= PlayerLeft;

        networkInputData.PlayerReadyEvent -= OnPlayerReadyReceived;
        uiOutputData.PlayAgainEvent -= PlayAgain;
    }

    private void PlayAgain()
    {
       StartCoroutine(WaitAndReplay());
    }

    private IEnumerator WaitAndReplay()
    {
        yield return new WaitForSeconds(1);
        if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2 && playersData.CurrentPlayers.Count == 2)
        {

            Player mainP = playersData.GetMainPlayer();
            uiInputData.SetButtonEvent?.Invoke(mainP.myId);
            uiInputData.SetPlayerStatusEvent?.Invoke(mainP.myId, "Start Game");


        }
        else if (uiOutputData.multiPlayerGameMode != MultiPlayerGameMode.P1vsP2 && playersData.CurrentPlayers.Count == 4)
        {

            Player mainP = playersData.GetMainPlayer();
            uiInputData.SetButtonEvent?.Invoke(mainP.myId);
            uiInputData.SetPlayerStatusEvent?.Invoke(mainP.myId, "Start Game");
           
            foreach (Player p in playersData.CurrentPlayers)
            {
                if (p.myPlayerRole == PlayerRole.AI)
                {
                    uiInputData.SetPlayerStatusEvent?.Invoke(p.myId, "Ready");
                    isPlayerReady[p.myId - 1] = true;
                }
            }


        }
        else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1P2vsP3P4 && playersData.CurrentPlayers.Count == 4)
        {
            SetInitialStatus("Not Ready");
            Player mainP = playersData.GetMainPlayer();
            uiInputData.SetButtonEvent?.Invoke(mainP.myId);
            uiInputData.SetPlayerStatusEvent?.Invoke(mainP.myId, "Start Game");
            foreach (Player p in playersData.CurrentPlayers)
            {
                if (p.myPlayerRole == PlayerRole.AI)
                {
                    uiInputData.SetPlayerStatusEvent?.Invoke(p.myId, "Ready");
                    isPlayerReady[p.myId - 1] = true;
                }
            }


        }
    }

    public void SetInitialStatus(string status)
    {
       
         uiInputData.SetPlayerStatusEvent?.Invoke(1, status);
         uiInputData.SetPlayerStatusEvent?.Invoke(2, status);
         uiInputData.SetPlayerStatusEvent?.Invoke(3, status);
         uiInputData.SetPlayerStatusEvent?.Invoke(4, status);
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public IEnumerator ShowPlayers()
    {
        yield return new WaitForSeconds(2);
        foreach (Player p in playersData.CurrentPlayers)
        {
            uiInputData.ShowPlayerDetails(p);
            uiInputData.SetPlayerStatusEvent?.Invoke(p.myId, "Joined");
        }



        if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2 && playersData.CurrentPlayers.Count == 2)
        {
            SetInitialStatus("Not Ready");
            Player mainP = playersData.GetMainPlayer();
            uiInputData.SetButtonEvent?.Invoke(mainP.myId);
            uiInputData.SetPlayerStatusEvent?.Invoke(mainP.myId, "Start Game");
            networkOutputData._runner.SessionInfo.IsVisible = false;
            isPlayerReady = new List<bool> { false, false };

        }
        else if (uiOutputData.multiPlayerGameMode != MultiPlayerGameMode.P1vsP2 && playersData.CurrentPlayers.Count == 4)
        {
            SetInitialStatus("Not Ready");
            Player mainP = playersData.GetMainPlayer();
            uiInputData.SetButtonEvent?.Invoke(mainP.myId);
            uiInputData.SetPlayerStatusEvent?.Invoke(mainP.myId, "Start Game");
            networkOutputData._runner.SessionInfo.IsVisible = false;
            isPlayerReady = new List<bool> { false, false, false, false };
            foreach (Player p in playersData.CurrentPlayers)
            {
                if (p.myPlayerRole == PlayerRole.AI)
                {
                    uiInputData.SetPlayerStatusEvent?.Invoke(p.myId, "Ready");
                    isPlayerReady[p.myId - 1] = true;
                }
            }
        }
        else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1P2vsP3P4 && playersData.CurrentPlayers.Count == 4)
        {
            SetInitialStatus("Not Ready");
            Player mainP = playersData.GetMainPlayer();
            uiInputData.SetButtonEvent?.Invoke(mainP.myId);
            uiInputData.SetPlayerStatusEvent?.Invoke(mainP.myId, "Start Game");
            networkOutputData._runner.SessionInfo.IsVisible = false;
            isPlayerReady = new List<bool> { false, false, false, false };
        }
    }

    private void PlayerJoined(Player player)
    {
        StartCoroutine(ShowPlayers());
    }

    private void PlayerLeft(Player player)
    {
        if(isPlayerInGame)
        {
            uiInputData.OtherPlayerLeft();
        }
    }

    private void OnPlayerReadyReceived(int id)
    {
        isPlayerReady[id-1] = true;
        uiInputData.SetPlayerStatusEvent?.Invoke(id, "Ready");

        foreach(bool b in isPlayerReady)
        {
            if (!b)
            {
                return;
            }
        }

        if (networkOutputData.IsHost())
        {         
            dataManager.StartGame(1);         
            isPlayerInGame= true;
            SetInitialStatus("In Game");

        }
        else
        {        

            isPlayerInGame = true;
            SetInitialStatus("In Game");

        }
    }

    public void EndGame()
    {
        isPlayerInGame = false;

        if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2 && playersData.CurrentPlayers.Count == 2)
        {

            isPlayerReady = new List<bool> { false, false };

        }
        else if (uiOutputData.multiPlayerGameMode != MultiPlayerGameMode.P1vsP2 && playersData.CurrentPlayers.Count == 4)
        {

            isPlayerReady = new List<bool> { false, false, false, false };

        }
        else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1P2vsP3P4 && playersData.CurrentPlayers.Count == 4)
        {

            isPlayerReady = new List<bool> { false, false, false, false };
        }

        foreach (Player p in playersData.CurrentPlayers)
        {
            uiInputData.SetPlayerStatusEvent?.Invoke(p.myId, "Game Over");
        }

    }

    public void LaunchInvitePanel()
    {
        
        AudioManager.instance.PlayButtonClickSound();
      
    }

}

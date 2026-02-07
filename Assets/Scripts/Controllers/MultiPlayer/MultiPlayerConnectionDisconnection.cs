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
        

        playersData.PlayerJoinedEvent += PlayerJoined;
        playersData.PlayerLeftEvent += PlayerLeft;
        networkInputData.PlayerReadyEvent += OnPlayerReadyReceived;
        uiOutputData.PlayAgainEvent += PlayAgain;

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

           
  


        }
        else if (uiOutputData.multiPlayerGameMode != MultiPlayerGameMode.P1vsP2 && playersData.CurrentPlayers.Count == 4)
        {

           


        }
        else if (uiOutputData.multiPlayerGameMode == MultiPlayerGameMode.P1P2vsP3P4 && playersData.CurrentPlayers.Count == 4)
        {


        }
    }




    private void PlayerJoined(Player player)
    {
      
    }

    private void PlayerLeft(Player player)
    {
      
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
         

        }
        else
        {        

            isPlayerInGame = true;
           

        }
    }

    public void EndGame()
    {
       
    }

    public void LaunchInvitePanel()
    {
        
        AudioManager.instance.PlayButtonClickSound();
      
    }

}

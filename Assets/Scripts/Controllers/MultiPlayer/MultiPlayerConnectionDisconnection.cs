using com.VisionXR.Controllers;
using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;

public class MultiPlayerConnectionDisconnection : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public PlayersDataSO playersData;
    public UIInputDataSO uiInputData;
    public InputDataSO inputData;
    public UIOutputDataSO uiOutputData;
    public NetworkOutputSO networkOutputData;
    public UIDataSO uiData;

    [Header(" States ")]
    public DataManager dataManager;
    public bool isPlayerInGame;

    [Header("Next And Previous Panels")]
    public string opponentPlayerLeftState;
    public string mainPlayerLeftState;




    private void OnEnable()
    {
        
        playersData.PlayerJoinedEvent += PlayerJoined;
        playersData.PlayerLeftEvent += PlayerLeft;

        networkOutputData.SetHostReady(true);
        networkOutputData.SetClientReady(true);

    }

    private void OnDisable()
    {
        isPlayerInGame = false;
        playersData.PlayerJoinedEvent -= PlayerJoined;
        playersData.PlayerLeftEvent -= PlayerLeft;
        
    }

    private void PlayerJoined(Player player)
    {
       
    }

    private void PlayerLeft(Player player)
    {
        if (isPlayerInGame)
        {
            uiData.uiManager.ChangeState(opponentPlayerLeftState, true);
            Debug.Log(" Opponenet Player left after game starts");
            inputData.DisableInput();
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

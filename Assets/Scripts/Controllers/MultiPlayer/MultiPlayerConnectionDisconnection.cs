using com.VisionXR.Controllers;
using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
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
    // T06: A tournament remains active while its board-result panel is open.
    private bool sessionActive;
    // T06/G01: Cancel pending game/result coroutines when the session is interrupted.
    public System.Action SessionLostEvent;

    [Header("Next And Previous Panels")]
    public string opponentPlayerLeftState;
    public string mainPlayerLeftState;




    private void OnEnable()
    {
        
        playersData.PlayerJoinedEvent += PlayerJoined;
        playersData.PlayerLeftEvent += PlayerLeft;
        // G01: Local transport failures need a UI route as well as remote PlayerLeft events.
        networkOutputData.ConnectionLostEvent += ConnectionLost;

        networkOutputData.SetHostReady(true);
        networkOutputData.SetClientReady(true);

    }

    private void OnDisable()
    {
        isPlayerInGame = false;
        // T06/G01: Ignore callbacks after this session has been left intentionally.
        sessionActive = false;
        networkOutputData.ConnectionLostEvent -= ConnectionLost;
        playersData.PlayerJoinedEvent -= PlayerJoined;
        playersData.PlayerLeftEvent -= PlayerLeft;
        
    }

    private void PlayerJoined(Player player)
    {
       
    }

    private void PlayerLeft(Player player)
    {
        // T06: Handle departures during a board and while waiting for the next tournament board.
        if (isPlayerInGame || sessionActive)
        {
            SessionLostEvent?.Invoke();
            inputData.DisableInput();
            isPlayerInGame = false;
            sessionActive = false;
            uiData.uiManager.ChangeState(opponentPlayerLeftState, true);
            uiData.uiManager.ShowCanvas(0);
            // T06: The result state has no OPPlayerLeft transition, so enter the existing disconnect state directly.
            uiData.uiManager.GoToState(StateName.MPHostDisconnectedState);
        }
    }

    // G01: A local network failure stops input and opens the existing disconnect panel with Home.
    private void ConnectionLost(string reason)
    {
        SessionLostEvent?.Invoke();
        inputData.DisableInput();
        isPlayerInGame = false;
        sessionActive = false;
        uiData.uiManager.ChangeState("MultiPlayerStartGame", false);
        uiData.uiManager.ChangeState(mainPlayerLeftState, true);
        uiData.uiManager.ShowCanvas(0);
        uiData.uiManager.GoToState(com.VisionXR.HelperClasses.StateName.MPClientDisconnectState);
        Debug.LogWarning("Network connection lost: " + reason);
    }

    // T06: Final tournament completion ends the session's board-waiting lifecycle.
    public void CompleteSession() { sessionActive = false; }

    public void EndGame()
    {
        isPlayerInGame = false;
        // T06: Ordinary matches end here; tournaments remain active until final completion.
        if (uiOutputData.challenge != com.VisionXR.HelperClasses.Challenge.BWTournament
            && uiOutputData.challenge != com.VisionXR.HelperClasses.Challenge.FSTournament) sessionActive = false;
    }

    public void StartGame()
    {
        // T06: Retain session activity between boards.
        sessionActive = true;
        isPlayerInGame = true;
        StartCoroutine(ShowPlayers());
    }

    private IEnumerator ShowPlayers()
    {
        yield return new WaitForSeconds(0.1f);
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

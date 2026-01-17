using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public NetworkOutputSO networkOutputData;
    public UIOutputDataSO uiOutputData;
    public PlayersDataSO playerData;
    public GameDataSO gameData;
    public CoinData coinData;

    [Header("Game Objects")]
    public Player mainPlayer;
    public PlayerNetworkData mainPlayerNetworkData;


    private void OnEnable()
    {
        uiOutputData.PlayerReadyEvent += SendReadyStatus;
    }

    private void OnDisable()
    {
        uiOutputData.PlayerReadyEvent -= SendReadyStatus;
    }

    public void StartGame(int id)
    {
        SetMainPlayer();
        if (networkOutputData.IsHost())
        {
            mainPlayerNetworkData.RPC_StartGame(id);
          
        }
    }

    public void SendGameData(CurrentGameData data)
    {
       
        SetMainPlayer();
        mainPlayerNetworkData.SetGameData(data);  
    
    }

    public void SendGameResult(GameResult gameResult)
    {
        SetMainPlayer();
        mainPlayerNetworkData.RPC_SendGameData(JsonUtility.ToJson(gameResult));
    }

    public void SendDestroyCoinsInThisTurn(string data)
    {
        SetMainPlayer(); 
        mainPlayerNetworkData.SetDestroyCoins(data);
        
    }

    public void SendFine(PlayerCoin coin)
    {
        SetMainPlayer();
        mainPlayerNetworkData.RPC_PutFine(coin);
    }

    public void SendReadyStatus(int id)
    {
        SetMainPlayer();
        mainPlayerNetworkData.RPC_PlayerReady(id);
    }

    public void SendStrikerChange(int playerId,int strikerId)
    {
        SetMainPlayer();
        mainPlayerNetworkData.RPC_ChangeStriker(playerId,strikerId);
    }

    private void SetMainPlayer()
    {
        if (mainPlayer == null)
        {
            mainPlayer = playerData.GetMainPlayer();
            mainPlayerNetworkData = mainPlayer.gameObject.GetComponent<PlayerNetworkData>();
        }
    }




}

using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Controllers
{
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



        public void StartGame(int turnId,int coinsId)
        {
            SetMainPlayer();
            if (networkOutputData.IsHost())
            {
                mainPlayerNetworkData.RPC_StartGame(turnId,coinsId);

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

        public void SendStrikerChange(int playerId, int strikerId)
        {
            SetMainPlayer();
            mainPlayerNetworkData.RPC_ChangeStriker(playerId, strikerId);
        }

        private void SetMainPlayer()
        {
            if (mainPlayer == null)
            {
                mainPlayer = playerData.GetMainPlayer();
                mainPlayerNetworkData = mainPlayer.gameObject.GetComponent<PlayerNetworkData>();
            }
        }

        public void PlayAgain()
        {

            if (networkOutputData.IsHost())
            {
                Player p = playerData.GetMainPlayer();
                PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
                networkData.RPC_HostReady();

            }
            else
            {
                Player p = playerData.GetMainPlayer();
                PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
                networkData.RPC_ClientReady();

            }
        }

    }
}

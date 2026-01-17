using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [Header("Scriptable Objects")]
    public NetworkOutputSO networkOutputData;
    public MyPlayerSettings playerSettings;
    public UIOutputDataSO uiData;
    public PlayersDataSO playersData;

    [Header("Game Objects")]  
    public GameObject NetworkPlayer;
    public GameObject AINetworkPlayer;
    

    // local variables
    private NetworkObject myPlayer;
    private NetworkObject aIPlayer;

    public void PlayerJoined(PlayerRef player)
    {
       
        networkOutputData.SetHost(Runner.IsSharedModeMasterClient);

        if (Runner.LocalPlayer == player)
        {
            SpawnPlayer(player);
        }
           
    }

    public void PlayerLeft(PlayerRef player)
    {
   

        if (Runner.LocalPlayer == player)
        {      
            DespawnPlayer();
        }

        if (player.IsMasterClient)
        {
            // set new host if the master client leaves

            Player newPlayer = playersData.GetMainPlayer();
            if (newPlayer!= null && newPlayer.myId == 2)
            {
                networkOutputData.SetHost(true);
            }
        }

    }

    public void SpawnPlayer(PlayerRef playerRef)
    {
        if(uiData.multiPlayerGameMode == MultiPlayerGameMode.P1vsP2)
        {
            CreatePlayerForP1VsP2(playerRef);
        }
        else if(uiData.multiPlayerGameMode == MultiPlayerGameMode.P1P2vsAI)
        {
            CreatePlayersForP1P2VsAI(playerRef);
        }
        else if (uiData.multiPlayerGameMode == MultiPlayerGameMode.P1AIvsP2AI)
        {
            CreatePlayersForP1AIVsP2AI(playerRef);
        }
        else
        {
            CreatePlayerForP1P2VsP3P4(playerRef);
        }

    }


    public void DespawnPlayer()
    {
        Runner.Despawn(myPlayer);
    }


    public void CreatePlayerForP1VsP2(PlayerRef playerRef)
    {
        PlayerProperties p = new PlayerProperties();

        p.myId = playerRef.PlayerId;

        p.myOculusID = playerSettings.MyOculusId;
        p.myName = playerSettings.MyName;
        p.myStrikerID = playerSettings.MyStrikerId;
        p.imageURL = playerSettings.ImageUrl;

        p.myPlayerControl = PlayerControl.Local;
        p.myPlayerRole = PlayerRole.Human;


        p.myAiDifficulty = uiData.aIDifficulty;

        if (playerRef.PlayerId == 1)
        {

            p.myCoin = PlayerCoin.White;
            p.myTeam = Team.TeamA;
        }
        else
        {

            p.myCoin = PlayerCoin.Black;
            p.myTeam = Team.TeamB;
        }

        myPlayer = Runner.Spawn(NetworkPlayer, Vector3.zero, Quaternion.identity);
        PlayerNetworkData playerNetworkData = myPlayer.GetComponent<PlayerNetworkData>();
        playerNetworkData.SetPlayerData(p);
    }

    public void CreatePlayersForP1P2VsAI(PlayerRef playerRef)
    {
        if(playerRef.PlayerId == 1)
        {
            PlayerProperties p1 = new PlayerProperties();

            p1.myId = 1;
            p1.myOculusID = playerSettings.MyOculusId;
            p1.myName = playerSettings.MyName;
            p1.myStrikerID = playerSettings.MyStrikerId;
            p1.imageURL = playerSettings.ImageUrl;
            p1.myPlayerControl = PlayerControl.Local;
            p1.myPlayerRole = PlayerRole.Human;
            p1.myAiDifficulty = uiData.aIDifficulty;
            p1.myCoin = PlayerCoin.White;
            p1.myTeam = Team.TeamA;


            PlayerProperties p3 = new PlayerProperties();

            p3.myId = 3;
            p3.myOculusID = 3;
            p3.myName = "AI3";
            p3.myStrikerID = 0;
            p3.imageURL = "";
            p3.myPlayerControl = PlayerControl.Local;
            p3.myPlayerRole = PlayerRole.AI;
            p3.myAiDifficulty = uiData.aIDifficulty;
            p3.myCoin = PlayerCoin.Black;
            p3.myTeam = Team.TeamB;

            myPlayer = Runner.Spawn(NetworkPlayer, Vector3.zero, Quaternion.identity);
            PlayerNetworkData playerNetworkData = myPlayer.GetComponent<PlayerNetworkData>();
            playerNetworkData.SetPlayerData(p1);


            aIPlayer = Runner.Spawn(AINetworkPlayer, Vector3.zero, Quaternion.identity);
            playerNetworkData = aIPlayer.GetComponent<PlayerNetworkData>();
            playerNetworkData.SetPlayerData(p3);

        }
        else
        {
            PlayerProperties p2 = new PlayerProperties();

            p2.myId = 2;
            p2.myOculusID = playerSettings.MyOculusId;
            p2.myName = playerSettings.MyName;
            p2.myStrikerID = playerSettings.MyStrikerId;
            p2.imageURL = playerSettings.ImageUrl;
            p2.myPlayerControl = PlayerControl.Local;
            p2.myPlayerRole = PlayerRole.Human;
            p2.myAiDifficulty = uiData.aIDifficulty;
            p2.myCoin = PlayerCoin.White;
            p2.myTeam = Team.TeamA;


            PlayerProperties p4 = new PlayerProperties();

            p4.myId = 4;
            p4.myOculusID = 4;
            p4.myName = "AI4";
            p4.myStrikerID = 0;
            p4.imageURL = "";
            p4.myPlayerControl = PlayerControl.Local;
            p4.myPlayerRole = PlayerRole.AI;
            p4.myAiDifficulty = uiData.aIDifficulty;
            p4.myCoin = PlayerCoin.Black;
            p4.myTeam = Team.TeamB;

            myPlayer = Runner.Spawn(NetworkPlayer, Vector3.zero, Quaternion.identity);
            PlayerNetworkData playerNetworkData = myPlayer.GetComponent<PlayerNetworkData>();
            playerNetworkData.SetPlayerData(p2);


            aIPlayer = Runner.Spawn(AINetworkPlayer, Vector3.zero, Quaternion.identity);
            playerNetworkData = aIPlayer.GetComponent<PlayerNetworkData>();
            playerNetworkData.SetPlayerData(p4);

        }

    }

    public void CreatePlayersForP1AIVsP2AI(PlayerRef playerRef)
    {
        if (playerRef.PlayerId == 1)
        {
            PlayerProperties p1 = new PlayerProperties();

            p1.myId = 1;
            p1.myOculusID = playerSettings.MyOculusId;
            p1.myName = playerSettings.MyName;
            p1.myStrikerID = playerSettings.MyStrikerId;
            p1.imageURL = playerSettings.ImageUrl;
            p1.myPlayerControl = PlayerControl.Local;
            p1.myPlayerRole = PlayerRole.Human;
            p1.myAiDifficulty = uiData.aIDifficulty;
            p1.myCoin = PlayerCoin.White;
            p1.myTeam = Team.TeamA;


            PlayerProperties p2 = new PlayerProperties();

            p2.myId = 2;
            p2.myOculusID = 2;
            p2.myName = "AI2";
            p2.myStrikerID = 0;
            p2.imageURL = "";
            p2.myPlayerControl = PlayerControl.Local;
            p2.myPlayerRole = PlayerRole.AI;
            p2.myAiDifficulty = uiData.aIDifficulty;
            p2.myCoin = PlayerCoin.White;
            p2.myTeam = Team.TeamA;

            myPlayer = Runner.Spawn(NetworkPlayer, Vector3.zero, Quaternion.identity);
            PlayerNetworkData playerNetworkData = myPlayer.GetComponent<PlayerNetworkData>();
            playerNetworkData.SetPlayerData(p1);


            aIPlayer = Runner.Spawn(AINetworkPlayer, Vector3.zero, Quaternion.identity);
            playerNetworkData = aIPlayer.GetComponent<PlayerNetworkData>();
            playerNetworkData.SetPlayerData(p2);

        }
        else
        {
            PlayerProperties p3 = new PlayerProperties();

            p3.myId = 3;
            p3.myOculusID = playerSettings.MyOculusId;
            p3.myName = playerSettings.MyName;
            p3.myStrikerID = playerSettings.MyStrikerId;
            p3.imageURL = playerSettings.ImageUrl;
            p3.myPlayerControl = PlayerControl.Local;
            p3.myPlayerRole = PlayerRole.Human;
            p3.myAiDifficulty = uiData.aIDifficulty;
            p3.myCoin = PlayerCoin.Black;
            p3.myTeam = Team.TeamB;


            PlayerProperties p4 = new PlayerProperties();

            p4.myId = 4;
            p4.myOculusID = 4;
            p4.myName = "AI4";
            p4.myStrikerID = 0;
            p4.imageURL = "";
            p4.myPlayerControl = PlayerControl.Local;
            p4.myPlayerRole = PlayerRole.AI;
            p4.myAiDifficulty = uiData.aIDifficulty;
            p4.myCoin = PlayerCoin.Black;
            p4.myTeam = Team.TeamB;

            myPlayer = Runner.Spawn(NetworkPlayer, Vector3.zero, Quaternion.identity);
            PlayerNetworkData playerNetworkData = myPlayer.GetComponent<PlayerNetworkData>();
            playerNetworkData.SetPlayerData(p3);


            aIPlayer = Runner.Spawn(AINetworkPlayer, Vector3.zero, Quaternion.identity);
            playerNetworkData = aIPlayer.GetComponent<PlayerNetworkData>();
            playerNetworkData.SetPlayerData(p4);

        }

    }

    public void CreatePlayerForP1P2VsP3P4(PlayerRef playerRef)
    {
        PlayerProperties p = new PlayerProperties();

        p.myId = playerRef.PlayerId;

        p.myOculusID = playerSettings.MyOculusId;
        p.myName = playerSettings.MyName;
        p.myStrikerID = playerSettings.MyStrikerId;
        p.imageURL = playerSettings.ImageUrl;

        p.myPlayerControl = PlayerControl.Local;
        p.myPlayerRole = PlayerRole.Human;


        p.myAiDifficulty = uiData.aIDifficulty;

        if (playerRef.PlayerId == 1 || playerRef.PlayerId == 2)
        {

            p.myCoin = PlayerCoin.White;
            p.myTeam = Team.TeamA;
        }
        else
        {

            p.myCoin = PlayerCoin.Black;
            p.myTeam = Team.TeamB;
        }

        myPlayer = Runner.Spawn(NetworkPlayer, Vector3.zero, Quaternion.identity);
        PlayerNetworkData playerNetworkData = myPlayer.GetComponent<PlayerNetworkData>();
        playerNetworkData.SetPlayerData(p);
    }

}
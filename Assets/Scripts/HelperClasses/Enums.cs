
namespace com.VisionXR.HelperClasses
{


    public enum SwipeDirection { LEFT, RIGHT };
    public enum DominantHand { RIGHT, LEFT ,BOTH}

    public enum Device { Oculus,Editor,Android,Ios}
    public enum StrikerName { Striker1, Striker2, Striker3, Striker4 }
    public enum EventCodes
    {
        gameData, coinData, strikerData, avatarData,
        turnData, StartGame, destroyCoins, TurnInformation, GameResult, PutFine, SoundData, AvatarData,
        NetworkPlayer, StrikerArrowOff, PlayerReady, AIMovement, AllCoinsRot
    }
    public enum NetworkType { Host, Client }
    public enum PlayerRole { Human, AI }
    public enum PlayerControl { Local, Remote }
    public enum PlayerCoin { Black, White, Red, All }
    public enum Team { TeamA, TeamB }
    public enum GameType { SinglePlayer, MultiPlayer, Tutorial,TrickShots,Home}
    public enum Game { BlackAndWhite, FreeStyle, Tournament, TrickShots }
    public enum MultiPlayerGameMode { P1vsP2, P1AIvsP2AI, P1P2vsAI, P1P2vsP3P4 }
    public enum SinglePlayerGameMode { PvsAI, PAIvsAI }
    public enum AIDifficulty { Easy, Medium, Hard }
    public enum GameState { Idle,Starting,Running }
    public enum RoomType { Public,Private}
    public enum AudioType { Coin, Edge, Hole }

   
    public enum ServerRegion { any, us, @in, eu, asia, au, uae, jp, kr, cae, hk, sa, tr, ussc, usw }
   

}


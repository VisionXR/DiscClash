
namespace com.VisionXR.HelperClasses
{

    public enum LoginType { Google, Guest }
    public enum SwipeDirection { LEFT, RIGHT,UP,DOWN };
    public enum TouchZone { LEFT,RIGHT,MIDDLE}
    public enum NetworkType { Host, Client }
    public enum PlayerRole { Human, AI }
    public enum PlayerControl { Local, Remote }
    public enum PlayerCoin { Black, White, Red, All }
    public enum Team { TeamA, TeamB }
    public enum GameType { PracticeOffline, OnlineMultiPlayer, PlayWithFriends,Tutorial , Home, VsCPU }
    public enum Challenge { BlackAndWhite, FreeStyle, Tournament, TrickShots }
    public enum GameMode { PvsAI, PAIvsAI,P1vsP2, P1AIvsP2AI, P1P2vsAI, P1P2vsP3P4 }
    public enum AIDifficulty { Easy, Medium, Hard }
    public enum GameState { Idle,Starting,Running }
    public enum RoomType { Public,Private}
    public enum AudioType { Coin, Edge, Hole }

   
    public enum ServerRegion { any, us, @in, eu, asia, au, uae, jp, kr, cae, hk, sa, tr, ussc, usw }
   

}


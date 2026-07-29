
namespace com.VisionXR.HelperClasses
{
    public enum StateName
    {
        TestState, LeaderBoardState, AchievementsState, RulesState, SettingsState, PurchaseState, QuitState,
        IdleState, HomeState, SinglePlayerState, SPInGameState, SPGameCompletedState, SPLeaderBoardState, SPPauseState, SPSettingsState, SPRulesState,
        MultiPlayerState, MPDestinationState, JoinRoomState, LobbyState, ExitLobbyState, MPInGameState, MPPauseState, MPSettingsState, MPRulesState,
        MPGameCompletedState, MPLeaderBoardState, MPHostDisconnectedState, MPClientDisconnectState, 
        ChangeDestinationState, Tutorial, InfoState, LoginState,AssetsState,MPAssetsState,
        SPBoardsState,SPStrikersState,SPCoinsState,MPBoardsState,MPStrikersState,MPCoinsState
    }
    public enum LoginType { Google, Guest }
    public enum SwipeDirection { LEFT, RIGHT,UP,DOWN };
    public enum TouchZone { LEFT,RIGHT,MIDDLE}
    public enum NetworkType { Host, Client }
    public enum PlayerRole { Human, AI }
    public enum PlayerControl { Local, Remote }
    public enum PlayerCoin { Black, White, Red, All }
    public enum Team { TeamA, TeamB }
    public enum GameType { VsCPU , PlayWithFriends,Tutorial}
    public enum Challenge { BlackAndWhite, FreeStyle, Tournament, TrickShots }
    public enum BoardType { Square4, Circle4,Octagon4 }

    public enum AchievementSection { SinglePlayer, MultiPlayer, General }

    public enum AchievementType { Simple, Progess }
    public enum SinglePlayerGameMode { PvsAI, PAIvsAI }
    public enum MultiPlayerGameMode { P1vsP2, P1AIvsP2AI, P1P2vsAI, P1P2vsP3P4 }
    public enum AIDifficulty { Easy, Medium, Hard }
    public enum GameState { Idle,Starting,Running }
    public enum RoomType { Public,Private}
    public enum RoomJoinType { Create,Join}
    public enum AudioType { Coin, Edge, Hole }

    public enum DominantHand {  Right,Left }

    public enum ServerRegion { any, us, @in, eu, asia, au, uae, jp, kr, cae, hk, sa, tr, ussc, usw }
   

}


using Fusion;
using System;
using UnityEngine;

namespace com.VisionXR.HelperClasses
{


    [Serializable]
    public struct StrikerSnapshot : INetworkStruct
    {
        [Networked] public Vector3Compressed Position { get; set; }
        [Networked] public Vector3Compressed Rotation { get; set; }

    }

    [Serializable]
    public struct CoinSnapshot : INetworkStruct
    {
        [Networked] public Vector3Compressed Position { get; set; }
        [Networked] public Vector3Compressed Rotation { get; set; }
        [Networked] public Vector3Compressed Velocity { get; set; }
    }

    [Serializable]
    public struct GameSnapshot : INetworkStruct
    {
        [Networked] public int FrameNumber { get; set; }
        [Networked] public CoinSnapshot Striker { get; set; }

        // You can adjust 22 if you want a smaller/larger max coin count
        [Networked, Capacity(22)] public NetworkArray<CoinSnapshot> Coins => default;
    }

   

    [Serializable]
    public struct NetworkGameData : INetworkStruct
    {
        [Networked] public int TotalCoins { get; set; }
        [Networked] public int TotalWhites { get; set; }
        [Networked] public int TotalBlacks { get; set; }
        [Networked] public int TotalReds { get; set; }

        [Networked] public int P1Whites { get; set; }
        [Networked] public int P1Blacks { get; set; }
        [Networked] public int P1Red { get; set; }

        [Networked] public int P2Whites { get; set; }
        [Networked] public int P2Blacks { get; set; }
        [Networked] public int P2Red { get; set; }

        [Networked] public int P3Whites { get; set; }
        [Networked] public int P3Blacks { get; set; }
        [Networked] public int P3Red { get; set; }

        [Networked] public int P4Whites { get; set; }
        [Networked] public int P4Blacks { get; set; }
        [Networked] public int P4Red { get; set; }

        [Networked] public NetworkBool isRedCovered { get; set; }
        [Networked] public NetworkBool ShouldICoverCoin { get; set; }
        [Networked] public NetworkBool isGameCompleted { get; set; }

    }

    [Serializable]
    public struct NetworkAvatarData : INetworkStruct
    {
        [Networked] public Vector3Compressed HeadPos { get; set; }
        [Networked] public Vector3Compressed HeadRot { get; set; }

        [Networked] public Vector3Compressed LeftHandPos { get; set; }
        [Networked] public Vector3Compressed LeftHandRot { get; set; }

        [Networked] public Vector3Compressed RightHandPos { get; set; }
        [Networked] public Vector3Compressed RightHandRot { get; set; }

    }
}

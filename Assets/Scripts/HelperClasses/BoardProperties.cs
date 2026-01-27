using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;

public class BoardProperties : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public CoinDataSO coinData;
    public MyPlayerSettings playerSettings;
    public BoardDataSO boardData;

    [Header("Board Properties")]
    [SerializeField] private List<GameObject> Holes;
    [SerializeField] private List<GameObject> HolesTriggers;
    [SerializeField] private List<Transform> Striker1Positions;
    [SerializeField] private List<Transform> Striker2Positions;
    [SerializeField] private List<Transform> Striker3Positions;
    [SerializeField] private List<Transform> Striker4Positions;
    [SerializeField] private List<Transform> FinePositions;
    [SerializeField] private List<Transform> PlayerPositions;
    [SerializeField] private List<Transform> AvatarPositions;
    [SerializeField] private GameObject Ground;
    [SerializeField] private Transform AllCoins;
    [SerializeField] private float StrikerRadius;
    [SerializeField] private float CoinRadius;

    private void Start()
    {
        // Set all properties from this script to the ScriptableObject

            boardData.SetHoles(Holes);

            boardData.SetHoleTriggers(HolesTriggers);

            boardData.SetAllCoinsTransform(AllCoins);

            boardData.SetStrikerPositions(1, Striker1Positions);

            boardData.SetStrikerPositions(2, Striker2Positions);

            boardData.SetStrikerPositions(3, Striker3Positions);

            boardData.SetStrikerPositions(4, Striker4Positions);

            boardData.SetFinePositions(FinePositions);

            boardData.SetPlayerPositions(PlayerPositions);

            boardData.SetAvatarPositions(AvatarPositions);

            boardData.SetStrikerRadius(StrikerRadius);

            boardData.SetCoinRadius(CoinRadius);

            boardData.SetGround(Ground);

    }
}



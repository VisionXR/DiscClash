using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;

public class BoardProperties : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public CoinDataSO coinData;
    public MyPlayerSettings playerSettings;
    public BoardPropertiesSO boardProperties;

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

            boardProperties.SetHoles(Holes);

            boardProperties.SetHoleTriggers(HolesTriggers);

            boardProperties.SetAllCoinsTransform(AllCoins);

            boardProperties.SetStrikerPositions(1, Striker1Positions);

            boardProperties.SetStrikerPositions(2, Striker2Positions);

            boardProperties.SetStrikerPositions(3, Striker3Positions);

            boardProperties.SetStrikerPositions(4, Striker4Positions);

            boardProperties.SetFinePositions(FinePositions);

            boardProperties.SetPlayerPositions(PlayerPositions);

            boardProperties.SetAvatarPositions(AvatarPositions);

            boardProperties.SetStrikerRadius(StrikerRadius);

            boardProperties.SetCoinRadius(CoinRadius);

            boardProperties.SetGround(Ground);

    }
}



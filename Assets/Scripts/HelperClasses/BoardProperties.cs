using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class BoardProperties : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public BoardDataSO boardData;

    [Header("Board Properties")]
    [SerializeField] private List<GameObject> Holes;
    [SerializeField] private List<GameObject> HolesTriggers;
    [SerializeField] private List<GameObject> StrikerRotations;
    [SerializeField] private List<SplineContainer> strikerStrips;
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

            boardData.SetStrikerRotations(StrikerRotations);

            boardData.SetAllCoinsTransform(AllCoins);

            boardData.SetStrikerStrips(strikerStrips);


            boardData.SetFinePositions(FinePositions);

            boardData.SetPlayerPositions(PlayerPositions);

            boardData.SetAvatarPositions(AvatarPositions);

            boardData.SetStrikerRadius(StrikerRadius);

            boardData.SetCoinRadius(CoinRadius);

            boardData.SetGround(Ground);

    }
}



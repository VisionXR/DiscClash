using com.VisionXR.ModelClasses;

using System.Collections;

using System;

using System.Collections.Generic;

using UnityEngine;

public class BoardProperties : MonoBehaviour

{

    public static BoardProperties instance;

    [Header("Scriptable Objects")]

    public CoinDataSO coinData;

    public MyPlayerSettings playerSettings;

    public BoardPropertiesSO boardProperties;

    [Header("Hole Glows")]

    public GameObject HoleOuterGlow1;

    public GameObject HoleOuterGlow2;

    public GameObject HoleOuterGlow3;

    public GameObject HoleOuterGlow4;

    

    [Header("Board Properties")]

   

    [SerializeField] private List<GameObject> HolesTriggers;

    [SerializeField] private List<GameObject> Holes;

    [SerializeField] private List<GameObject> Striker1Positions;

    [SerializeField] private List<GameObject> Striker2Positions;

    [SerializeField] private List<GameObject> Striker3Positions;

    [SerializeField] private List<GameObject> Striker4Positions;

    [SerializeField] private List<Transform> FinePositions;


    [SerializeField] private List<GameObject> MainCanvasPositions;

    [SerializeField] private List<Transform> PlayerPositions;

    [SerializeField] private List<Transform> AvatarPositions;


    [SerializeField] private GameObject Ground;

    [SerializeField] private Transform AllCoins;

    [SerializeField] private float StrikerRadius;

    [SerializeField] private float CoinRadius;

    [SerializeField] private float BoardHeight;

    [Tooltip("How long the glow stays on after a pocket (seconds).")]

    public float holeGlowDuration = 1f;

    // Track active glow coroutines so rapid pockets don't stack weirdly

    private readonly Dictionary<GameObject, Coroutine> _glowRoutines = new();


    private void OnEnable()

    {

        // Ensure the ScriptableObject is not null

        if (boardProperties != null)

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

            boardProperties.SetMainCanvasPositions(MainCanvasPositions);

            boardProperties.SetStrikerRadius(StrikerRadius);

            boardProperties.SetCoinRadius(CoinRadius);

            boardProperties.SetBoardHeight(BoardHeight);

            boardProperties.SetGround(Ground);

            // You can also handle other data, such as the display materials or display base if needed

            coinData.CoinpocketedUntoHoleEvent += OnCoinFellIntoHole;

            SetAllGlows(false);

        }

        else

        {

            Debug.LogWarning("BoardPropertiesSO ScriptableObject is not assigned.");

        }

    }

    private void OnDisable()

    {

        if (boardProperties != null)

        {

            coinData.CoinpocketedUntoHoleEvent -= OnCoinFellIntoHole;

        }

    }

    // ---- FILL THIS ----

    private void OnCoinFellIntoHole(GameObject hole)

    {



        if (!hole) return;

        int idx = ExtractHoleIndex(hole.name); // 1..4 expected

        GameObject glow = idx switch

        {

            1 => HoleOuterGlow1,

            2 => HoleOuterGlow2,

            3 => HoleOuterGlow3,

            4 => HoleOuterGlow4,

            _ => null

        };

        if (!glow)

        {

            Debug.LogWarning($"[TrickShotManager] No glow mapped for hole '{hole.name}' (index {idx}).");

            return;

        }

        TriggerGlow(glow, holeGlowDuration);

    }

    // ---- Helpers ----

    private void TriggerGlow(GameObject glow, float duration)

    {

        if (_glowRoutines.TryGetValue(glow, out var running) && running != null)

            StopCoroutine(running);

        _glowRoutines[glow] = StartCoroutine(GlowRoutine(glow, duration));

    }

    private IEnumerator GlowRoutine(GameObject glow, float duration)

    {

        glow.SetActive(true);

        yield return new WaitForSeconds(duration);

        glow.SetActive(false);

        _glowRoutines.Remove(glow);

    }

    private void SetAllGlows(bool on)

    {

        if (HoleOuterGlow1) HoleOuterGlow1.SetActive(on);

        if (HoleOuterGlow2) HoleOuterGlow2.SetActive(on);

        if (HoleOuterGlow3) HoleOuterGlow3.SetActive(on);

        if (HoleOuterGlow4) HoleOuterGlow4.SetActive(on);

    }

    private int ExtractHoleIndex(string name)

    {

        if (string.IsNullOrEmpty(name)) return -1;

        // get trailing digits (so "Hole2", "Hole_2", "Something2" all work)

        int end = name.Length - 1;

        int start = end;

        while (start >= 0 && char.IsDigit(name[start])) start--;

        start++;

        if (start <= end && int.TryParse(name.Substring(start, end - start + 1), out int n))

            return n;

        // fallback for exact "HoleN" pattern

        if (name.StartsWith("Hole", StringComparison.OrdinalIgnoreCase) &&

            int.TryParse(name.Substring(4), out n))

            return n;

        return -1;

    }

}



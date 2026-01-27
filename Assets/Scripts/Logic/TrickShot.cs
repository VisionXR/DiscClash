using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickShot : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public StrikerDataSO strikerData;
    public CoinDataSO coinData;
    public InputDataSO inputData;
    public TrickShotsDataSO trickShotsData;
    public BoardDataSO boardData;

    [Header(" Local Objects")]
    public float levelTime = 100f;
    public List<GameObject> AllCoins;

    // Fixed star fractions (3★, then 2★, then 1★)
    private const float ThreeStarFrac = 0.30f;      // 0%  -> 30%  of levelTime
    private const float TwoStarFracCumulative = 0.70f; // 30% -> 70% of levelTime (30% + 40%)

    private float _elapsedTime;
    private Coroutine _timerCo;

    private void OnEnable()
    {
        coinData.CoinFellInHoleEvent += CoinFellInHole;
        strikerData.StrikerFellInHoleEvent += StrikerFellIntoHole;


        trickShotsData.SetCurrentLevelTime(levelTime);
        inputData.ActivateInput();
        _elapsedTime = 0f;
        _timerCo = StartCoroutine(StartTimer());

    }

    private void OnDisable()
    {
        coinData.CoinFellInHoleEvent -= CoinFellInHole;
        strikerData.StrikerFellInHoleEvent -= StrikerFellIntoHole;

        inputData.DeactivateInput();
        if (_timerCo != null)
        {
            StopCoroutine(_timerCo);
            _timerCo = null;
        }
    }

    private void CoinFellInHole(GameObject coin)
    {
        AudioManager.instance.PlayTurnChangedSound();
        if (AllCoins.Contains(coin))
        {
            AllCoins.Remove(coin);
            if (AllCoins.Count == 0)
            {
                EndLevelSuccessWithStars();
            }
        }
    }

    private void StrikerFellIntoHole(GameObject striker)
    {
       if(AllCoins.Count > 0)
        {
            
            GameObject coinPrefab = Instantiate(AllCoins[0],FindCoinPosition(),Quaternion.identity,transform);
            AllCoins.Add(coinPrefab);
            coinData.ShowFoulEvent?.Invoke();
        }
    }

    public void EndLevel()
    {
        if (AllCoins.Count > 0)
        {
          
            trickShotsData.LevelFail();
            CleanupAndDestroy();
        }
        else
        {
            EndLevelSuccessWithStars();
        }
    }

    private void EndLevelSuccessWithStars()
    {
        int stars = ComputeStars(_elapsedTime);
   
        // Prefer LevelSuccess(int) if your SO supports it; else fall back.
        var m = trickShotsData.GetType().GetMethod("LevelSuccess", new System.Type[] { typeof(int) });
        if (m != null) trickShotsData.LevelSuccess(stars);
        else trickShotsData.LevelSuccess(stars);

        CleanupAndDestroy();
    }

    private IEnumerator StartTimer()
    {
        _elapsedTime = 0f;
        while (_elapsedTime < levelTime)
        {
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
        EndLevel(); // time up
    }

    private void CleanupAndDestroy()
    {
        inputData.DeactivateInput();
        if (_timerCo != null) { StopCoroutine(_timerCo); _timerCo = null; }
        Destroy(gameObject);
    }

    // ---------- Stars (fixed 30/40/30) ----------

    private int ComputeStars(float elapsed)
    {
        float t3End = levelTime * ThreeStarFrac;          // 30%
        float t2End = levelTime * TwoStarFracCumulative;  // 70%

        if (elapsed <= t3End) return 3;
        if (elapsed <= t2End) return 2;
        return 1;
    }

    // ---------- UI helpers for your time bar ----------

    /// <summary>Normalized cutoffs for vertical markers at 30% and 70% of the bar.</summary>
    public (float threeStarCutoffN01, float twoStarCutoffN01) GetNormalizedCutoffs()
        => (ThreeStarFrac, TwoStarFracCumulative);

    /// <summary>Absolute seconds for placing labels on the bar.</summary>
    public (float threeStarEndSec, float twoStarEndSec) GetAbsoluteCutoffs()
        => (levelTime * ThreeStarFrac, levelTime * TwoStarFracCumulative);


    /// <summary>
    /// Finds a valid position on the board to place a coin, avoiding overlap with existing coins.
    /// </summary>
    /// <returns>A Vector3 representing the correct position for a new coin.</returns>
    public Vector3 FindCoinPosition()
    {
        bool isThisCorrectPosition = false;
        Vector3 correctPosition = Vector3.zero;
        foreach (Transform fine in boardData.GetFinePositions())
        {
            isThisCorrectPosition = true;
            correctPosition = fine.position;
            Collider[] cols = Physics.OverlapSphere(fine.position, boardData.GetCoinRadius());
            foreach (Collider c in cols)
            {
                if (c.gameObject.tag == "White" || c.gameObject.tag == "Red" || c.gameObject.tag == "Black")
                {
                    isThisCorrectPosition = false;
                }
            }
            if (isThisCorrectPosition)
            {
                break;
            }
        }
        if (!isThisCorrectPosition)
        {
            correctPosition = boardData.GetFinePositions()[0].position + new Vector3(0, 0.1f, 0);
        }
        return correctPosition;
    }
}

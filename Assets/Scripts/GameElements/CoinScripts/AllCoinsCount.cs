using com.VisionXR.ModelClasses;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCoinsCount : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public GameDataSO gameData;
    public BoardDataSO boardData;

    [Header(" Local variables")]
    public List<Rigidbody> coins;
    public int TotalCoins;
    public int TotalWhites;
    public int TotalBlacks;
    public int TotalReds;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        foreach (Rigidbody rb in coins)
        {
            rb.isKinematic = false;
        }

        gameData.SetData(TotalCoins, TotalWhites, TotalBlacks, TotalReds);
    }
}

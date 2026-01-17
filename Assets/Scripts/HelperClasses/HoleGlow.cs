using com.VisionXR.ModelClasses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HoleGlow : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public CoinDataSO coinData;


    [Header(" Hole Objects")]
    public List<GameObject> HoleObjects;
    private void OnEnable()
    {
        coinData.CoinpocketedUntoHoleEvent += CoinPocketed;
    }

    private void OnDisable()
    {
        coinData.CoinpocketedUntoHoleEvent += CoinPocketed;
    }

    private void CoinPocketed(GameObject hole)
    {
        foreach(GameObject obj in HoleObjects)
        {
            if (obj == hole)
            {
                
            }
        }
    }
}

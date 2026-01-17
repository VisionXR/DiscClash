using System;
using UnityEngine;

[Serializable]
public class CoinInfo
{
    public GameObject StrikerPos;
    public GameObject Coin;
    public GameObject Hole;
    public float angle;
    public Vector3 FinalPos;
    public bool isBlockedH = false;
    public bool isBlockedC = false;
    public float distance;
    public GameObject blockedCoinAlongStriker;
    public GameObject blockedCoinAlongHole;
}

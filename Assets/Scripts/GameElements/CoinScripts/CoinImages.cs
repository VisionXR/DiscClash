using com.VisionXR.ModelClasses;
using UnityEngine;

public class CoinImages : MonoBehaviour
{

    [Header(" Scriptable Objects")]
    public UIOutputDataSO uIOutputData;


    [Header(" Sprites ")]
    public Sprite WhiteCoin;
    public Sprite BlackCoin;
    public Sprite RedCoin;
    public Sprite BlackAndWhiteCoin;

    public void OnEnable()
    {
        uIOutputData.SetCoinImages(WhiteCoin, BlackCoin, RedCoin, BlackAndWhiteCoin);
    }
}

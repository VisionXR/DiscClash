using com.VisionXR.ModelClasses;
using UnityEngine;

public class CoinManagerTest : MonoBehaviour
{
     public CoinDataSO coinData;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            coinData.CreateAllCoins();
        }
       
       
    }
}

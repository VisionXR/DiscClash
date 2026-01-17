using com.VisionXR.ModelClasses;
using UnityEngine;

public class CoinRegistration : MonoBehaviour
{
    public CoinDataSO coinData;
    private void OnEnable()
    {
        coinData.RegisterCoin(gameObject.GetComponent<Rigidbody>());

        if(gameObject.tag == "Black")
        {
            coinData.BlackCoins.Add(gameObject);
        }
        else if (gameObject.tag == "White")
        {
            coinData.WhiteCoins.Add(gameObject);
        }
        else if (gameObject.tag == "Red")
        {
            coinData.RedCoin = gameObject;
        }

        if (coinData.AllCoinsReference != null)
        {
            transform.parent = coinData.AllCoinsReference.transform;
        }
    }
    private void OnDisable()
    {

        if (gameObject.tag == "Black")
        {
            if (coinData.BlackCoins.Contains(gameObject))
            {
                coinData.BlackCoins.Remove(gameObject);
            }
        }
        else if (gameObject.tag == "White")
        {
            if (coinData.WhiteCoins.Contains(gameObject))
            {
                coinData.WhiteCoins.Remove(gameObject);
            }
        }
        else if (gameObject.tag == "Red")
        {
            coinData.RedCoin = null;
        }

        coinData.DeRegisterCoin(gameObject.GetComponent<Rigidbody>());
    }
}

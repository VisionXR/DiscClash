using com.VisionXR.ModelClasses;
using UnityEngine;

public class CoinCollision : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public CoinDataSO coinDataSO;

    // local variables
    public bool isPassed = false;
    public void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.tag == "Hole")
        {
         
            isPassed = true;
            coinDataSO.CoinFellInHole(gameObject);
            coinDataSO.CoinPocketedUntoHole(other.gameObject);
                     
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
       
        if(collision.collider.gameObject.tag == "Ground")
        {
            if(!isPassed)
            {
                coinDataSO.CoinFellOnGround(gameObject);             
            }
        }
    }

}

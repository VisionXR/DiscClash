using com.VisionXR.ModelClasses;
using UnityEngine;

public class CoinCollision : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public CoinDataSO coinDataSO;

    public void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.tag == "Hole")
        {
            coinDataSO.CoinFellInHole(gameObject);
            coinDataSO.CoinPocketedUntoHole(other.gameObject);
            GetComponent<MeshCollider>().enabled = false;
                     
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
       
        if(collision.collider.gameObject.tag == "Ground")
        {
           coinDataSO.CoinFellOnGround(gameObject);                       
        }
    }

}

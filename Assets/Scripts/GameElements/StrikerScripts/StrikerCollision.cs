using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

public class StrikerCollision : MonoBehaviour
{
    public StrikerDataSO strikerDataSO;
    public bool isPassed = false;
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hole")
        {
            isPassed = true;
            strikerDataSO.StrikerFellInHole(gameObject);        
            strikerDataSO.StrikerPocketedUntoHole(other.gameObject);
          
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
      
        if (collision.collider.gameObject.tag == "Ground")
        {
            if(!isPassed)
            {
                strikerDataSO.StrikerFellInHole(gameObject);
            }
            else
            {
                isPassed = false;
            }
        }
    }

}

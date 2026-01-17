using UnityEngine;

public class DisplayStrikerCollision : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Striker")
        {
          //  TutorialManager.instance.isStrikerPlacedCorrectly();
        }
    }
}

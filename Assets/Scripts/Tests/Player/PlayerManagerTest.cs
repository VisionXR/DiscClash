using com.VisionXR.ModelClasses;
using Fusion;
using UnityEngine;

public class PlayerManagerTest : MonoBehaviour
{
    
    public NetworkObject player;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {

           // networkData._runner.Despawn(player);
        }
       
    }
}

using com.VisionXR.ModelClasses;
using UnityEngine;

public class StrikerManagerTest : MonoBehaviour
{
    public StrikerDataSO strikerData;
    public int strikerID;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
          //  strikerData.CreateStriker(strikerID,1);
        }
    }
}

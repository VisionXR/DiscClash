using com.VisionXR.HelperClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikerMovementTest : MonoBehaviour
{

    private IStrikerMovement strikerMovement;
    public int StrikerId;
    // Start is called before the first frame update
    void Start()
    {
        strikerMovement = GetComponent<IStrikerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.S))
        {
            strikerMovement.SetStrikerID(StrikerId);
        }       
    }
}

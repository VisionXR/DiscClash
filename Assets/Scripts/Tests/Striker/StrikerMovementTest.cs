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

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            strikerMovement.MoveStriker(SwipeDirection.RIGHT);
        }

        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            strikerMovement.MoveStriker(SwipeDirection.LEFT);
        }

        else if (Input.GetKey(KeyCode.A))
        {
            strikerMovement.AimStriker(SwipeDirection.LEFT);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            strikerMovement.AimStriker(SwipeDirection.RIGHT);
        }

       
    }
}

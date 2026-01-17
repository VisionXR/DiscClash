using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAuthorityTest : MonoBehaviour
{
    public CoinDataSO coinData;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
           // coinData.RequestStateAuthority();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
          //  coinData.ReleaseStateAuthority();
        }
    }
}

using UnityEngine;

public class GestureDetector : MonoBehaviour
{
    public void LeftClosed()
    {
        Debug.Log("Left hand closed gesture detected.");
    }

    public void LeftOpen()
    {
        Debug.Log("Left hand open gesture detected.");
    }

    public void RightClosed()
    {
        Debug.Log("Right hand closed gesture detected.");
    }

    public void RightOpen()
    {
        Debug.Log("Right hand open gesture detected.");
    }


}

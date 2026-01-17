using UnityEngine;

public class EnableLine : MonoBehaviour
{
    public LineRenderer lr;

    private void Start()
    {
        
    }
    private void OnEnable()
    {
        lr.enabled = true;
    }

    private void OnDisable()
    {
        lr.enabled = false;
    }
}

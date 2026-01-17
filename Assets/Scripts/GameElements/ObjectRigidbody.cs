using UnityEngine;

public class ObjectRigidbody : MonoBehaviour,IRigidBody
{

    private Rigidbody rb;
    private void Awake()
    {
       rb =  GetComponent<Rigidbody>();
    }
    public void TurnOffRigidbody()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        rb.isKinematic = true;
    }
    public void TurnOnRigidbody()
    {
        rb.isKinematic = false;
    }
}

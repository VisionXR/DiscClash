using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;

public class StrikerShooting : MonoBehaviour,IStrikerShoot
{
    [Header("Scriptable Objects")]
    public StrikerDataSO strikerData;

    [Header(" Local variables ")]
    public Rigidbody strikerRigidbody;
    [SerializeField] private float period = 10.0f;

    // actions
    public Action<float,Vector3> StrikeStartedEvent;
    public Action StrikeFinishedEvent;
    public Action<float> StrikeForceChangedEvent;
    public Action StrikeForceStartedEvent;

    // variables
    private float StrikeForce = 2;
    private Coroutine WaitRoutine;

    public void FireStriker()
    {

        strikerRigidbody.AddForce(transform.forward * StrikeForce, ForceMode.VelocityChange);
        StrikeStartedEvent?.Invoke(StrikeForce,transform.forward);
        if (WaitRoutine == null)
        {
            WaitRoutine = StartCoroutine(WaituntilStrikeFinished());
        }

    }
    public void SetStrikerForce(float normalizedValue)
    {
        // Map the normalized value to the desired range
        float range = strikerData.ForceUpperLimit - strikerData.ForceLowerLimit;
        StrikeForce = strikerData.ForceLowerLimit + normalizedValue * range;
        StrikeForceChangedEvent?.Invoke(normalizedValue);
    }
    public void FireStriker(Vector3 direction, float force)
    {

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        strikerRigidbody.AddForce(transform.forward * force, ForceMode.VelocityChange);
        StrikeStartedEvent?.Invoke(force, transform.forward);
        if (WaitRoutine == null)
        {
            WaitRoutine = StartCoroutine(WaituntilStrikeFinished());
        }

    }
    private IEnumerator WaituntilStrikeFinished()
    {
        yield return new WaitUntil(() => strikerRigidbody.linearVelocity.magnitude < 0.005f);
        yield return new WaitForSeconds(5);
        StrikeFinishedEvent?.Invoke();
        WaitRoutine = null;
    }

   
}

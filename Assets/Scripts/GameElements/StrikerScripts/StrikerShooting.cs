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
    private bool isFired = false;  
    private float startTime;
    private Coroutine FireRoutine;
    private Coroutine WaitRoutine;

    public void FireStriker()
    {

        strikerRigidbody.AddForce(transform.forward * 4, ForceMode.VelocityChange);
        StrikeStartedEvent?.Invoke(4,transform.forward);
        if (WaitRoutine == null)
        {
            WaitRoutine = StartCoroutine(WaituntilStrikeFinished());
        }

    }

    public void FireStriker(float force)
    {

        if (force > 0.5f && !isFired)
        {
            isFired = true;
            StartArrowChange();
            StrikeForceStartedEvent?.Invoke();
        }

        else if (force < 0.1f && isFired)
        {

            StopArrowChange();
            strikerRigidbody.AddForce(transform.forward * StrikeForce, ForceMode.VelocityChange);
            StrikeStartedEvent?.Invoke(StrikeForce,transform.forward);
            AppProperties.instance.PlayVibration();

            if (WaitRoutine == null)
            {
                WaitRoutine = StartCoroutine(WaituntilStrikeFinished());
            }

            isFired = false;
        }

    }

    public void StartArrowChange()
    {
        startTime = Time.time;
        if (FireRoutine == null)
        {
            FireRoutine = StartCoroutine(WaitAndChangeArrow());
        }
    }

    public void StopArrowChange()
    {
        if (FireRoutine != null)
        {
            StopCoroutine(FireRoutine);
            FireRoutine = null;
        }
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


    private IEnumerator WaitAndChangeArrow()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            float timeSinceStart = Time.time - startTime;
            float period = 2f; // Time taken to complete one full cycle
            float t = timeSinceStart / period; // Normalized time between 0 and 1

            // Linearly interpolate between 0 and 1 and then back to 0
            float normalizedValue = Mathf.PingPong(t, 1f);

            // Map the normalized value to the desired range
            float range = strikerData.ForceUpperLimit - strikerData.ForceLowerLimit;
            StrikeForce = strikerData.ForceLowerLimit + normalizedValue * range;

            StrikeForceChangedEvent?.Invoke(normalizedValue);
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

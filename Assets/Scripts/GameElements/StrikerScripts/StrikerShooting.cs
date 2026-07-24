using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;

public class StrikerShooting : MonoBehaviour,IStrikerShoot
{
    [Header("Scriptable Objects")]
    public StrikerDataSO strikerData;

    [Header(" Local variables ")]
    public StrikerArrow strikerArrow;
    public Rigidbody strikerRigidbody;
    public float period = 10.0f;
    public float cutOffValue = 0.15f;

    // actions
    public Action<float,Vector3> StrikeStartedEvent;
    public Action<float> StrikeForceChangedEvent;
    public Action StrikeFinishedEvent;



    // variables
    private float StrikeForce = 2;
    private Coroutine WaitRoutine;

    public void FireStriker(float val)
    {
        if (val > cutOffValue)
        {
            strikerRigidbody.AddForce(transform.forward * StrikeForce, ForceMode.VelocityChange);
            
          
            if (WaitRoutine == null)
            {
                WaitRoutine = StartCoroutine(WaituntilStrikeFinished());
            }

            strikerArrow.TurnOffArrow();
            StrikeStartedEvent?.Invoke(StrikeForce, transform.forward);
        }


    }

    public void SetStrikerForce(float normalizedValue)
    {
 
        float range = strikerData.ForceUpperLimit - strikerData.ForceLowerLimit;
        StrikeForce = strikerData.ForceLowerLimit + (normalizedValue) * range;
        strikerArrow.ChangeColorOfArrow(normalizedValue);
        StrikeForceChangedEvent?.Invoke(normalizedValue);
    }

    public void SetStrikerArrow(float normalizedValue)
    {
        strikerArrow.ChangeColorOfArrow(normalizedValue);
    }

    public void FireStriker(Vector3 direction, float force)
    {

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        strikerRigidbody.AddForce(transform.forward * force, ForceMode.VelocityChange);
      
   
        if (WaitRoutine == null)
        {
            WaitRoutine = StartCoroutine(WaituntilStrikeFinished());
        }

        strikerArrow.TurnOffArrow();
        StrikeStartedEvent?.Invoke(force, transform.forward);

    }
    private IEnumerator WaituntilStrikeFinished()
    {
        yield return new WaitUntil(() => strikerRigidbody.linearVelocity.magnitude < 0.005f);
        yield return new WaitForSeconds(5);
        StrikeFinishedEvent?.Invoke();
        WaitRoutine = null;
    }
 
}

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
        // T11: Reject a second shot while this shot is still being resolved.
        if (val > cutOffValue && WaitRoutine == null)
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

        // T11: Remote/AI calls obey the same one-shot-at-a-time rule.
        if (WaitRoutine != null) return;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        strikerRigidbody.AddForce(transform.forward * force, ForceMode.VelocityChange);
      
   
        if (WaitRoutine == null)
        {
            WaitRoutine = StartCoroutine(WaituntilStrikeFinished());
        }

        strikerArrow.TurnOffArrow();
        StrikeStartedEvent?.Invoke(force, transform.forward);

    }
    // T11: Coin motion can outlast striker motion; never score the shot using striker velocity alone.
    public bool IsBoardSettled()
    {
        if (IsMoving(strikerRigidbody)) return false;
        if (strikerData.coinData != null)
            foreach (Rigidbody body in strikerData.coinData.AvailableCoinsInGame)
                if (IsMoving(body)) return false;
        return true;
    }

    private bool IsMoving(Rigidbody body)
    {
        return body != null && body.gameObject.activeInHierarchy && !body.isKinematic
            && (body.linearVelocity.sqrMagnitude >= 0.005f * 0.005f
                || body.angularVelocity.sqrMagnitude >= 0.05f * 0.05f);
    }

    // Restore the original shot-completion timing: striker slows down, then wait five seconds.
    private IEnumerator WaituntilStrikeFinished()
    {
        yield return new WaitUntil(() => strikerRigidbody.linearVelocity.magnitude < 0.005f);
        yield return new WaitForSeconds(5);
        WaitRoutine = null;
        StrikeFinishedEvent?.Invoke();
    }
    private void StopBody(Rigidbody body)
    {
        if (body == null || body.isKinematic) return;
        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.Sleep();
    }

    private void OnDisable()
    {
        // T11: A destroyed/hidden board must not retain a pending shot callback on reuse.
        if (WaitRoutine != null) StopCoroutine(WaitRoutine);
        WaitRoutine = null;
    }
}

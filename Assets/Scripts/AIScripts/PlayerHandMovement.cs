using System.Collections;
using UnityEngine;

public class PlayerHandMovement : MonoBehaviour
{
    public GameObject  Hand, HandPos, HandPosRest, AllParts;
    private Animator HandAnimator;
    public GameObject Striker;
    public Vector3 FirePosition;
    public Vector3  HandInitPos, HandInitRot;
    public float Offset = 0.08f;
    void Start()
    {
        HandAnimator = Hand.GetComponent<Animator>();
        HandInitPos = Hand.transform.position;
        HandInitRot = Hand.transform.eulerAngles;
    }

    public void SetStriker(GameObject striker)
    {
     
        this.Striker = striker;
        HandPos = striker.transform.GetChild(0).gameObject;
    }
    public void ShowFingerCloseAnimation()
    {
        Debug.Log(" In finger close animation ");
        HandAnimator.SetBool("StrikeFinished", false);
        HandAnimator.SetBool("CloseFinger", true);
       
    }
    public void ShowFingerStrikeAnimation()
    {
        Debug.Log(" In finger strike animation ");
        HandAnimator.SetBool("CloseFinger", true);
        HandAnimator.SetBool("FingerStrike", true);
        StartCoroutine(AfterStrikeFinished());
    }
    private IEnumerator AfterStrikeFinished()
    {
        yield return new WaitForSeconds(0.1f);
        HandAnimator.SetBool("StrikeFinished", true);
        HandAnimator.SetBool("CloseFinger", false);
        HandAnimator.SetBool("FingerStrike", false);
        
    }

}

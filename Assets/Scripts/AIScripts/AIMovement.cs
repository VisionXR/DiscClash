using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    // Variables

    [Header("Scriptable Objects")]
    public AIDataSO aiData;
    public PlayersDataSO playersData;
    public GameDataSO gameData;
    
    [Header("local Objects")]
    public GameObject Head;
    public GameObject Hand;
    public GameObject AllParts;
    public Animator HandAnimator;

    // local variables
    public Sprite AIIcon;
    public GameObject Striker;
    public Action<string> AIBotAnimationEvent;  
    private Coroutine headNodRoutine;  
    private GameObject HandPos;
    private int MyId;
    private Quaternion desiredRotation,BotInitRotation, HandInitRot,handdesiredRotation, headdesiredRotation,HeadInitRot;
    private Vector3 desiredPosition,handdesiredPosition, BotInitPos, HandInitPos,CoinPos;
    private Vector3 hittingDirection,lookDirection;
    public bool canIAnimate = false;


    // hand move coroutine reference
    private Coroutine handMoveRoutine;

    void SetInitialPosition()
    {
        BotInitPos = transform.position;
        BotInitRotation = transform.rotation;
        HandInitPos = Hand.transform.position;
        HandInitRot = Hand.transform.rotation;
        HeadInitRot = Head.transform.rotation;

        desiredPosition = transform.position;
        desiredRotation = transform.rotation;
        handdesiredPosition = HandInitPos;
        handdesiredRotation = HandInitRot;
        headdesiredRotation = HeadInitRot;

        canIAnimate = true;
    }

    private void OnEnable()
    {     
        aiData.ReceiveAIBotAnimationEvent += MoveBot;       
    }

    void OnDisable()
    {

        aiData.ReceiveAIBotAnimationEvent -= MoveBot;
        Destroy(Hand);
    }

    public void SetStriker(GameObject striker,int id)
    {      
        Striker = striker;
        MyId = id;
        HandPos = striker.GetComponent<StrikerProperties>().HandPosObject;
        SetInitialPosition();
    }
    public void MoveBot(string  data)
    {
        AIBotAnimationDetails details = JsonUtility.FromJson<AIBotAnimationDetails>(data);
        if (MyId == details.myId)
        {

            Vector3 strikerpos = DataConverter.ParseVector3(details.strikerPosition);
            Vector3 strikerRot = DataConverter.ParseVector3(details.strikerRotation);
            Vector3 coinPos = DataConverter.ParseVector3(details.coinPosition);

            if (details.eventId == 1)
            {
                Striker.transform.position = strikerpos;
                Striker.transform.eulerAngles = strikerRot;
              
                Hand.transform.parent = Striker.transform;
                handdesiredPosition = HandPos.transform.position;
                handdesiredRotation = HandPos.transform.rotation;
            }
            else if (details.eventId == 2)
            {
                CoinPos = coinPos;            
                Striker.transform.position = strikerpos ;
                Striker.transform.eulerAngles = strikerRot;
                hittingDirection = (CoinPos - Striker.transform.position).normalized;

                SetHandPosition();
                SetBotPosition();
                StartCoroutine(CloseFinger());
            }
            else if (details.eventId == 3)
            {

                HandAnimator.SetBool("CloseFinger", true);
                HandAnimator.SetBool("FingerStrike", true);
                StartCoroutine(AfterStrikeFinished());
            }
        }
    }

    public void MoveHandToStriker()
    {
            
        Hand.transform.parent = Striker.transform;
        handdesiredPosition = HandPos.transform.position;
        handdesiredRotation = HandPos.transform.rotation;

        StartHandMove(0.5f);
        SendAIMovement(MyId, 1, Vector3.one, Striker.transform.position, Striker.transform.eulerAngles);
           
    }

    public void ShowFingerCloseAnimation(Vector3 coinPos)
    {

            CoinPos = coinPos;
            hittingDirection = (coinPos - Striker.transform.position).normalized;
         //   SetHandPosition();
            SetBotPosition();
            StartCoroutine(CloseFinger());
            SendAIMovement(MyId, 2, coinPos, Striker.transform.position, Striker.transform.eulerAngles);
        
    }
    public void ShowFingerStrikeAnimation(Vector3 coinPos)
    {

            HandAnimator.SetBool("CloseFinger", true);
            HandAnimator.SetBool("FingerStrike", true);
            StartCoroutine(AfterStrikeFinished());
            SendAIMovement(MyId, 3, coinPos, Striker.transform.position, Striker.transform.eulerAngles);
        
    }
    private void SetHandPosition()
    {
        handdesiredRotation = Quaternion.LookRotation(Vector3.Cross(-HandPos.transform.up, hittingDirection), HandPos.transform.up);
        handdesiredPosition = new Vector3(Striker.transform.position.x, HandPos.transform.position.y, Striker.transform.position.z) + hittingDirection * -0.125f;
      
    }
    private void SetBotPosition()
    {
        float angle = Vector3.SignedAngle(transform.forward, hittingDirection, Vector3.up);

        if (angle > 45)
        {
            hittingDirection = Quaternion.AngleAxis(45, Vector3.up) * transform.forward;
        }
        else if (angle < -45)
        {
            hittingDirection = Quaternion.AngleAxis(-45, Vector3.up) * transform.forward;
        }


        desiredRotation = Quaternion.LookRotation(hittingDirection);
        desiredPosition = Striker.transform.position + hittingDirection * -0.55f;

    }
    private IEnumerator CloseFinger()
    {
       
        Hand.transform.parent = null;
        yield return new WaitForSeconds(2);
        
        HandAnimator.SetBool("StrikeFinished", false);
        HandAnimator.SetBool("CloseFinger", true);
    }
    private IEnumerator AfterStrikeFinished()
    {
        yield return new WaitForSeconds(1);
        HandAnimator.SetBool("StrikeFinished", true);
        HandAnimator.SetBool("CloseFinger", false);
        HandAnimator.SetBool("FingerStrike", false);

        yield return new WaitForSeconds(1);
        desiredPosition = BotInitPos;
        desiredRotation = BotInitRotation;
        handdesiredPosition = HandInitPos;
        handdesiredRotation = HandInitRot;

        StartHandMove(0.5f);
        headdesiredRotation = HeadInitRot;
        Hand.transform.parent = AllParts.transform;
    }


   
    /// <summary>
    /// Smoothly move the Hand from its current world position/rotation to handdesiredPosition/handdesiredRotation over 'duration' seconds.
    /// If a previous hand move is running it will be stopped and replaced.
    /// Call StartHandMove(duration) to begin.
    /// </summary>
    /// <param name="duration">Time in seconds for the move. If <= 0 the hand will snap to the target.</param>
    public void StartHandMove(float duration)
    {
        if (handMoveRoutine != null)
        {
            StopCoroutine(handMoveRoutine);
            handMoveRoutine = null;
        }

        handMoveRoutine = StartCoroutine(MoveHandToDesired(duration));
    }
    private IEnumerator MoveHandToDesired(float duration)
    {
        if (Hand == null)
            yield break;


        // If parented, we still operate in world space to avoid messing local offsets
        Vector3 startPos = Hand.transform.position;
        Quaternion startRot = Hand.transform.rotation;

        // Immediate snap when duration is zero or negative
        if (duration <= 0f)
        {
            Hand.transform.position = handdesiredPosition;
            Hand.transform.rotation = handdesiredRotation;
            handMoveRoutine = null;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            // ease-out for a natural feel
            float eased = 1f - Mathf.Pow(1f - t, 2f);

            Hand.transform.position = Vector3.Lerp(startPos, handdesiredPosition, eased);
            Hand.transform.rotation = Quaternion.Slerp(startRot, handdesiredRotation, eased);

            elapsed += Time.deltaTime;
            yield return null;
        }
        // Ensure exact final values
        Hand.transform.position = handdesiredPosition;
        Hand.transform.rotation = handdesiredRotation;

        handMoveRoutine = null;
    }


    public void SendAIMovement(int myId, int eventId, Vector3 coinPos, Vector3 strikePos, Vector3 strikerot)
    {

            AIBotAnimationDetails details = new AIBotAnimationDetails();
            details.time = DateTime.UtcNow.ToString();
            details.myId = myId;
            details.eventId = eventId;
            details.coinPosition = DataConverter.FormatVector3(coinPos);
            details.strikerPosition = DataConverter.FormatVector3(strikePos);
            details.strikerRotation = DataConverter.FormatVector3(strikerot);
            AIBotAnimationEvent?.Invoke(JsonUtility.ToJson(details));            
        
    }

}



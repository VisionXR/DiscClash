using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivePlayerData : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public PlayersDataSO playerData;
    public GameDataSO gameData;
    public CoinDataSO coinData;
    public BoardDataSO boardData;
    public StrikerDataSO strikerData;

    [Header("local Objects")]
    public Player player;
    public PlayerNetworkData networkData;

    // Local Variables

    public int currentFrameNumber;
    public int currentReceivedFrameNumber;

    public bool canISendSnapShot;
    public bool canIReceiveSnapShot;
    public bool canIReceive;

    private GameSnapshot receiveSnapShotData;
    private List<Rigidbody> coinRbs;


    private void OnEnable()
    {
        gameData.TurnChangedEvent += OnTurnChanged;
    }

    private void OnDisable()
    {
        gameData.TurnChangedEvent -= OnTurnChanged;
    }

    private void OnTurnChanged(int obj)
    {
        PlayerStrikeEnded();
    }

    public void PlayerStrikeStarted(float force, Vector3 dir)
    {

        currentFrameNumber = 1;
        currentReceivedFrameNumber = 0;
        canIReceive = true;

      
        player.strikerArrow.TurnOffArrow();
        player.strikerRigidBody.AddForce(dir * force, ForceMode.VelocityChange);

        playerData.PlayerStrikeStartedEvent?.Invoke(gameData.currentTurnId, 1);
    }

    public void PlayerStrikeForceStarted()
    {
        
    }

    public void PlayerStrikeEnded()
    {
        canIReceive = false;
        canIReceiveSnapShot = false;
        currentFrameNumber = 0;
        currentReceivedFrameNumber = 0;

    }

    public void ChangeStrikerReceived(int playerId, int strikerId)
    {
        strikerData.NetworkStrikerEvent?.Invoke(playerId, strikerId);
    }

    public void ReceiveStrikerData(Vector3 pos, Vector3 rot)
    {
        if (!canIReceive)
        {
            player.strikerRigidBody.position = pos;
            player.strikerRigidBody.transform.eulerAngles = rot;
        }
    }

    public void ReceiveSnapShot(GameSnapshot data)
    {
        if (canIReceive)
        {
            receiveSnapShotData = data;
            canIReceiveSnapShot = true;
        }

    }

    public void ReceiveCoinRotationData(float val)
    {
        coinData.SetAllCoinsRotation(val);
    }


    public void ReceiveAIData(string data)
    {
        player.aIMovement.MoveBot(data);
    }

    private void FixedUpdate()
    {
        // receiving     
        ReceiveData();
    }

    public void ReceiveData()
    {
        if (canIReceive)
        {
            currentFrameNumber++;
        }

        if (canIReceive && canIReceiveSnapShot)
        {
            ProcessSnapShot();
            canIReceiveSnapShot = false;

        }
    }
    public void ProcessSnapShot()
    {

        if (receiveSnapShotData.FrameNumber < currentReceivedFrameNumber)
        {
            return;
        }

        currentReceivedFrameNumber = receiveSnapShotData.FrameNumber;
        int frameDelta = currentFrameNumber - currentReceivedFrameNumber;

        float dt = Time.fixedDeltaTime * (frameDelta + playerData.DelayRate);



        // Decode striker snapshot
        CoinSnapshot strikerData = receiveSnapShotData.Striker;
        Vector3 strikerRot = strikerData.Rotation;
        // Estimate striker position
        Vector3 strikerEstimated =
        GetEstimatedReflectedPoint(strikerData.Position, strikerData.Velocity, player.strikerProperties.time, dt, boardData.GetStrikerRadius());


        Vector3 VelEstimated = (Vector3)strikerData.Velocity * Mathf.Exp(-player.strikerProperties.time * dt);



        // Make striker kinematic before correction
        player.strikerRigidBody.isKinematic = true;
        // Lerp to estimated striker state
        StartCoroutine(LerpToTarget(player.strikerRigidBody, strikerEstimated, VelEstimated, strikerRot, playerData.DelayRate * Time.fixedDeltaTime));


        coinRbs = coinData.AvailableCoinsInGame;
        for (int i = 0; i < coinData.AvailableCoinsInGame.Count; i++)
        {

            CoinSnapshot coinData = receiveSnapShotData.Coins[i];
            Vector3 coinPos = coinData.Position;
            Vector3 coinVel = coinData.Velocity;
            Vector3 coinRot = coinData.Rotation;

            Vector3 CoinEstimated =
            GetEstimatedReflectedPoint(coinPos, coinVel, playerData.coinK, dt, boardData.GetCoinRadius());

            Vector3 CoinVelEstimated = coinVel * Mathf.Exp(-playerData.coinK * dt);
            coinRbs[i].isKinematic = true;
            StartCoroutine(LerpToTarget(coinRbs[i], CoinEstimated, CoinVelEstimated, coinRot, playerData.DelayRate * Time.fixedDeltaTime));

        }
    }

    public void SetAllCoinsRotation()
    {
        // Decode striker snapshot
        CoinSnapshot strikerData = receiveSnapShotData.Striker;
        Vector3 strikerRot = strikerData.Rotation;
        player.strikerRigidBody.transform.eulerAngles = strikerRot;


        coinRbs = coinData.AvailableCoinsInGame;
        for (int i = 0; i < coinData.AvailableCoinsInGame.Count; i++)
        {
            CoinSnapshot coinData = receiveSnapShotData.Coins[i];
            Vector3 coinRot = coinData.Rotation;
            coinRbs[i].transform.eulerAngles = coinRot;
        }
    }


    public float EstimateYRotation(float initialYRotation, float initialAngularVelocityRad, float k, float t)
    {
        float initialAngularVelocityDeg = initialAngularVelocityRad * Mathf.Rad2Deg;
        float deltaAngle = (initialAngularVelocityDeg / k) * (1f - Mathf.Exp(-k * t));
        return initialYRotation + deltaAngle;
    }

    private IEnumerator LerpToTarget(Rigidbody rb, Vector3 targetPos, Vector3 targetVel, Vector3 targetRot, float duration)
    {
        Vector3 start = rb.position;
        Vector3 startRot = rb.transform.eulerAngles;
        float elapsed = 0;
        while (elapsed < duration)
        {
            rb.position = Vector3.Lerp(start, targetPos, elapsed / duration);

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        rb.position = targetPos;
        rb.isKinematic = false;
        rb.linearVelocity = targetVel;
        rb.transform.eulerAngles = targetRot;
        //  rb.angularVelocity = Vector3.zero;

    }
    public Vector3 GetEstimatedReflectedPoint(Vector3 startPos, Vector3 velocity, float k, float dt, float radius)
    {
        Vector3 direction = velocity.normalized;
        float decayFactor = 1 - Mathf.Exp(-k * dt);
        float totalDistance = (velocity.magnitude / k) * decayFactor;

        // First raycast
        if (Physics.SphereCast(startPos, radius, direction, out RaycastHit hit1, totalDistance))
        {
            if (hit1.collider.CompareTag("Edge"))
            {
                float distToHit1 = hit1.distance - radius;
                float remainingDist1 = totalDistance - distToHit1;

                Vector3 reflectedDir1 = Vector3.Reflect(direction, hit1.normal);

                // Second raycast from hit point in reflected direction
                if (Physics.SphereCast(hit1.point - direction * radius, boardData.GetStrikerRadius(), reflectedDir1, out RaycastHit hit2, remainingDist1))
                {
                    if (hit2.collider.CompareTag("Edge"))
                    {
                        float distToHit2 = hit2.distance - radius;
                        float remainingDist2 = remainingDist1 - distToHit2;

                        Vector3 reflectedDir2 = Vector3.Reflect(reflectedDir1, hit2.normal);
                        return hit2.point - reflectedDir1 * radius + reflectedDir2 * remainingDist2;
                    }
                    else
                    {
                        return hit1.point - direction * radius + reflectedDir1 * remainingDist1;
                    }
                }
                else
                {
                    return hit1.point - direction * radius + reflectedDir1 * remainingDist1;
                }
            }
        }

        // No bounce
        return startPos + direction * totalDistance;
    }


}

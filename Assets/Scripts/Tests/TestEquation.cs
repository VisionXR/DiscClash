using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;

public class TestEquation : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public TestDataSO testData;
    public PlayersDataSO playerData;
    public GameDataSO gameData;
    public BoardPropertiesSO boardData;

    [Header(" Game Objects")]
    public Rigidbody strikerObject;

    [Header(" Line Renderes")]
    public LineRenderer actualLine;
    public LineRenderer predictedLine;

    [Header("Local variables")]
    public float delayTime = 2;
    private bool canIRecord;
    private int currentFrameNumber = 1;
    public bool showActual;



    private void OnEnable()
    {

        SetLines();
        testData.StartRecordingEvent += StartRecord;
        testData.StartActualEvent += ShowActual;
        testData.StartPredictionEvent += ShowPrediction;
    }

    private void OnDisable()
    {
        testData.StartRecordingEvent -= StartRecord;
        testData.StartActualEvent -= ShowActual;
        testData.StartPredictionEvent -= ShowPrediction;
    }

    private void ShowActual(int id)
    {
        actualLine.positionCount = 1;
        for (int i = 0; i<id;i++)   
        {
            SampleCoinData data = testData.data[i];
            actualLine.SetPosition(actualLine.positionCount-1, DataConverter.ParseVector3(data.position));
            actualLine.positionCount++;
        }
    }

    private void ShowPrediction(int id,int countNumber)
    {
       
        SampleCoinData data = testData.data[id];
        Vector3 position = DataConverter.ParseVector3(data.position);
        Vector3 velocity = DataConverter.ParseVector3(data.velocity);

        predictedLine.positionCount = 1;
        predictedLine.SetPosition(0, position);
        predictedLine.positionCount = 2;
        for (int i = 0; i <= countNumber; i++)         
        {
            float dt = i * Time.fixedDeltaTime;    
            predictedLine.SetPosition(predictedLine.positionCount-1, GetEstimatedReflectedPoint(position, velocity, 1.25f, dt,boardData.GetStrikerRadius()));         
            predictedLine.positionCount++;
        }

    }

    private void StartRecord()
    {
        canIRecord = !canIRecord;
    }

    private void FixedUpdate()
    {
        if(canIRecord)
        {
            SampleCoinData data = new SampleCoinData();
            data.FrameNumber = currentFrameNumber;
            data.position = DataConverter.FormatVector3(strikerObject.position);
            data.rotataion = DataConverter.FormatVector3(strikerObject.transform.eulerAngles) ;
            data.velocity = DataConverter.FormatVector3(strikerObject.linearVelocity);
            data.angularVelocity = strikerObject.angularVelocity.y;
            testData.AddData(data);

            currentFrameNumber++;
        }
    }

    public Vector3 GetEstimatedReflectedPoint(Vector3 startPos, Vector3 velocity, float k, float dt,float radius)
    {
        Vector3 direction = velocity.normalized;
        float decayFactor = 1 - Mathf.Exp(-k * dt);
        float totalDistance = (velocity.magnitude / k) * decayFactor;

        // First raycast
        if (Physics.SphereCast(startPos,radius, direction, out RaycastHit hit1, totalDistance))
        {
            if (hit1.collider.CompareTag("Edge"))
            {
                float distToHit1 = hit1.distance - radius;
                float remainingDist1 = totalDistance - distToHit1;

                Vector3 reflectedDir1 = Vector3.Reflect(direction, hit1.normal);

                // Second raycast from hit point in reflected direction
                if (Physics.SphereCast(hit1.point - direction*radius,boardData.GetStrikerRadius(), reflectedDir1, out RaycastHit hit2, remainingDist1))
                {
                    if (hit2.collider.CompareTag("Edge"))
                    {
                        float distToHit2 = hit2.distance - radius;
                        float remainingDist2 = remainingDist1 - distToHit2;

                        Vector3 reflectedDir2 = Vector3.Reflect(reflectedDir1, hit2.normal);
                        return hit2.point-reflectedDir1*radius + reflectedDir2 * remainingDist2;
                    }
                    else
                    {
                        return hit1.point - direction * radius + reflectedDir1 * remainingDist1;
                    }
                }
                else
                {
                    return hit1.point- direction*radius + reflectedDir1 * remainingDist1;
                }
            }
        }

        // No bounce
        return startPos + direction * totalDistance;
    }



    private void SetLines()
    {
        actualLine.startWidth = 0.01f;
        actualLine.endWidth = 0.01f;
        actualLine.startColor = Color.green;
        actualLine.endColor = Color.green;
        actualLine.positionCount = 0;

        predictedLine.startWidth = 0.01f;
        predictedLine.endWidth = 0.01f;
        predictedLine.startColor = Color.red;
        predictedLine.endColor = Color.red;
        predictedLine.positionCount = 0;
    }

   
    public void StartStrike()
    {
        StartCoroutine(Striking());
    }

    private IEnumerator Striking()
    {
        yield return new WaitForSeconds(delayTime);
        Player p = playerData.GetPlayer(gameData.currentTurnId);
        p.strikerShoot.FireStriker();
    }
}
                                            
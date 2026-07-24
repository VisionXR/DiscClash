using com.VisionXR.ModelClasses;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public BoardDataSO boardData;

    [Header("Game Objects")]
    public Transform checkTransform;
    public Renderer lineRenderer;
    public Renderer circleRenderer;
    public GameObject line;
    public GameObject quadCircle;

    [Header("Properties")]
    public LayerMask colliderMask;
    public float CutOffLength = 1f;
    public float arrowCutOffLength = 0.15f;
    public float LineThickness = 0.08f;
    public int NoofArrowsPerUnit = 5;
    public float distanceFactor = 0.075f;

    public float strikerOffset = 0.01f;

    // local variables
    private RaycastHit hit;

    public void SetCutOffLength(float d)
    {
        CutOffLength = d;
    }

    public void SetColor(Color color)
    {
        if (lineRenderer != null && lineRenderer.material != null)
        {
            lineRenderer.material.color = color;
            circleRenderer.material.color = color;
        }
    }

    private void FixedUpdate()
    {
        // SphereCast up to CutOffLength
        bool hasHit = Physics.SphereCast(checkTransform.position, boardData.StrikerRadius, checkTransform.forward, out hit, CutOffLength, colliderMask);

        // Clamp distance so it never exceeds CutOffLength
        float targetDistance = hasHit ? Mathf.Min(hit.distance, CutOffLength) : CutOffLength;
        float hitDistance = targetDistance / distanceFactor;

        if (hitDistance < 1)
        {
            line.SetActive(false);
            quadCircle.SetActive(false);
            return;
        }

        line.SetActive(true);
        quadCircle.SetActive(true);

        Vector3 circleCenter;

        if (hasHit && hit.distance <= CutOffLength)
        {
            // Position circle directly at the impact surface offset
            circleCenter = hit.point + hit.normal * (boardData.StrikerRadius - strikerOffset);
        }
        else
        {
            // Position circle at the maximum allowed range along the aim vector
            circleCenter = checkTransform.position + checkTransform.forward * CutOffLength;
        }

        quadCircle.transform.position = circleCenter;

        // Scale line to span precisely from its origin to the circle center
        float exactDistance = Vector3.Distance(line.transform.position, circleCenter);
        line.transform.localScale = new Vector3(LineThickness, exactDistance, LineThickness);
    }
}
using com.VisionXR.ModelClasses;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    [Header("References")]
    public BoardDataSO boardData;
    public GameObject line;
    public Renderer lineRenderer;

    // local variables
    private RaycastHit hit;
    private float CutOffLength = 1f;

    // Added variable for line thickness
    public  float LineThickness = 0.08f;

    public void SetCutOffLength(float d)
    {
        CutOffLength = d;
    }

    public void SetColor(Color color)
    {
        if (lineRenderer != null && lineRenderer.material != null)
        {
            lineRenderer.material.color = color;
        }
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.up, out hit))
        {
            Vector3 hitPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            float d = Vector3.Distance(hit.point, transform.position);
            d = d / 0.15f;
            float scaleY = d > CutOffLength ? CutOffLength : d;
            line.transform.localScale = new Vector3(LineThickness, scaleY, 1);

            // Set tiling for lineRenderer's material
            if (lineRenderer != null && lineRenderer.material != null)
            {
                lineRenderer.material.mainTextureScale = new Vector2(1, scaleY*10);
            }
        }
        else
        {
            line.transform.localScale = new Vector3(LineThickness, CutOffLength, 1);

            // Set tiling for lineRenderer's material
            if (lineRenderer != null && lineRenderer.material != null)
            {
                lineRenderer.material.mainTextureScale = new Vector2(1, CutOffLength*10);
            }
        }
    }
}

using UnityEngine;

public class LoadingImage : MonoBehaviour
{
    [Header("Rotation")]
    [Tooltip("Rotation speed in degrees per second")]
    public float rotationSpeed = 180f;

    [Tooltip("If true rotates clockwise, otherwise counter-clockwise")]
    public bool clockwise = true;

    // cached transform for a small perf win
    Transform cachedTransform;

    void Awake()
    {
        cachedTransform = transform;
    }

    void Update()
    {
        float dir = clockwise ? -1f : 1f; // negative z rotates clockwise visually for UI
        cachedTransform.Rotate(0f, 0f, dir * rotationSpeed * Time.deltaTime);
    }

    // Optional runtime control
    public void SetSpeed(float degPerSec)
    {
        rotationSpeed = degPerSec;
    }

    public void SetClockwise(bool cw)
    {
        clockwise = cw;
    }
}

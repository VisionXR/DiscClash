using System.Collections;
using UnityEngine;

public class HeadMovementController : MonoBehaviour
{
    public float minHeadInterval = 5.0f;
    public float maxHeadInterval = 10.0f;
    public float maxHeadRotationX = 10.0f;
    public float maxHeadRotationY = 5.0f;

    private Quaternion originalRotation;

    private void Start()
    {
        originalRotation = transform.localRotation;
        StartCoroutine(StartHeadMovement());
    }

    private IEnumerator StartHeadMovement()
    {
        while (true)
        {
            float headInterval = Random.Range(minHeadInterval, maxHeadInterval);
            float headRotationX = Random.Range(-maxHeadRotationX, maxHeadRotationX);
            float headRotationY = Random.Range(-maxHeadRotationY, maxHeadRotationY);
            yield return new WaitForSeconds(headInterval);
            StartCoroutine(AnimateHeadMovement(headRotationX, headRotationY));
        }
    }

    private IEnumerator AnimateHeadMovement(float targetRotationX, float targetRotationY)
    {
        float time = 0.0f;
        float duration = 0.5f; // The time it takes for the head to complete the movement
        Quaternion targetQuaternion = originalRotation * Quaternion.Euler(targetRotationX, targetRotationY, 0);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.localRotation = Quaternion.Slerp(originalRotation, targetQuaternion, t);
            yield return null;
        }

        transform.localRotation = targetQuaternion;

        // Return the head to its original position
        time = 0.0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.localRotation = Quaternion.Slerp(targetQuaternion, originalRotation, t);
            yield return null;
        }

        transform.localRotation = originalRotation;
    }
}

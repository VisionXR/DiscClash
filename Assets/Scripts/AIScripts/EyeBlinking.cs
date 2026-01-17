using System.Collections;
using UnityEngine;

public class EyeBlinking : MonoBehaviour
{
    private Coroutine blinkRoutine;

    // Min and Max wait time between blinks, editable in Inspector
    [SerializeField] private float minBlinkInterval = 1f;
    [SerializeField] private float maxBlinkInterval = 5f;
    [SerializeField] private float blinkDuration = 0.2f; // Controls how fast the blink happens

    void OnEnable()
    {
        blinkRoutine = StartCoroutine(StartBlinking());
    }

    private void OnDisable()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);
    }

    private IEnumerator StartBlinking()
    {
        while (true)
        {
            // Wait for a random interval between blinks
            float waitTime = Random.Range(minBlinkInterval, maxBlinkInterval);
            yield return new WaitForSeconds(waitTime);

            // Smoothly scale down to simulate blinking
            yield return StartCoroutine(ScaleOverTime(Vector3.one, new Vector3(0.2f, 0.2f, 0.2f), blinkDuration));

            // Pause briefly before "opening" the eyes
            yield return new WaitForSeconds(blinkDuration);

            // Smoothly scale back to the original size
            yield return StartCoroutine(ScaleOverTime(new Vector3(0.2f, 0.2f, 0.2f), Vector3.one, blinkDuration));
        }
    }

    private IEnumerator ScaleOverTime(Vector3 startScale, Vector3 endScale, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endScale;
    }
}


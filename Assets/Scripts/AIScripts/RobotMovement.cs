using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    public float upDownDistance = 1.0f; // The distance the robot will move up and down
    public float upDownDuration = 2.0f; // The duration of each up and down movement

    private Vector3 originalPosition;
    private bool movingUp = true;

    private void Start()
    {
        // Save the original position of the robot
        originalPosition = transform.position;

        // Start the continuous up and down movement
        StartUpDownAnimation();
    }

    private void StartUpDownAnimation()
    {
        // Calculate the target position for the next movement (up or down)
        Vector3 targetPosition = movingUp ? originalPosition + Vector3.up * upDownDistance : originalPosition;

        // Define the LeanTween animation
        LeanTween.move(gameObject, targetPosition, upDownDuration)
            .setEase(LeanTweenType.easeInOutQuad) // Use easeInOutQuad for smooth up and down movement
            .setOnComplete(OnUpDownComplete);
    }

    private void OnUpDownComplete()
    {
        // Toggle the movingUp flag to switch between up and down movement
        movingUp = !movingUp;

        // Start the next up and down animation
        StartUpDownAnimation();
    }
}

using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "TutorialStep", menuName = "RealCarrom/TutorialStep", order = 0)]
public class TutorialStep : ScriptableObject
{
    public int stepNumber;           // The step's number
    public string stepText;          // The description for the step
    public VideoClip stepVideo;      // The video clip associated with the step
    public AudioClip stepAudio;      // The audio clip associated with the step
    public InteractiveStepType interactiveStepType;

    // Success and failure feedback
    public string successText;       // Text displayed on successful completion of the step
    public string failureText;       // Text displayed on failure of the step
    public AudioClip successAudio;   // Audio clip played on successful completion
    public AudioClip failureAudio;   // Audio clip played on failure

    // Data for Positioning Step:
    public Vector3 strikerPosition; // Desired position for the striker during the positioning step.

    // Data for Aiming Step:
    public Vector3 aimingStrikerPosition; // Starting position for the striker during the aiming step.
    public Vector3 aimingStrikerRotation; // Desired rotation for the striker during the aiming step.

    // Data for Striking Step:
    public Vector3 strikingStrikerPosition; // Starting position for the striker for the striking step.
    public Vector3 strikingStrikerRotation; // Starting rotation for the striker during the striking step.
    public Vector3 coinPosition; // Position where the coin should be placed for the striking step.
    public string holeName;
}

public enum InteractiveStepType
{
    None,        // For automated steps
    Positioning,
    Aiming,
    Striking,
    Display
}

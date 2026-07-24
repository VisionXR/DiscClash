using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "TutorialStep", menuName = "RealCarrom/TutorialStep", order = 0)]
public class TutorialStep : ScriptableObject
{
    public int stepNumber;           // The step's number
    //this text should be display as line by line inspector
    [TextArea(3, 10)]
    public string stepText;          // The description for the step

    public AudioClip stepAudio;      // The audio clip associated with the step
    public InteractiveStepType interactiveStepType;
    

    // Success and failure feedback

    [TextArea(2, 6)]
    public string successText;       // Text displayed on successful completion

    [TextArea(2, 6)]
    public string failureText;       // Text displayed on failure

    public AudioClip successAudio;   // Audio clip played on successful completion
    public AudioClip failureAudio;   // Audio clip played on failure


    // Data for Aiming Step:
    public Vector3 aimingStrikerRotation; // Desired rotation for the striker during the aiming step.

    // Data for Striking Step:
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
    Foul
}

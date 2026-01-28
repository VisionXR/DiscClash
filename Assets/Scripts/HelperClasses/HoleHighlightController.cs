using UnityEngine;

public class HoleHighlightController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string strikerTag = "Striker";
    [SerializeField] private string blackTag = "Black";
    [SerializeField] private string whiteTag = "White";
    [SerializeField] private string redTag = "Red";

    // This allows the highlight to turn itself off after being triggered
    [SerializeField] private float displayDuration = 1.5f;

    [Header("References")]
    // Drag the visual part of the highlight here (the glow mesh)
    [SerializeField] private GameObject highlightVisual;

    private void Awake()
    {
         highlightVisual.SetActive(false);
    }

    // Since you are dragging the Trigger onto the Highlight script, 
    // the Highlight object must also have a Trigger Collider on it 
    // OR this script should be on the Hole Trigger itself.
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided"+other.gameObject.name);
        if (other.CompareTag(strikerTag) || other.CompareTag(blackTag) || other.CompareTag(whiteTag)  || other.CompareTag(redTag))
        {
            Debug.Log("Inside the collidion"+other.gameObject.name);
            ShowGlow();
        }
    }

    private void ShowGlow()
    {
        CancelInvoke(nameof(HideGlow)); // Reset timer if another coin hits
        highlightVisual.SetActive(true);
        Invoke(nameof(HideGlow), displayDuration);
    }

    private void HideGlow()
    {
        highlightVisual.SetActive(false);
    }
}

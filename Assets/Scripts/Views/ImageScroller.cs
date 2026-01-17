using UnityEngine;


public class ImageScroller : MonoBehaviour
{
    public RectTransform imageTransform;
    public float scrollDuration = 0.1f;
    public float scrollDistance = 200f;
    public LeanTweenType scrollEaseType = LeanTweenType.linear;
    public bool isScrolling = false;

    public void StartScrolling()
    {
        if(isScrolling)
        {
            return;
        }

        // Calculate the target position for scrolling
        Vector3 targetPosition = imageTransform.anchoredPosition3D + new Vector3(0f, scrollDistance, 0f);

        // Set the initial position below the screen
        imageTransform.anchoredPosition3D -= new Vector3(0f, scrollDistance, 0f);

        // Start the scrolling animation if the player has not joined yet
        isScrolling = true;
        if (isScrolling)
        {
            LeanTween.moveLocalY(gameObject, targetPosition.y, scrollDuration)
                .setEase(scrollEaseType)
                .setLoopClamp()
                .setOnComplete(ResetPosition);
        }
    }

    private void ResetPosition()
    {
        // Reset the position to the starting point
        imageTransform.anchoredPosition3D -= new Vector3(0f, scrollDistance, 0f);

        // Calculate the target position for scrolling
        Vector3 targetPosition = imageTransform.anchoredPosition3D + new Vector3(0f, scrollDistance, 0f);

        // Start the scrolling animation again if the player has not joined yet
        if (isScrolling)
        {
            LeanTween.moveLocalY(gameObject, targetPosition.y, scrollDuration)
                .setEase(scrollEaseType)
                .setLoopClamp()
                .setOnComplete(ResetPosition);
        }
    }

    public void StopScrolling()
    {
        if (isScrolling)
        {           
           // Stop the scrolling animation when the player joins
            isScrolling = false;
            LeanTween.cancel(imageTransform);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        }
        
    }
}

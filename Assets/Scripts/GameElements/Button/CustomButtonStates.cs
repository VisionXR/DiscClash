using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButtonStates : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private bool isHovering = false;
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private Image HoverImage;
    [SerializeField] private Image ToolTipImage;

    [Header(" Colors ")]
    public Color SelectedColor;
    public Color HoverColor;
    public Color IdleColor;
    public Color HoverIdle;

    void OnEnable()
    {
        Invoke("SetHover", 0.1f);
    }

    void SetHover()
    {
        HoverImage.color = HoverIdle;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        if (BackgroundImage.gameObject.GetComponent<UIGradient>().enabled == false)
        {
            isHovering = true;
            HoverImage.color = HoverColor;
            AppProperties.instance.PlayHapticVibration();

        }
        // Perform desired actions when the pointer enters the button

        if (ToolTipImage != null)
        {
            ToolTipImage.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       // Debug.Log(" exit id is " + eventData.);
        if (BackgroundImage.gameObject.GetComponent<UIGradient>().enabled == false)
        {
            isHovering = false;
            HoverImage.color = HoverIdle;

        }
        // Perform desired actions when the pointer exits the button

        if (ToolTipImage != null)
        {
            ToolTipImage.gameObject.SetActive(false);
        }

        AppProperties.instance.StopVibration();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isHovering)
        {
            Debug.Log("Pointer clicked");
            HoverImage.color = HoverIdle;
            // Perform desired actions when the pointer clicks the button
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       
    }

    public void OnDrag(PointerEventData eventData)
    {
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
    }
}

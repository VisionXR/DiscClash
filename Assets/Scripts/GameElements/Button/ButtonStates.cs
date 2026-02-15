using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonStates : MonoBehaviour, IPointerEnterHandler,
IPointerExitHandler, IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{

    [Header(" Scriptable Objects ")]
    public AppDataSO appData;

    [Header(" Images ")]
    public Image BackgroundImage;
    public Image HoverImage;
    

    // local variables
    private bool isHovering = false;

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (BackgroundImage.gameObject.GetComponent<UIGradient>().enabled == false)
        {
            isHovering = true;
            HoverImage.color = appData.HoverColor;
            appData.PlayButtonVibration();

        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
       // Debug.Log(" exit id is " + eventData.);
        if (BackgroundImage.gameObject.GetComponent<UIGradient>().enabled == false)
        {
            isHovering = false;
            HoverImage.color = appData.IdleColor;
           
        }
     
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isHovering)
        {
           
          
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

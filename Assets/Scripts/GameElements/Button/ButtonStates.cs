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

        //if (BackgroundImage.color == appData.IdleColor)
        //{
        //    isHovering = true;
        //    HoverImage.gameObject.SetActive(true);
        //    appData.PlayButtonVibration();

        //}

    }

    public void OnPointerExit(PointerEventData eventData)
    {
       //// Debug.Log(" exit id is " + eventData.);
       // if (BackgroundImage.color == appData.IdleColor)
       // {
       //     isHovering = false;
       //     HoverImage.gameObject.SetActive(false);

       // }
     
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isHovering)
        {
            AudioManager.instance.PlayButtonClickSound();
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

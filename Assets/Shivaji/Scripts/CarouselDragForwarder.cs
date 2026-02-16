using UnityEngine;
using UnityEngine.EventSystems;

public class CarouselDragForwarder : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private CarouselController carousel;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (carousel != null)
            carousel.OnBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (carousel != null)
            carousel.OnEndDrag(eventData);
    }
}

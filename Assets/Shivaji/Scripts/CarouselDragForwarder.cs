using UnityEngine;
using UnityEngine.EventSystems;

public class CarouselDragForwarder : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public CarouselController carousel;

    public void OnBeginDrag(PointerEventData eventData) => carousel.OnBeginDrag();
    public void OnEndDrag(PointerEventData eventData) => carousel.OnEndDrag();
}

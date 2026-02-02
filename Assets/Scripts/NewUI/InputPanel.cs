using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class InputPanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Slider strikerPosSlider;
    private RectTransform _rectTransform;
    private bool _isPointerDown;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        // center by default
        if (strikerPosSlider != null)
            strikerPosSlider.value = 0.5f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
        UpdateValueFromPointer(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isPointerDown) return;
        UpdateValueFromPointer(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
    }

    private void UpdateValueFromPointer(PointerEventData eventData)
    {
        if (strikerPosSlider == null || _rectTransform == null) return;

        // Use the event's camera (can be null for ScreenSpace-Overlay canvases)
        Camera eventCam = eventData.pressEventCamera;

        Vector2 localPoint;
        bool ok = RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, eventCam, out localPoint);
        if (!ok)
        {
           
            return;
        }

        float width = _rectTransform.rect.width;

        // localPoint.x is relative to pivot. Convert to 0..1 across width:
        float normalized = Mathf.Clamp01((localPoint.x / width) + 0.5f);
        strikerPosSlider.value = normalized;
    }
}

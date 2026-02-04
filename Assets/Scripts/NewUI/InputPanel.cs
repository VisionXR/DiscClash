using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class InputPanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("UI Elements")]
    public StrikerDataSO strikerData;
    public Slider strikerPosSlider;
    public Image sliderImage;

    [Header("Animation Settings")]
    [SerializeField] private float leftLimit = -275f;
    [SerializeField] private float rightLimit = 275f;
    [SerializeField] private float animationDuration = 1.5f; // Time for one full back-and-forth
    [SerializeField] private float repeatDelay = 5.0f;       // Wait time between animations

    private RectTransform _rectTransform;
    private RectTransform _sliderImageRect;
    private bool _isPointerDown;
    private Coroutine _hintCoroutine;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (sliderImage != null)
        {
            _sliderImageRect = sliderImage.GetComponent<RectTransform>();
            sliderImage.gameObject.SetActive(false); // Start hidden
        }
    }

    private void OnEnable()
    {
        if (strikerPosSlider != null)
            strikerPosSlider.value = 0.5f;

        // Start the hint routine
        if (sliderImage != null)
        {
            _hintCoroutine = StartCoroutine(SliderHintRoutine());
        }
    }

    private void OnDisable()
    {
        // Stop the routine to prevent errors when the panel is hidden
        if (_hintCoroutine != null)
        {
            StopCoroutine(_hintCoroutine);
            _hintCoroutine = null;
        }
    }

    // --- Animation Logic ---

    private IEnumerator SliderHintRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(repeatDelay);

            // Don't show hint if the player is currently touching the screen
            if (!strikerData.isMoving && !strikerData.isAimimg)
            {
                yield return StartCoroutine(AnimateSliderImage());
            }
        }
    }

    private IEnumerator AnimateSliderImage()
    {
        sliderImage.gameObject.SetActive(true);
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            // Calculate a 0 to 1 to 0 value using Sine for smooth ping-pong
            // Or use Mathf.PingPong for a more linear movement
            float t = Mathf.PingPong(elapsed / (animationDuration * 0.5f), 1f);

            // Smooth step the t value for nicer ease-in/out
            t = Mathf.SmoothStep(0, 1, t);

            float targetX = Mathf.Lerp(leftLimit, rightLimit, t);
            _sliderImageRect.anchoredPosition = new Vector2(targetX, _sliderImageRect.anchoredPosition.y);

            yield return null;
        }

        sliderImage.gameObject.SetActive(false);
    }

    // --- Input Handling (Original Logic) ---

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
        strikerData.isMoving = true;
        // Hide hint immediately if player starts interacting
        sliderImage.gameObject.SetActive(false);
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
        strikerData.isMoving = false;
    }

    private void UpdateValueFromPointer(PointerEventData eventData)
    {
        if (strikerPosSlider == null || _rectTransform == null) return;

        Camera eventCam = eventData.pressEventCamera;
        Vector2 localPoint;
        bool ok = RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, eventCam, out localPoint);

        if (!ok) return;

        float width = _rectTransform.rect.width;
        float normalized = Mathf.Clamp01((localPoint.x / width) + 0.5f);
        strikerPosSlider.value = normalized;
    }
}
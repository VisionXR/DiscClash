using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using com.VisionXR.ModelClasses;
using com.VisionXR.GameElements;

[RequireComponent(typeof(Slider))]
public class AimSlider : MonoBehaviour, IDragHandler,IPointerDownHandler
{
    [Header("Scriptable Objects")]
    public InputDataSO inputData;
//    public PlayerDataSO playerData;
    public GameObject aimArrow;

    // local
    private RectTransform sliderRectTransform;
    private Player mp;
    private float lastAngle = 0f;

    private void Awake()
    {

        sliderRectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (aimArrow != null)
            aimArrow.transform.rotation = Quaternion.identity;

     //   mp = playerData.GetMainPlayer();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Call OnDrag immediately to update the aim direction on pointer down
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 1. Get the center of the slider in Screen Space
        // Ensure your sliderRectTransform pivot is (0.5, 0.5) for this to be the visual center
        Vector2 sliderCenterScreen = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, sliderRectTransform.position);

        // 2. Vector from center to mouse
        Vector2 direction = eventData.position - sliderCenterScreen;

        if (direction.magnitude > 0.025f)
        {
            // 3. Use SignedAngle for smooth continuous rotation without wrapping discontinuities
            // Calculate angle from right (0,1) vector to maintain consistency
            float angle = Vector2.SignedAngle(Vector2.right, direction);

            if (mp != null)
            {
                // Normalize angle difference to handle smooth wrapping across -180/180 boundary
                float angleDiff = Mathf.DeltaAngle(lastAngle, angle);
                lastAngle += angleDiff;

                // If your arrow graphic points UP in the sprite file, use: angle - 90
                // If your arrow graphic points RIGHT in the sprite file, use: angle
                float visualAngle = lastAngle - 90f;

                //if (mp.playerProperties.myId == 2)
                //{
                //    aimArrow.transform.rotation = Quaternion.Euler(0, 0, visualAngle);
                //    // Adjusting the absolute rotation for the striker logic
                //    inputData.RotateStrikerAbsolute(-(visualAngle + 180));
                //}
                //else
                //{
                //    aimArrow.transform.rotation = Quaternion.Euler(0, 0, visualAngle);
                //    inputData.RotateStrikerAbsolute(-visualAngle);
                //}
            }
        }
    }


}
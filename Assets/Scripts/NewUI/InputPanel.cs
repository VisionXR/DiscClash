using UnityEngine;
using UnityEngine.UI;

public class InputPanel : MonoBehaviour
{

    public Slider strikerPosSlider;

    private void OnEnable()
    {
        strikerPosSlider.value = 0.5f;
    }
}

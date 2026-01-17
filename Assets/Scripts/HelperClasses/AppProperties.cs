using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;

public class AppProperties : MonoBehaviour
{
    public static AppProperties instance;
    public MyPlayerSettings playerSettings;
   

    [Header(" Colors ")]
    public Color SelectedColor;
    public Color HoverColor;
    public Color IdleColor;
    public Color HoverIdle;

    [Header(" Static Icons")]
    public Sprite AIIcon;
    public Sprite DummyPersonIcon;

    [Header(" Local variables")]
    public float vibrationDuration = 0.5f;
    public float vibrationAmplitude = 0.1f;
    public float vibrationAmplitudeForStriking = 1f;


    private bool isLeft, isRight;


    private void Awake()
    {
        instance = this;
    }

    public void PlayVibration()
    {
        StartCoroutine(PlayVibrationCoroutine());
    }

    public void PlayHapticVibration()
    {
        StartCoroutine(PlayHapticVibrationCoroutine());
    }

    // Summary: Start haptic vibration for a given duration
    public IEnumerator PlayHapticVibrationCoroutine()
    {
      
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + vibrationDuration)
        {
            yield return null;
        }

        StopVibration();
    }

    public IEnumerator PlayVibrationCoroutine()
    {

        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + vibrationDuration)
        {
            yield return null;
        }

        StopVibration();
    }

    public void StopVibration()
    {
        
    }


    public void LeftHandHovered()
    {
       
        isLeft = true;
    }

    public void RightHandHovered()
    {
        
        isRight = true;
    }

    public void LeftHandUnHovered()
    {
        isLeft = false;
    }

    public void RightHandUnUnHovered()
    {
        isRight = false;
    }
}

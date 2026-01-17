using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

public class HandChange : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public MyPlayerSettings myPlayerSettings;

    [Header("References")]
    public GameObject rightHandInteraction;
    public GameObject leftHandInteraction;

    private void OnEnable()
    {
        myPlayerSettings.DominantHandChanged += OnDominantHandChanged;
        OnDominantHandChanged(DominantHand.BOTH);
    }

    private void OnDisable()
    {
        myPlayerSettings.DominantHandChanged -= OnDominantHandChanged;
    }

    private void OnDominantHandChanged(DominantHand hand)
    {
        if(hand == DominantHand.RIGHT)
        {
            leftHandInteraction.SetActive(true);
            rightHandInteraction.SetActive(false);
        }
        else if(hand == DominantHand.LEFT)
        {
            leftHandInteraction.SetActive(false);
            rightHandInteraction.SetActive(true);
        }
        else
        {
            leftHandInteraction.SetActive(true);
            rightHandInteraction.SetActive(true);
        }
    }

 
}

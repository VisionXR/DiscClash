using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    public GameObject startPose;  // Assign your start pose hand in the Unity Editor.
    public GameObject endPose;    // Assign your end pose hand in the Unity Editor.
    public float lerpDuration = 1.0f; // Duration to complete the lerp.
    private float lerpProgress = 0.0f; // Track the progress of the lerp.

    public void StartLerp()
    {
        lerpProgress = 0.0f;
        StartCoroutine(LerpHandPoses());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            StartLerp();
            
        }
    }

    private IEnumerator LerpHandPoses()
    {
        Debug.Log(" Starting");
        int childCount = startPose.transform.childCount;

        // Ensure both have the same number of children.
        if (childCount != endPose.transform.childCount)
        {
            Debug.LogError("StartPose and EndPose do not have the same number of children!");
            yield break;
        }

        while (lerpProgress < 1.0f)
        {
            lerpProgress += Time.deltaTime / lerpDuration;
            for (int i = 0; i < childCount; i++)
            {
                Transform startChild = startPose.transform.GetChild(i);
                Transform endChild = endPose.transform.GetChild(i);

                // Interpolate position and rotation for the corresponding child transforms.
                startChild.position = Vector3.Lerp(startChild.position, endChild.position, lerpProgress);
                startChild.rotation = Quaternion.Lerp(startChild.rotation, endChild.rotation, lerpProgress);
            }

            yield return null; // Wait for the next frame.
        }
    }

}

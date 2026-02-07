using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{

    public TMP_Text roomName;
    private Coroutine loadingRountine;


    private void OnEnable()
    {
        if(loadingRountine == null)
        {
            loadingRountine = StartCoroutine(ConnectingToRoom());
        }
    }

    private void OnDisable()
    {
        if (loadingRountine != null)
        {
            StopCoroutine(loadingRountine);
            loadingRountine = null;
        }
    }

    private IEnumerator ConnectingToRoom()
    {
        while (true)
        {
            roomName.text = ".";
            yield return new WaitForSeconds(0.5f);
            roomName.text = "..";
            yield return new WaitForSeconds(0.5f);
            roomName.text = "...";
            yield return new WaitForSeconds(0.5f);
            roomName.text = "....";
            yield return new WaitForSeconds(0.5f);
            roomName.text = ".....";
            yield return new WaitForSeconds(0.5f);
            roomName.text = "......";
        }
    }
}

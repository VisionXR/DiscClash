using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;

public class SinglePlayerConnectionManager : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public PlayersDataSO playersData;
    public UIOutputDataSO uiOutputData;
    public UIInputDataSO uiInputData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnEnable()       
    {
        StartCoroutine(ShowPlayers());
    }

    private IEnumerator ShowPlayers()
    {
        yield return new WaitForSeconds(1);
        foreach (Player p in playersData.CurrentPlayers)
        {
            uiInputData.ShowPlayerDetails(p);
        }
    }

}

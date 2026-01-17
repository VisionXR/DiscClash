using System.Collections.Generic;
using UnityEngine;


public class ColliderManager : MonoBehaviour
{
    public static ColliderManager instance;
    public List<GameObject> holeTrigger;
    public GameObject Ground;

    void Awake()
    {
        instance = this;
    }

    public void TurnOnColliders()
    {
        foreach(GameObject hole in holeTrigger)
        {
            hole.SetActive(true);
        }
        Ground.SetActive(true);
    }

    public void TurnOffColliders()
    {
        foreach (GameObject hole in holeTrigger)
        {
            hole.SetActive(false);
        }
        Ground.SetActive(false);
    }
}


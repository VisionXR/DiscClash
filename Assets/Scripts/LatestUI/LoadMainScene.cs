using System.Collections;
using UnityEngine;

public class LoadMainScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
    }

  
}

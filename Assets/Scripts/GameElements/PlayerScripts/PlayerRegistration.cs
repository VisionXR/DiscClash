using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class PlayerRegistration : MonoBehaviour
{
    public PlayersDataSO playerData;

    private void OnEnable()
    {
        playerData.AddPlayer(GetComponent<Player>());
    }

    private void OnDisable()
    {
        playerData.RemovePlayer(GetComponent<Player>());
    }
}

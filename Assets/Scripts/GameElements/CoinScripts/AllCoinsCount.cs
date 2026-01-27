using com.VisionXR.ModelClasses;
using UnityEngine;

public class AllCoinsCount : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public GameDataSO gameData;
    public BoardDataSO boardData;

    [Header(" Local variables")]
    public int TotalCoins;
    public int TotalWhites;
    public int TotalBlacks;
    public int TotalReds;

    private void OnEnable()
    {
        transform.position = boardData.GetAllCoinsTransform().position;
        transform.rotation = boardData.GetAllCoinsTransform().rotation;
        gameData.SetData(TotalCoins, TotalWhites, TotalBlacks, TotalReds);
    }
}

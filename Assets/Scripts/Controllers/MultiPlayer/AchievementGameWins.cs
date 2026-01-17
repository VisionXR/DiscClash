using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class AchievementGameWins : MonoBehaviour
{
   
    [Header("Scriptable Objects")]
    public UIOutputDataSO uiOutputData;
    public AchievementsDataSO achievementsData;

    private void OnEnable()
    {
        achievementsData.SinglePlayerGameWonEvent += AddSinglePlayerWins;
        achievementsData.MultiPlayerGameWonEvent += AddMultiPlayerWins;
    }

    private void OnDisable()
    {
        achievementsData.SinglePlayerGameWonEvent -= AddSinglePlayerWins;
        achievementsData.MultiPlayerGameWonEvent -= AddMultiPlayerWins;
    }

    private void AddSinglePlayerWins()
    {
      
    }

    public void AddMultiPlayerWins()
    {
       
    }


}

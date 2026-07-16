using TMPro;
using UnityEngine;

public class LeaderBoardItem : MonoBehaviour
{
    public TMP_Text NameText;
    public TMP_Text RankText;
    public TMP_Text PointsText;


    public void SetLeaderBoardData(string name, int rank, int points)
    {
        NameText.text = name;
        RankText.text = rank.ToString();
        PointsText.text = points.ToString();
    }
}

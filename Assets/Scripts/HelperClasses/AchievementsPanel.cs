using com.VisionXR.ModelClasses;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementsPanel : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public AchievementsDataSO achievementData;

    [Header("UI Objects")]
    public List<AchievementUI> achievementUI;
    void OnEnable()
    {
        Initialise();
        achievementData.UnlockAchievementEvent += UnLocked;
    }

    private void OnDisable()
    {
        achievementData.UnlockAchievementEvent -= UnLocked;
    }

    private void UnLocked(string name)
    {
        Initialise();
    }
    public void Initialise()
    {
        for (int i = 0; i < achievementData.AllAchievementInfo.Count; i++)
        {
            achievementUI[i].NameText.text = achievementData.AllAchievementInfo[i].achievementName;
            achievementUI[i].DescriptionText.text = achievementData.AllAchievementInfo[i].description;

            if (achievementData.AllAchievementInfo[i].isAchieved)
            {
                achievementUI[i].unLockedObject.SetActive(true);
                achievementUI[i].lockedObject.SetActive(false);              
            }
            else
            {
                achievementUI[i].lockedObject.SetActive(true);
                achievementUI[i].unLockedObject.SetActive(false);
            }
        }
    }

    public void BackBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        gameObject.SetActive(false);
    }

    public void CrossBtnClicked()
    {
        AudioManager.instance.PlayButtonClickSound();
        gameObject.SetActive(false);
    }
}

[Serializable]
public class AchievementUI
{
    public TMP_Text NameText;
    public TMP_Text DescriptionText;
    public GameObject lockedObject;
    public GameObject unLockedObject;

}

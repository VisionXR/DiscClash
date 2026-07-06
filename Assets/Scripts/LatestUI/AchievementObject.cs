using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class AchievementObject : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public AchievementsDataSO achievementsData;
        public AchievementInfo achievementInfo;

        [Header("UI Elements")]

        public TMP_Text id;
        public Image Icon;
        public TMP_Text title;
        public TMP_Text description;

        [Header("Achivement Types")]

        public GameObject simpleObject;
        public GameObject LockedObject;
        public GameObject UnLockedObject;

        public GameObject progressObject;
        public Slider progressSlider;
        public TMP_Text progressText;


        //public void SetAchivementInfo(AchievementInfo achievementInfo)
        //{
        //    this.achievementInfo = achievementInfo;
        //    id.text = achievementInfo.id.ToString();
        //    Icon.sprite = achievementInfo.icon;
        //    title.text = achievementInfo.achievementName;
        //    description.text = achievementInfo.description;
          

        //    if (achievementInfo.achievementType == AchievementType.Simple)
        //    {
        //        simpleObject.SetActive(true);
        //        progressObject.SetActive(false);
        //        if (achievementInfo.isAchieved)
        //        {
        //            UnLockedObject.SetActive(true);
        //            LockedObject.SetActive(false);
        //        }
        //        else
        //        {
        //            UnLockedObject.SetActive(false);
        //            LockedObject.SetActive(true);
        //        }
        //    }
        //    else
        //    {
        //        simpleObject.SetActive(false);
        //        progressObject.SetActive(true);

        //        if (achievementInfo.actual >= achievementInfo.target)
        //        {
        //            achievementInfo.actual = achievementInfo.target;
        //        }

        //        progressSlider.maxValue = achievementInfo.target;
        //        progressSlider.value = achievementInfo.actual;
        //        progressText.text = $"{achievementInfo.actual}/{achievementInfo.target}";
        //    }
        //}
    }
}

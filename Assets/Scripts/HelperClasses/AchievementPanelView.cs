using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class AchievementsPanelView : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public AchievementsDataSO achievementData;
        public UIDataSO uiData;

        [Header("Tab Objects")]
        public List<GameObject> SelectionImages;
        public List<GameObject> TabPanels;

        [Header("Panel Objects")]
        public string currentState;


        [Header("UI Objects")]
        public GameObject achievementObjectPrefab;
        public Transform generalContentTransform;
        public Transform singlePlayerContentTransform;
        public Transform multiPlayerContentTransform;



        void OnEnable()
        {

            Show();
            achievementData.GotAllAchievementsEvent += Show;
        }

        private void OnDisable()
        {
            achievementData.GotAllAchievementsEvent -= Show;
        }

        private void Show()
        {
            StartCoroutine(Initialise());
        }

        public void TabButtonClicked(int id)
        {
            AudioManager.instance.PlayButtonClickSound();
            ResetTabs();
            TabPanels[id].SetActive(true);
            SelectionImages[id].SetActive(true);
        }

        private void ResetTabs()
        {
            foreach (var tab in TabPanels)
            {
                tab.SetActive(false);
            }

            foreach (var img in SelectionImages)
            {
                img.SetActive(false);
            }
        }

        public IEnumerator Initialise()
        {

            Clear();
            yield return new WaitForSeconds(0.5f);
            int generalCount = 0;
            int singlePlayerCount = 0;
            int multiPlayerCount = 0;
            var list = achievementData.AllAchievementInfo;
            for (int i = 0; i < list.Count; i++)
            {

                var info = list[i];
                GameObject go;
                if (info.achievementSection == AchievementSection.General)
                {
                    go = Instantiate(achievementObjectPrefab, generalContentTransform);
                    AchievementObject ao = go.GetComponent<AchievementObject>();
                    info.id = (generalCount + 1);
                    ao.SetAchivementInfo(info);
                    generalCount++;
                }
                else if (info.achievementSection == AchievementSection.SinglePlayer)
                {
                    go = Instantiate(achievementObjectPrefab, singlePlayerContentTransform);
                    AchievementObject ao = go.GetComponent<AchievementObject>();
                    info.id = (singlePlayerCount + 1);
                    ao.SetAchivementInfo(info);
                    singlePlayerCount++;
                }
                else
                {
                    go = Instantiate(achievementObjectPrefab, multiPlayerContentTransform);
                    AchievementObject ao = go.GetComponent<AchievementObject>();
                    info.id = (multiPlayerCount + 1);
                    ao.SetAchivementInfo(info);
                    multiPlayerCount++;
                }

            }
        }

        private void Clear()
        {
            // Clear existing entries
            for (int i = generalContentTransform.childCount - 1; i >= 0; i--)
            {
                var child = generalContentTransform.GetChild(i).gameObject;
                if (child != null)
                {
                    Destroy(child);
                }
            }

            for (int i = singlePlayerContentTransform.childCount - 1; i >= 0; i--)
            {
                var child = singlePlayerContentTransform.GetChild(i).gameObject;
                if (child != null)
                {
                    Destroy(child);
                }
            }

            for (int i = multiPlayerContentTransform.childCount - 1; i >= 0; i--)
            {
                var child = multiPlayerContentTransform.GetChild(i).gameObject;
                if (child != null)
                {
                    Destroy(child);
                }
            }
        }

        public void RefreshBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            achievementData.GetAllAchievements();
        }

        public void BackBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            uiData.uiManager.ChangeState(currentState, false);
        }
    }
}



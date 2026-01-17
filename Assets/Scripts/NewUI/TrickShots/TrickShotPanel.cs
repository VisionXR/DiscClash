using com.VisionXR.ModelClasses;
using UnityEngine;

public class TrickShotPanel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public TrickShotsDataSO trickShotData;
    public UIOutputDataSO uiOutputData;

    [Header("Panels")]
    public GameObject LevelPanel;
    public GameObject InGamePanel;
    public GameObject ResultPanel;

    [Header("Level UI")]
    public Transform levelsContainer;   // parent of all level buttons

    [Header("Countdown")]
    public CountdownUI countdown;       // assign the overlay object here
    public int countdownFrom = 3;

    private bool isLoading;

    private void OnEnable()
    {
       
        trickShotData.InitOrLoadProgress();
        ApplyLocks();
        trickShotData.LevelSuccessEvent += OnLevelSuccess;
    }
    private void Start()
    {
   
        LevelPanel.SetActive(true);
        InGamePanel.SetActive(false);
        ResultPanel.SetActive(false);

        isLoading = false;
    }

    private void OnDisable()
    {
        trickShotData.LevelSuccessEvent -= OnLevelSuccess;
        trickShotData.SaveProgress();
    }

    private void OnLevelSuccess(int _stars)
    {
        ApplyLocks(); // unlock next level immediately when returning
    }

    public void OnLevelBtnClicked(int levelNo)
    { 

        Debug.Log($"[TrickShotPanel] OnLevelBtnClicked({levelNo})");
        AudioManager.instance.PlayButtonClickSound();

        if (!trickShotData.IsUnlocked(levelNo))
        {
            Debug.Log("Level is locked.");
            return;
        }
        ResetPanels();

        // StartCoroutine(LoadAfterCountdown(levelNo));
        trickShotData.LoadTrickShotLevel(levelNo);
        
        //InGamePanel.SetActive(true);
    }


    public void QuitBtnclciked()
    {
        AudioManager.instance.PlayButtonClickSound();
        uiOutputData.StopTrickShotsEvent();
        ResetPanels();
        uiOutputData.HomeEvent?.Invoke();
    }

    private void ResetPanels()
    {
        LevelPanel.SetActive(false);
        InGamePanel.SetActive(false);
        ResultPanel.SetActive(false);
    }


    private void ApplyLocks()
    {
       
        if (!levelsContainer) return;

        foreach (var meta in levelsContainer.GetComponentsInChildren<LevelButtonMeta>(true))
        {
          
            if (!meta || !meta.button) continue;

            bool unlocked = trickShotData.IsUnlocked(meta.levelNo);
            
            meta.button.interactable = unlocked;

            if (meta.lockGO) meta.lockGO.SetActive(!unlocked);

            // NEW: reflect best-stars saved in PlayerPrefs
            meta.RefreshStars(trickShotData);
        }
    }

}

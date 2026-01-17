using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class TrickShotManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public InputDataSO inputData;
    public UIOutputDataSO uIOutputData;
    public TrickShotsDataSO trickShotData;
    public CamPositionSO camPosition;
    public StrikerDataSO strikerData;
    public MyPlayerSettings myPlayerSettings;
 

    [Header("Panels")]
    public GameObject LevelPanel;
    public GameObject InGamePanel;
    public GameObject ResultPanel;
    public GameObject CountDownPanel;
    public GameObject ExitPanel;

    [Header("Objects")]
    public GameObject Board; 
    public ParticleSystem winPs1;
    public ParticleSystem winPs2;
    public AudioSource winSound;
    public AudioSource loseSound;

    [Header("Striker Objects")]
    public GameObject MyStriker;
    public IStrikerMovement strikerMovement;
    public StrikerShooting strikerShooting;
    public StrikerArrow strikerArrow;

    [Header("Interaction Objects")]
    public GameObject leftInteractions;
    public GameObject rightInteractions;


    [Header("Countdown")]
    public CountdownUI countdown;   // keep this active/visible during countdown
    public int countdownFrom = 3;

    private bool isLoading;
    private GameObject currentLevelInstance;
    private Action<GameObject> strikerCreatedEvent;


    private void OnEnable()
    {
       
        trickShotData.LoadTrickShotLevelEvent += LoadLevel;   // will start a coroutine wrapper 
        trickShotData.LevelSuccessEvent += LevelSuccess;
        trickShotData.LevelFailEvent += LevelFail;
        

        uIOutputData.ExitGameEvent += GoToHome;
        uIOutputData.HomeEvent += GoToHome;

        Initialise();
        uIOutputData.SetIsPlaying(true);
        if (myPlayerSettings.myDominantHand == DominantHand.RIGHT)
        {
            rightInteractions.SetActive(false);
            leftInteractions.SetActive(true);
        }
        else
        {
            rightInteractions.SetActive(true);
            leftInteractions.SetActive(false);
        }
    }



    private void OnDisable()
    {
        trickShotData.LoadTrickShotLevelEvent -= LoadLevel;    
        trickShotData.LevelSuccessEvent -= LevelSuccess;
        trickShotData.LevelFailEvent -= LevelFail;
        

        uIOutputData.ExitGameEvent -= GoToHome;
        uIOutputData.HomeEvent -= GoToHome;

        uIOutputData.SetIsPlaying(false);
        rightInteractions.SetActive(true);
        leftInteractions.SetActive(true);
    }

    private void GoToHome()
    {
        
        // Clean up any existing level instance (optional)
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = null;
        }
    }

    // ---- FILL THIS ----
    public void Initialise()
    {
        camPosition.SetCamPosition(1);

        Board.SetActive(true);
    }

    public void CreateStriker()
    {
        DestroyStriker();
        // Define the callback here
        strikerCreatedEvent = (GameObject striker) =>
        {

            MyStriker = striker;
            striker.transform.parent = gameObject.transform;
            strikerMovement = striker.GetComponent<StrikerMovement>();
            strikerShooting = striker.GetComponent<StrikerShooting>();         
            strikerArrow = striker.GetComponent<StrikerArrow>();

            strikerShooting.StrikeFinishedEvent += StrikeFinished;
            strikerShooting.StrikeStartedEvent += StrikeStarted;
        };

        strikerData.CreateStriker(1, myPlayerSettings.MyStrikerId, strikerCreatedEvent);
    }

    public void DestroyStriker()
    {
        if (MyStriker != null)
        {
            strikerShooting.StrikeFinishedEvent -= StrikeFinished;
            strikerShooting.StrikeStartedEvent -= StrikeStarted;
            Destroy(MyStriker);
        }
    }


    // ---------- Events ----------

    private void LevelSuccess(int stars)
    {
        ResetPanels();
        ResultPanel.SetActive(true);

        var panel = ResultPanel.GetComponent<LevelResultPanel>();
        panel.LevelSuccess(stars);

        winPs1.Play();
        winPs2.Play();
        winSound.Play();
    }

    private void LevelFail()
    {
        ResetPanels();
        ResultPanel.SetActive(true);
        ResultPanel.GetComponent<LevelResultPanel>().LevelFail();
        loseSound.Play();
    }

    private void StrikeStarted(float force, Vector3 direction)
    {
        inputData.DeactivateInput();
        strikerArrow.TurnOffArrow();
    }

    private void StrikeFinished()
    {
        strikerMovement.ResetStriker();
        strikerArrow.TurnOnArrow();
        inputData.ActivateInput();
    }

    // ---------- Level Load with countdown ----------

    // NOTE: This is subscribed to the SO event; just start the routine here.
    private void LoadLevel(int level)
    {
        if (isLoading) return;
        StartCoroutine(LoadLevelRoutine(level));
    }

    private IEnumerator LoadLevelRoutine(int level)
    {
        isLoading = true;
        ResetPanels();
        // Do NOT hide the panel that contains this script (coroutines stop on disable).
        // Keep LevelPanel visible (or dim it) while you show the countdown overlay.
        if (countdown != null)
        {
            // Show countdown ON TOP of current UI
            yield return StartCoroutine(countdown.PlayRoutine(countdownFrom));
        }

        // Clean up any existing level instance (optional)
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = null;
        }

        // Now load and instantiate the level
        var path = $"TrickShots/TrickShot{level}";
        GameObject levelPrefab = Resources.Load<GameObject>(path);
        if (levelPrefab != null)
        {
            currentLevelInstance = Instantiate(levelPrefab, transform, false);
           

            // Switch panels AFTER countdown & instantiation
            ResetPanels();
            InGamePanel.SetActive(true);
        }
        else
        {
            
            // Stay on LevelPanel if not found
            ResetPanels();
            LevelPanel.SetActive(true);
        }

        isLoading = false;
    }

    private void ResetPanels()
    {
        LevelPanel.SetActive(false);
        InGamePanel.SetActive(false);
        ResultPanel.SetActive(false);
        CountDownPanel.SetActive(false);
        ExitPanel.SetActive(false);
    }
}

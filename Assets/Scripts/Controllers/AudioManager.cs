using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    [SerializeField] private AudioSource buttonClickAS;
    [SerializeField] private AudioSource putFineAS;
    [SerializeField] private AudioSource turnChangedAS;
    [SerializeField] private AudioSource winningAS;
    [SerializeField] private AudioSource losingAS;
    [SerializeField] private AudioSource redCoveredAS;
    [SerializeField] public AudioSource backgroundMusicAS;
    [SerializeField] private AudioSource PopUpSound;
    [SerializeField] private AudioSource ToastSound;
    [SerializeField] private AudioSource NotificationSound;
    [SerializeField] private AudioSource ClockSound;

    private void Awake()
    {
        instance = this;
    }
    public void SetBackGroundVolume(float volume)
    {
        
        backgroundMusicAS.volume = volume;
    }
    public void PlayButtonClickSound()
    {
        buttonClickAS.Play();
    }

    public void PlayRedCoveredSound()
    {
        redCoveredAS.Play();
    }

    public void PlayWinningSound()
    {
        winningAS.Play();
    }

    public void PlayLosingSound()
    {
        losingAS.Play();
    }

    public void PlayTurnChangedSound()
    {
        turnChangedAS.Play();
    }

    public void PlayFoulSound()
    {
        putFineAS.Play();
    }

    public void PlayPopUpSound()
    {
        PopUpSound.Play();
    }

    public void PlayToastSound()
    {
        ToastSound.Play();
    }

    public void PlayNotificationSound()
    {
        NotificationSound.Play();
    }

    public void PlayClockSound()
    {
        ClockSound.Play();
    }

    public void StopClockSound()
    {
        ClockSound.Stop();
    }
}

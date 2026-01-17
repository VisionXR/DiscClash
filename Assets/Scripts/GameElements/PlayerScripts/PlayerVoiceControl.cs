using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Photon.Voice.Unity;
using UnityEngine;

public class PlayerVoiceControl : MonoBehaviour { 

    [Header(" Scriptable Objects Objects")]
    public UIOutputDataSO uIOutputData;

    [Header(" Local Objects")]
    public Player currentPlayer;
    public AudioSource speaker;
    public Recorder recorder;

    public void OnEnable()
    {
        uIOutputData.TurnOnMicEvent += TurnOnMic;
        uIOutputData.TurnOffMicEvent += TurnOffMic;
        uIOutputData.TurnOnSpeakerEvent += TurnOnSpeaker;
        uIOutputData.TurnOffSpeakerEvent += TurnOffSpeaker;
    }

    private void OnDisable()
    {
        uIOutputData.TurnOnMicEvent -= TurnOnMic;
        uIOutputData.TurnOffMicEvent -= TurnOffMic; 
        uIOutputData.TurnOnSpeakerEvent -= TurnOnSpeaker;
        uIOutputData.TurnOffSpeakerEvent -= TurnOffSpeaker;
    }

    private void TurnOnSpeaker()
    {
        if (currentPlayer.myPlayerRole == PlayerRole.Human &&  currentPlayer.myPlayerControl == PlayerControl.Remote)
        {
            speaker.mute = false;
        }
    }

    private void TurnOffSpeaker()
    {
        if (currentPlayer.myPlayerRole == PlayerRole.Human  && currentPlayer.myPlayerControl == PlayerControl.Remote)
        {
            speaker.mute = true;
        }
    }

    private void TurnOnMic()
    {
        if (currentPlayer.myPlayerRole == PlayerRole.Human && currentPlayer.myPlayerControl == PlayerControl.Local)
        {
           
            recorder.TransmitEnabled = true;
        }
    }

    private void TurnOffMic()
    {
        if (currentPlayer.myPlayerRole == PlayerRole.Human &&  currentPlayer.myPlayerControl == PlayerControl.Local)
        {
            
            recorder.TransmitEnabled = false;
        }
    }
}

using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Photon.Voice.Unity;
using UnityEngine;

public class PlayerVoiceControl : MonoBehaviour { 


    [Header(" Local Objects")]
    public Player currentPlayer;
    public AudioSource speaker;
    public Recorder recorder;



    public void TurnOnSpeaker()
    {
        if (currentPlayer.myPlayerRole == PlayerRole.Human &&  currentPlayer.myPlayerControl == PlayerControl.Remote)
        {
            speaker.mute = false;
        }
    }

    public void TurnOffSpeaker()
    {
        if (currentPlayer.myPlayerRole == PlayerRole.Human  && currentPlayer.myPlayerControl == PlayerControl.Remote)
        {
            speaker.mute = true;
        }
    }

    public void TurnOnMic()
    {
        if (currentPlayer.myPlayerRole == PlayerRole.Human && currentPlayer.myPlayerControl == PlayerControl.Local)
        {
           
            recorder.TransmitEnabled = true;
        }
    }

    public void TurnOffMic()
    {
        if (currentPlayer.myPlayerRole == PlayerRole.Human &&  currentPlayer.myPlayerControl == PlayerControl.Local)
        {
            
            recorder.TransmitEnabled = false;
        }
    }
}

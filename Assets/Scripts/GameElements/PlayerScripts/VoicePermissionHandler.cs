using Fusion;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.Android; // Required for Android permissions

public class VoicePermissionHandler : NetworkBehaviour
{
    public Recorder photonRecorder;
    public AudioSource speaker;


    void Start()
    {
        if (HasStateAuthority)
        {
            if (Application.isEditor)
            {
                StartRecording();
            }

            else
            {

                if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
                {
                    // This triggers the system popup
                    var callbacks = new PermissionCallbacks();
                    callbacks.PermissionGranted += OnPermissionGranted;
                    Permission.RequestUserPermission(Permission.Microphone, callbacks);
                }
                else
                {
                    StartRecording();
                }
            }

        }
        else
        {
            photonRecorder.enabled = false;
            speaker.mute = false;        
        }
    }
    void OnPermissionGranted(string permissionName)
    {
        if (permissionName == Permission.Microphone)
        {
            StartRecording();
        }
    }

    void StartRecording()
    {
        if (photonRecorder != null)
        {
            photonRecorder.RecordingEnabled = true;

            photonRecorder.SetAndroidNativeMicrophoneSettings(true, true, true);
            speaker.mute = true;
            speaker.enabled = false;

            photonRecorder.RestartRecording();

           
        }
    }
}
using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class MicAndSpeakerControls : MonoBehaviour
    {
        [Header("ScriotableObjects")]
        public PlayersDataSO playersData;

        [Header("Images")]
        public Image MicImage;
        public Image SpeakerImage;

        [Header("Sprites")]
        public Sprite MicOnSprite;
        public Sprite MicOffSprite;
        public Sprite SpeakerOnSprite;
        public Sprite SpeakerOffSprite;


        public void MicBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            Player mp = playersData.GetMainPlayer();

            if (mp != null)
            {
                PlayerVoiceControl pvc = mp.GetComponent<PlayerVoiceControl>();
                if (pvc != null)
                {

                    if (MicImage.sprite == MicOnSprite)
                    {
                        MicImage.sprite = MicOffSprite;
                        pvc.TurnOffMic();
                    }
                    else
                    {
                        MicImage.sprite = MicOnSprite;
                        pvc.TurnOnMic();
                    }
                }
            }
        }

        public void SpeakerBtnClicked()
        {
            AudioManager.instance.PlayButtonClickSound();
            Player op = playersData.GetOtherPlayer();

            if (op != null)
            {
                PlayerVoiceControl pvc = op.GetComponent<PlayerVoiceControl>();
                if (pvc != null)
                {

                    if (SpeakerImage.sprite == SpeakerOnSprite )
                    {
                        SpeakerImage.sprite = SpeakerOffSprite;
                        pvc.TurnOffSpeaker();
                    }
                    else
                    {
                        SpeakerImage.sprite = SpeakerOnSprite;
                        pvc.TurnOnSpeaker();
                    }
                }
            }
        }
    }
}

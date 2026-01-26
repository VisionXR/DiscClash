using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace Com.VisionXR.GameElements
{
    public class StrikerAudio : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public StrikerDataSO strikerData;
        public CoinDataSO coinData;
      
        [Header(" Local variables")]
        [SerializeField] private AudioSource[] audioSources;
        [SerializeField] private float CutOffVelocityForMaxAudio = 1f;
        [SerializeField] private float MinVelocityToPlayAudio = 0.005f;

        void Start()
        {
            audioSources = GetComponents<AudioSource>();
        }

        public void StrikerAudioReceived(AudioData data)
        {
            if (data.audioType == com.VisionXR.HelperClasses.AudioType.Coin)
            {
                PlayCoinCollisionAudio(data.volume);
            }
            else if (data.audioType == com.VisionXR.HelperClasses.AudioType.Hole)
            {
                PlayCoinFellInHoleAudio(data.volume);
            }
            else if (data.audioType == com.VisionXR.HelperClasses.AudioType.Edge)
            {
                PlayEdgeCollisionAudio(data.volume);
            }

        }


        public void PlayCoinCollisionAudio(float volume)
        {
            audioSources[0].volume = volume;
            audioSources[0].Play();
        }
        public void PlayEdgeCollisionAudio(float volume)
        {
            audioSources[1].volume = volume;
            audioSources[1].Play();
        }
        public void PlayCoinFellInHoleAudio(float volume)
        {
            audioSources[2].volume = volume;
            audioSources[2].Play();
        }

        public void OnCollisionEnter(Collision collision)
        {
            GameObject collidedObject = collision.gameObject;
            if (collidedObject.tag == "Edge")
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb.linearVelocity.magnitude > CutOffVelocityForMaxAudio)
                {
                    PlayEdgeCollisionAudio(1);
                    SendAudioData(com.VisionXR.HelperClasses.AudioType.Edge, 1);

                }
                else if (rb.linearVelocity.magnitude > MinVelocityToPlayAudio && rb.linearVelocity.magnitude < CutOffVelocityForMaxAudio)
                {
                    float volume = (1 / CutOffVelocityForMaxAudio - MinVelocityToPlayAudio) * (rb.linearVelocity.magnitude);
                    PlayEdgeCollisionAudio(volume);
                    if (volume > 0.2f)
                    {
                        SendAudioData(com.VisionXR.HelperClasses.AudioType.Edge, volume);
                    }

                }
            }

            if (collidedObject.tag == "Black" || collidedObject.tag == "White" || collidedObject.tag == "Red" || collidedObject.tag == "Striker")
            {
                Rigidbody otherbody = collidedObject.GetComponent<Rigidbody>();
                Rigidbody thisbody = GetComponent<Rigidbody>();
                Vector3 relativeVelocity = (thisbody.linearVelocity - otherbody.linearVelocity);
                if (relativeVelocity.magnitude > CutOffVelocityForMaxAudio)
                {
                    PlayCoinCollisionAudio(1);
                    SendAudioData(com.VisionXR.HelperClasses.AudioType.Coin, 1);

                }
                else if (relativeVelocity.magnitude > MinVelocityToPlayAudio  && relativeVelocity.magnitude < CutOffVelocityForMaxAudio)
                {
                    float volume = (1 / (CutOffVelocityForMaxAudio - MinVelocityToPlayAudio)) * (relativeVelocity.magnitude - MinVelocityToPlayAudio);
                    PlayCoinCollisionAudio(volume);
                    if (volume > 0.2f)
                    {
                        SendAudioData( com.VisionXR.HelperClasses.AudioType.Coin, volume);
                    }

                }
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Hole")
            {
                PlayCoinFellInHoleAudio(1);
                     
            }
        }

        public void SendAudioData( com.VisionXR.HelperClasses.AudioType audioType, float volume = 1)
        {
            AudioData audioObject = new AudioData();
            audioObject.audioType = audioType;
            audioObject.volume = volume;
        //    networkData.SetAudioData(audioObject);

        }

    }

}
    
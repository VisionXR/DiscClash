using System;
using UnityEngine;

namespace Com.VisionXR.GameElements
{
    public class CoinAudio : MonoBehaviour
    {
        
        [SerializeField] private AudioSource[] audioSources;
        [SerializeField] private float CutOffVelocityForMaxAudio = 0.3f;
        [SerializeField] private float MinVelocityToPlayAudio = 0.01f;

    
        void Start()
        {
            audioSources = GetComponents<AudioSource>();
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
                 
                }
                else if (rb.linearVelocity.magnitude > MinVelocityToPlayAudio && rb.linearVelocity.magnitude < CutOffVelocityForMaxAudio)
                {
                    float volume = (1 / CutOffVelocityForMaxAudio - MinVelocityToPlayAudio) * (rb.linearVelocity.magnitude);
                    PlayEdgeCollisionAudio(volume);
                

                }
            }
        }
        public void OnCollisionExit(Collision collision)
        {
            GameObject collidedObject = collision.gameObject;
            if (collidedObject.tag == "Black" || collidedObject.tag == "White" || collidedObject.tag == "Red" || collidedObject.tag == "Striker")
            {
                Rigidbody rb = collidedObject.GetComponent<Rigidbody>();
                Rigidbody thisbody = GetComponent<Rigidbody>();
                if (thisbody.linearVelocity.magnitude > CutOffVelocityForMaxAudio)
                {
                    PlayCoinCollisionAudio(1);
                  

                }
                else if (rb.linearVelocity.magnitude > MinVelocityToPlayAudio && thisbody.linearVelocity.magnitude > MinVelocityToPlayAudio && thisbody.linearVelocity.magnitude < CutOffVelocityForMaxAudio)
                {
                    float volume = (1 / (CutOffVelocityForMaxAudio - MinVelocityToPlayAudio)) * (thisbody.linearVelocity.magnitude - MinVelocityToPlayAudio);
                    PlayCoinCollisionAudio(volume);
                   
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

    }
}

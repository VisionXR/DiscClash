using UnityEngine;

namespace Com.VisionXR.GameElements
{
    public class CoinAudio : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float cutOffVelocityForMaxAudio = 1f;
        [SerializeField] private float minVelocityToPlayAudio = 0.005f; // Increased slightly to ignore jitter

        private AudioSource[] _audioSources;
        private Rigidbody _rb;

        void Start()
        {
            _audioSources = GetComponents<AudioSource>();
            _rb = GetComponent<Rigidbody>();
        }

        public void OnCollisionEnter(Collision collision)
        {
            // Use relativeVelocity to determine impact strength
            float impactVelocity = collision.relativeVelocity.magnitude;

            // 1. Exit early if the hit is too weak (prevents physics jitter sounds)
            if (impactVelocity < minVelocityToPlayAudio) return;

            // 2. Map the velocity to a 0-1 volume range
            float volume = Mathf.InverseLerp(minVelocityToPlayAudio, cutOffVelocityForMaxAudio, impactVelocity);

            // 3. Determine which sound to play based on Tag
            if (collision.gameObject.CompareTag("Edge"))
            {
                PlayAudio(1, volume);
            }
        }

        public void OnCollisionExit(Collision collision)
        {
            // Use relativeVelocity to determine impact strength
            float impactVelocity = collision.relativeVelocity.magnitude;

            // 1. Exit early if the hit is too weak (prevents physics jitter sounds)
            if (impactVelocity < minVelocityToPlayAudio) return;

            // 2. Map the velocity to a 0-1 volume range
            float volume = Mathf.InverseLerp(minVelocityToPlayAudio, cutOffVelocityForMaxAudio, impactVelocity);


            if (IsCoinOrStriker(collision.gameObject))
            {
             
                PlayAudio(0, volume);
                
            }
        }
        

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hole"))
            {
                PlayAudio(2, 1.0f);
            }
        }

        private void PlayAudio(int index, float volume)
        {
            if (_audioSources.Length <= index) return;

            _audioSources[index].volume = volume;
            _audioSources[index].Play();
        }

        private bool IsCoinOrStriker(GameObject go)
        {
            return go.CompareTag("Black") || go.CompareTag("White") ||
                   go.CompareTag("Red") || go.CompareTag("Striker");
        }
    }
}
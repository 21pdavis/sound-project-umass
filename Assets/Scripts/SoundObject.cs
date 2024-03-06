using UnityEngine;

using static Helpers;

public class SoundObject : MonoBehaviour
{
    public AudioSource audioSource;

    [SerializeField]
    [Tooltip("Time before the sound begins to decay")]
    private float decayDelay = 0.75f;

    [SerializeField]
    [Tooltip("The value subtracted from the source's volume every 0.1 seconds")]
    private float decayRate = 0.1f;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("SoundPulse"))
        {
            Debug.Log($"Playing sound in object {gameObject.name}");

            audioSource.volume = 1;
            audioSource.Play();
            StartCoroutine(DecaySound(audioSource, decayRate: decayRate, timeAtMaxVolume: decayDelay));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("entered");
    }
}

using UnityEngine;

using static Helpers;

public class SoundObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    [Tooltip("Time before the sound begins to decay")]
    private float decayDelay;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("SoundPulse"))
        {
            source.volume = 1;
            source.Play();
            StartCoroutine(DecaySound(source, timeAtMaxVolume: decayDelay));
        }
    }
}

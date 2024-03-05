using UnityEngine;

using static Helpers;

public class SoundObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("SoundPulse"))
        {
            source.volume = 1;
            source.Play();
            StartCoroutine(DecaySound(source));
        }
    }
}

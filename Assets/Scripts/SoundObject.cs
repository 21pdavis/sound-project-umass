using UnityEngine;

using static Helpers;

public class SoundObject : MonoBehaviour
{
    [SerializeField]
    private AudioClip soundClip;

    [SerializeField]
    [Tooltip("Time before the sound begins to decay")]
    private float decayDelay = 0.75f;

    [SerializeField]
    [Tooltip("The value subtracted from the source's volume every 0.1 seconds")]
    private float decayRate = 0.1f;

    [Header("References")]
    [SerializeField]
    private GameObject soundPointPrefab;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("SoundPulse"))
        {
            Debug.Log($"Playing sound in object {gameObject.name}");

            // TODO: get contact point as spawn point for soundPoint
            GameObject soundPoint = Instantiate(soundPointPrefab, transform.position, Quaternion.identity, transform);
            AudioSource source = soundPoint.GetComponent<AudioSource>();
            source.volume = 1;
            source.clip = soundClip;
            source.Play();
            StartCoroutine(DecaySound(source, decayRate: decayRate, timeAtMaxVolume: decayDelay, destroyObject: true));
        }
    }
}

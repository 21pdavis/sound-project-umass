using System.Collections;
using UnityEngine;

public static class Helpers
{
    public static IEnumerator DecaySound(AudioSource source, float decayRate=0.025f, float timeAtMaxVolume=0f, bool destroyObject=false)
    {
        float startTime = Time.time;
        while (Time.time < startTime + timeAtMaxVolume)
        {
            yield return new WaitForEndOfFrame();
        }

        while (source.volume > 0)
        {
            Debug.Log($"Source volume is {source.volume}");
            source.volume = Mathf.Clamp01(source.volume - decayRate);
            yield return new WaitForSeconds(0.1f);
        }

        source.Stop();
        if (destroyObject )
        {
            Object.Destroy(source.gameObject);
        }
        else
        {
            source.volume = 1;
        }
    }
}

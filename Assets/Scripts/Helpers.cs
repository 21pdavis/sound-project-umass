using System.Collections;
using UnityEngine;

public static class Helpers
{
    public static IEnumerator DecaySound(AudioSource source, float decayRate=0.025f)
    {
        while (source.volume > 0)
        {
            Debug.Log($"Source volume is {source.volume}");
            source.volume = Mathf.Clamp01(source.volume - decayRate);
            yield return new WaitForSeconds(0.1f);
        }

        source.Stop();
        source.volume = 1;
    }
}

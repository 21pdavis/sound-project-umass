using System.Collections.Generic;
using UnityEngine;

public class SoundWall : MonoBehaviour
{
    [SerializeField]
    private AudioClip sound;

    [SerializeField]
    private int horizontalSteps;

    [SerializeField]
    private int verticalSteps;

    [SerializeField]
    private MeshRenderer meshRenderer;

    private List<List<GameObject>> soundPoints;

    private Vector3 topLeft;
    private int totalSteps = -1;
    private int currTotalSteps;

    private void Start()
    {
        topLeft = transform.position - meshRenderer.bounds.size.x / 2 * Vector3.right + meshRenderer.bounds.size.y / 2 * Vector3.up;
    }

    private void Update()
    {
        if (currTotalSteps != totalSteps)
        {
            totalSteps = currTotalSteps;
            UpdateSoundPoints();
        }

        currTotalSteps = horizontalSteps * verticalSteps;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (Application.isPlaying)
        {
            foreach (List<GameObject> list in soundPoints)
            {
                foreach (GameObject obj in list)
                {
                    Gizmos.DrawWireSphere(obj.transform.position, 0.5f);
                }
            }
        }
    }

    private void InitAudioSource(AudioSource source)
    {
        source.clip = sound;
        source.playOnAwake = true;
        source.loop = true;
        source.spatialize = true;
        source.spatialBlend = 1;

        // TODO: parameterize wall sweep vs full grid play
        //source.Play();
    }

    private void UpdateSoundPoints()
    {
        soundPoints = new List<List<GameObject>>();

        for (int i = 0; i <= horizontalSteps; i++)
        {
            soundPoints.Add(new List<GameObject>());
            for (int j = 0; j <= verticalSteps; j++)
            {
                Vector3 pos = topLeft + new Vector3(
                    i * (meshRenderer.bounds.size.x / horizontalSteps),
                    -(j * (meshRenderer.bounds.size.y / verticalSteps)),
                    0f
                );

                GameObject soundPoint = new GameObject($"SoundPoint{i}_{j}");
                soundPoint.transform.position = pos;

                soundPoint.AddComponent<AudioSource>();
                InitAudioSource(soundPoint.GetComponent<AudioSource>());

                soundPoints[i].Add(soundPoint);
            }
        }
    }
}

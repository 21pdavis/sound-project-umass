using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SoundWall : MonoBehaviour
{
    [SerializeField]
    private AudioClip soundClip;

    [SerializeField]
    private int horizontalSteps;

    [SerializeField]
    private int verticalSteps;

    [SerializeField]
    private bool sweep;

    [SerializeField]
    private float sweepSpeed;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [Header("References")]
    [SerializeField]
    private GameObject soundPointPrefab;

    private List<List<GameObject>> soundPoints;

    private Vector3 topLeft;
    private int totalSteps = -1;
    private int currTotalSteps;

    //! DEBUG
    private GameObject sweepPoint;

    private void Start()
    {
        topLeft = transform.position - meshRenderer.bounds.size.x / 2 * transform.right + meshRenderer.bounds.size.y / 2 * transform.up;
    }

    private void Update()
    {
        if (currTotalSteps != totalSteps && !sweep)
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
            if (soundPoints != null)
            {
                foreach (List<GameObject> list in soundPoints)
                {
                    foreach (GameObject obj in list)
                    {
                        Gizmos.DrawWireSphere(obj.transform.position, 0.5f);
                    }
                }
            }

            if (sweepPoint != null)
            {
               Gizmos.DrawWireSphere(sweepPoint.transform.position, 1f);
            }
        }
    }

    private GameObject InitSoundObject(GameObject soundPoint, Vector3 pos)
    {
        soundPoint.transform.position = pos;
        soundPoint.transform.parent = transform;

        soundPoint.AddComponent<AudioSource>();
        AudioSource source = soundPoint.GetComponent<AudioSource>();

        source.clip = soundClip;
        source.playOnAwake = false;
        source.loop = true;

        // 3D Settings
        source.spatialize = true;
        source.spatialBlend = 1;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.minDistance = 1;
        source.maxDistance = 15;
        source.volume = 1f;

        soundPoint.AddComponent<SphereCollider>();
        SphereCollider collider = soundPoint.GetComponent<SphereCollider>();
        collider.radius = 1f;
        collider.isTrigger = true;

        //soundPoint.AddComponent<SoundObject>();
        //SoundObject obj = soundPoint.GetComponent<SoundObject>();
        //obj.audioSource = source;

        return soundPoint;
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

                soundPoints[i].Add(InitSoundObject(new GameObject($"SoundPoint{i}_{j}"), pos));
            }
        }
    }

    private IEnumerator SweepSound(GameObject soundPoint, Vector3 destination)
    {
        while (Vector3.Distance(soundPoint.transform.position, destination) > 0.01f)
        {
            soundPoint.transform.position -= Time.deltaTime * sweepSpeed * transform.right;
            yield return new WaitForEndOfFrame();
        }

        Destroy(soundPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sweep && other.CompareTag("SoundPulse"))
        {
            Vector3 pos = transform.position + (meshRenderer.bounds.size.x / 1.5f) * transform.right;

            sweepPoint = Instantiate(soundPointPrefab, pos, Quaternion.identity, transform);
            AudioSource source = sweepPoint.GetComponent<AudioSource>();
            source.clip = soundClip;
            source.Play();

            StartCoroutine(SweepSound(sweepPoint, transform.position - meshRenderer.bounds.size.x / 1.5f * transform.right));
        }
    }
}

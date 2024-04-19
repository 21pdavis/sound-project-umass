using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundWall : MonoBehaviour
{
    private UnityEvent<GameObject, Vector3> PulseCollide = new();

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

    public Transform sweepStart;

    public Transform sweepEnd;

    //[SerializeField]
    //private MeshRenderer meshRenderer;

    [Header("References")]
    [SerializeField]
    private GameObject soundPointPrefab;

    private List<List<GameObject>> soundPoints;

    private Vector3 topLeft;
    private int totalSteps = -1;
    private int currTotalSteps;

    //! DEBUG
    private GameObject sweepPoint1;
    private GameObject sweepPoint2;

    private void Start()
    {
        //! assumes that the parent is the room
        PulseCollide.AddListener(transform.parent.GetComponent<Room>().StartSweep);
    }

    private void Update()
    {
        if (currTotalSteps != totalSteps && !sweep)
        {
            totalSteps = currTotalSteps;
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

            if (sweepPoint1 != null)
            {
               Gizmos.DrawWireSphere(sweepPoint1.transform.position, 1f);
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

    //private void UpdateSoundPoints()
    //{
    //    soundPoints = new List<List<GameObject>>();

    //    for (int i = 0; i <= horizontalSteps; i++)
    //    {
    //        soundPoints.Add(new List<GameObject>());
    //        for (int j = 0; j <= verticalSteps; j++)
    //        {
    //            Vector3 pos = topLeft + new Vector3(
    //                i * (meshRenderer.bounds.size.x / horizontalSteps),
    //                -(j * (meshRenderer.bounds.size.y / verticalSteps)),
    //                0f
    //            );

    //            soundPoints[i].Add(InitSoundObject(new GameObject($"SoundPoint{i}_{j}"), pos));
    //        }
    //    }
    //}

    private IEnumerator SweepSound(GameObject soundPoint, Vector3 destination)
    {
        Vector3 direction = -(destination - soundPoint.transform.position).normalized;

        while (Vector3.Distance(soundPoint.transform.position, destination) > 0.1f)
        {
            soundPoint.transform.position -= Time.deltaTime * sweepSpeed * direction;
            yield return new WaitForEndOfFrame();
        }

        Destroy(soundPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sweep && other.CompareTag("SoundPulse"))
        {
            Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            PulseCollide.Invoke(gameObject, contactPoint);
        }
    }
}

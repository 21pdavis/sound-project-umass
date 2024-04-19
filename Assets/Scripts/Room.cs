using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> walls;

    [SerializeField]
    private AudioClip soundClip;

    [SerializeField]
    private GameObject soundPointPrefab;

    [SerializeField]
    private float sweepSpeed;

    private GameObject sweepPoint1;
    private GameObject sweepPoint2;
    private int sweepCount = 0;

    private GameObject startWall;

    private IEnumerator SendSoundPointOnPath(GameObject soundPoint, List<Vector3> path)
    {
        //Vector3 direction = -(destination - soundPoint.transform.position).normalized;

        //while (Vector3.Distance(soundPoint.transform.position, destination) > 0.1f)
        //{
        //    soundPoint.transform.position -= Time.deltaTime * sweepSpeed * direction;
        //    yield return new WaitForEndOfFrame();
        //}

        foreach (Vector3 pathNode in path)
        {
            Vector3 direction = pathNode - soundPoint.transform.position;

            while (Vector3.Distance(soundPoint.transform.position, pathNode) > 0.1f)
            {
                soundPoint.transform.position += Time.deltaTime * sweepSpeed * direction.normalized;
                yield return new WaitForEndOfFrame();
            }
        }

        sweepCount -= 1;
    }

    private IEnumerator RevolveSound(GameObject soundPoint1, GameObject soundPoint2, List<Vector3> path1, List<Vector3> path2)
    {
        // start both Sends and then wait for both to finish before resetting startWall and destroying sweepPoints
        StartCoroutine(SendSoundPointOnPath(soundPoint1, path1));
        StartCoroutine(SendSoundPointOnPath(soundPoint2, path2));

        sweepCount += 2;
        while (sweepCount > 0)
        {
            yield return new WaitForEndOfFrame();
        }

        startWall = null;
        Destroy(sweepPoint1);
        Destroy(sweepPoint2);
    }

    public void StartSweep(GameObject wall, Vector3 startPoint)
    {
        if (startWall != null)
        {
            Debug.Log("Start wall currently set, wait for next sweep");
            return;
        }

        startWall = wall;

        // setup
        sweepPoint1 = Instantiate(soundPointPrefab, startPoint, Quaternion.identity, transform);
        sweepPoint2 = Instantiate(soundPointPrefab, startPoint, Quaternion.identity, transform);

        AudioSource source1 = sweepPoint1.GetComponent<AudioSource>();
        source1.clip = soundClip;
        source1.Play();

        AudioSource source2 = sweepPoint2.GetComponent<AudioSource>();
        source2.clip = soundClip;
        source2.Play();

        // determine path for each soundPoint
        int wallIndex = walls.IndexOf(wall);

        List<Vector3> path1 = new() { walls[wallIndex].GetComponent<SoundWall>().sweepStart.position };
        List<Vector3> path2 = new() { walls[wallIndex].GetComponent<SoundWall>().sweepEnd.position };

        for (int i = wallIndex + 1; i < walls.Count; i++)
        {
            path1.Add(walls[i].GetComponent<SoundWall>().sweepStart.position);
            path1.Add(walls[i].GetComponent<SoundWall>().sweepEnd.position);
        }

        for (int i = wallIndex - 1; i >= 0; i--)
        {
            path2.Add(walls[i].GetComponent<SoundWall>().sweepStart.position);
            path2.Add(walls[i].GetComponent<SoundWall>().sweepEnd.position);
        }

        StartCoroutine(RevolveSound(sweepPoint1, sweepPoint2, path1, path2));
    }
}

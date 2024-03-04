using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicWall : MonoBehaviour
{
    [SerializeField]
    private AudioClip sound;

    [SerializeField]
    private int horizontalSteps;

    [SerializeField]
    private int verticalSteps;

    private List<List<Vector3>> soundPoints;
    private MeshRenderer meshRenderer;

    private Vector3 topLeft;
    private int totalSteps = -1;
    private int currTotalSteps;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        topLeft = transform.position - meshRenderer.bounds.size.x / 2 * Vector3.right + meshRenderer.bounds.size.y / 2 * Vector3.up;
    }

    // Update is called once per frame
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
            foreach (List<Vector3> list in soundPoints)
            {
                foreach (Vector3 vec in list)
                {
                    Gizmos.DrawWireSphere(vec, 0.5f);
                }
            }
        }
    }

    void UpdateSoundPoints()
    {
        soundPoints = new List<List<Vector3>>();

        for (int i = 0; i <= horizontalSteps; i++)
        {
            soundPoints.Add(new List<Vector3>());
            for (int j = 0; j <= verticalSteps; j++)
            {
                soundPoints[i].Add(
                    topLeft + new Vector3(
                        i * (meshRenderer.bounds.size.x / horizontalSteps),
                        -(j * (meshRenderer.bounds.size.y / verticalSteps)),
                        0f
                    )
                );
            }
        }
    }
}

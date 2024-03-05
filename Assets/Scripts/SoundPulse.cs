using UnityEngine;

public class SoundPulse : MonoBehaviour
{
    public float MaxRadius = 10f;

    [SerializeField]
    private float growthRate = 5f;

    private SphereCollider detectionCollider;

    private void Start()
    {
        detectionCollider = GetComponent<SphereCollider>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionCollider.radius);
    }

    private void Update()
    {
        if (detectionCollider.radius < MaxRadius)
        {
            detectionCollider.radius += Time.deltaTime * growthRate;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

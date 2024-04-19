using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSoundDetection : MonoBehaviour
{
    [Header("Pulse Options")]
    [SerializeField]
    private Transform soundOrigin;

    [SerializeField]
    private bool continuousPulsing;

    [SerializeField]
    private float timeBetweenPulses;

    [Header("References")]
    [SerializeField]
    private GameObject soundPulse;

    private float lastPulseTime = 0f;
    private bool hasPulsed = false;
    
    private void Update()
    {
        if (continuousPulsing)
        {
            float nextPulseTime = hasPulsed ? lastPulseTime + timeBetweenPulses : 0f;
            if (Time.time >= nextPulseTime)
            {
                SpawnSoundPulse();
            }
        }
    }

    private void SpawnSoundPulse()
    {
        Debug.Log("Pulse");
        hasPulsed = true;
        lastPulseTime = Time.time;
        Instantiate(soundPulse, soundOrigin.position, Quaternion.identity);
    }

    public void SoundPulse(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        SpawnSoundPulse();
    }
}

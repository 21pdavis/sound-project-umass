using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerCameraManager : MonoBehaviour
{
    [Header("Camera Options")]
    [SerializeField]
    private float normalOrthoSize = 10f;

    [SerializeField]
    private float normalFieldOfView = 90f;

    public float firstPersonSensitivity = 0.1f;

    [Header("References")]
    [SerializeField]
    private List<MeshRenderer> playerMeshes;

    [SerializeField]
    private CinemachineVirtualCamera isometricCamera;

    [SerializeField]
    private CinemachineVirtualCamera firstPersonCamera;

    //[SerializeField]
    //private GameObject HUD;

    [SerializeField]
    private Transform firstPersonCameraMountPoint;

    [SerializeField]
    private CinemachineBrain brain;

    // TODO: rename this to something more accurate ("TopDown"?)
    public bool Isometric;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        // lock or unlock cursor
        Cursor.lockState = Isometric ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = Isometric;
        //HUD.SetActive(!Isometric);

        // transition camera (order actually matters here)
        isometricCamera.gameObject.SetActive(Isometric);
        firstPersonCamera.gameObject.SetActive(!Isometric);
        playerMovement = GetComponent<PlayerMovement>();

        isometricCamera.m_Lens.OrthographicSize = normalOrthoSize;
        firstPersonCamera.m_Lens.FieldOfView = normalFieldOfView;
    }

    private IEnumerator TemporarilyDisablePlayerMovement()
    {
        playerMovement.canMove = false;
        playerMovement.canLook = false;
        playerMovement.moveDirection = Vector3.zero;
        yield return new WaitForEndOfFrame();

        while (brain.IsBlending)
        {
            yield return new WaitForEndOfFrame();
        }

        playerMovement.canMove = true;
        playerMovement.canLook = true;
    }

    private void Update()
    {
        // TODO: better handling of Isometric --> First Person transition (disappears too early right now)
        if (playerMeshes.Count > 0)
        {
            if (
                Isometric && !playerMeshes[0].enabled
                ||
                !Isometric && playerMeshes[0].enabled
            )
            {
                // enable/disable player mesh
                foreach (MeshRenderer meshRenderer in playerMeshes)
                {
                    meshRenderer.enabled = Isometric;
                }
            }
        }
    }

    public void DebugCameraSwitch(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        Debug.Log("Switching Camera");

        Isometric = !Isometric;

        // transition camera
        isometricCamera.gameObject.SetActive(Isometric);
        firstPersonCamera.gameObject.SetActive(!Isometric);

        StartCoroutine(TemporarilyDisablePlayerMovement());

        // lock or unlock cursor
        Cursor.lockState = Isometric ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = !Isometric;
        //HUD.SetActive(!Isometric);
    }

    internal CinemachineVirtualCamera GetActiveCamera()
    {
        return Isometric ? isometricCamera : firstPersonCamera;
    }

    internal void SetLensSize(float size)
    {
        if (Isometric)
        {
            isometricCamera.m_Lens.OrthographicSize = size;
        }
        else
        {
            firstPersonCamera.m_Lens.FieldOfView = size;
        }
    }

    internal void AddToLensSize(float delta)
    {
        if (Isometric)
        {
            isometricCamera.m_Lens.OrthographicSize += delta;
        }
        else
        {
            firstPersonCamera.m_Lens.FieldOfView += delta;
        }
    }

    internal float GetCurrentLensSize()
    {
        return Isometric ? isometricCamera.m_Lens.OrthographicSize : firstPersonCamera.m_Lens.FieldOfView;
    }

    internal float GetNormalLensSize()
    {
        return Isometric ? normalOrthoSize : normalFieldOfView;
    }
}

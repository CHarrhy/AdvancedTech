using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public GameObject entryPortalPrefab;
    public GameObject exitPortalPrefab;
    public Transform entryPortalSpawnPoint;
    public Transform exitPortalSpawnPoint;

    public float walkingSpeed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float crouchSpeed = 2.0f;
    public float mouseSensitivity = 2.0f;
    public float jumpForce = 8.0f;
    public float gravity = 30.0f;

    private CharacterController characterController;
    private Camera playerCamera;
    private float verticalRotation = 0.0f;
    private float upDownRange = 60.0f;
    private bool isSprinting = false;
    private bool isCrouching = false;
    private bool canShootPortal = true;
    private Vector3 playerVelocity;

    private Portal entryPortal;
    private Portal exitPortal;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleSprinting();
        HandleCrouching();
        HandlePortalShooting();
    }

    void HandleMovement()
    {
        float speed = (isSprinting ? sprintSpeed : (isCrouching ? crouchSpeed : walkingSpeed));

        float forwardSpeed = Input.GetAxis("Vertical") * speed;
        float sideSpeed = Input.GetAxis("Horizontal") * speed;

        Vector3 speedVector = new Vector3(sideSpeed, 0, forwardSpeed);
        speedVector = transform.rotation * speedVector;

        // Apply gravity
        if (!characterController.isGrounded)
        {
            playerVelocity.y -= gravity * Time.deltaTime;
        }
        else
        {
            playerVelocity.y = -gravity * 0.5f; // Reset velocity when grounded to avoid accumulating gravity over frames
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            playerVelocity.y = jumpForce;
        }

        characterController.Move((speedVector + playerVelocity) * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation += mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);
    }

    void HandleSprinting()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
        }
    }

    void HandleCrouching()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = !isCrouching;

            if (isCrouching)
            {
                characterController.height /= 2;
            }
            else
            {
                characterController.height *= 2;
            }
        }
    }

    void HandlePortalShooting()
    {
        if (Input.GetKeyDown(KeyCode.E) && canShootPortal)
        {
            ShootPortal(KeyCode.E);
        }

        if (Input.GetKeyDown(KeyCode.R) && canShootPortal)
        {
            ShootPortal(KeyCode.R);
        }
    }

    void ShootPortal(KeyCode key)
    {
        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                CreatePortal(key, hit.point, hit.normal);
                Debug.Log("Portal shot!");
            }
        }
    }

    void CreatePortal(KeyCode key, Vector3 position, Vector3 normal)
    {
        Portal portalToInstantiate = (key == KeyCode.E) ? entryPortalPrefab.GetComponent<Portal>() : exitPortalPrefab.GetComponent<Portal>();

        // Destroy the existing portal of the same type
        if (key == KeyCode.E && entryPortal != null)
        {
            Destroy(entryPortal.gameObject);
        }
        else if (key == KeyCode.R && exitPortal != null)
        {
            Destroy(exitPortal.gameObject);
        }

        GameObject portalObject = Instantiate(portalToInstantiate.gameObject, position, Quaternion.LookRotation(normal));
        Portal portalScript = portalObject.GetComponent<Portal>();

        if (key == KeyCode.E)
        {
            entryPortal = portalScript;
            exitPortal.SetExitPortal(portalScript.transform);
        }
        else if (key == KeyCode.R)
        {
            exitPortal = portalScript;
            entryPortal.SetExitPortal(portalScript.transform);
        }

        StartCoroutine(PortalCooldown());
        Debug.Log("Portal created! Exit Portal Position: " + portalScript.exitPortal.position);
    }

    IEnumerator PortalCooldown()
    {
        canShootPortal = false;
        yield return new WaitForSeconds(2.0f); // Adjust the cooldown duration as needed
        canShootPortal = true;
    }
}
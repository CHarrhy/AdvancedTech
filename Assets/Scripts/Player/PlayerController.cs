using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
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
    private Vector3 playerVelocity;

    public GameObject portalPrefabA;
    public GameObject portalPrefabB;

    private GameObject portalA;
    private GameObject portalB;

    private float portalOffset = 0.1f; // Adjust this value as needed

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnPortal(portalPrefabA, ref portalA);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnPortal(portalPrefabB, ref portalB);
        }

        // Check for teleportation
        CheckTeleport(portalA, portalB);
        CheckTeleport(portalB, portalA);
    }

    private void SpawnPortal(GameObject portalPrefab, ref GameObject portal)
    {
        // Raycast to find the wall
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Wall"))
        {
            // Destroy the existing portal of the same type
            if (portal != null)
            {
                Destroy(portal);
            }

            // Calculate the spawn position slightly off the wall
            Vector3 spawnPosition = hit.point + hit.normal * portalOffset;

            // Calculate the rotation to align with the wall
            Quaternion spawnRotation = Quaternion.LookRotation(hit.normal, Vector3.up);

            // Spawn the new portal
            portal = Instantiate(portalPrefab, spawnPosition, spawnRotation);

            // Get the Portal component and set the otherPortal reference for bidirectional teleportation
            Portal portalComponent = portal.GetComponent<Portal>();
            if (portalComponent != null)
            {
                portalComponent.otherPortal = (portal == portalA) ? portalB.GetComponent<Portal>() : portalA.GetComponent<Portal>();
            }
            else
            {
                Debug.LogError("Portal component not found on the spawned portal.");
            }
        }
    }

    private void CheckTeleport(GameObject fromPortal, GameObject toPortal)
    {
        if (fromPortal != null && toPortal != null)
        {
            Collider fromCollider = fromPortal.GetComponent<Collider>();
            Collider toCollider = toPortal.GetComponent<Collider>();

            if (fromCollider.bounds.Contains(transform.position))
            {
                Teleport(toPortal);
            }
        }
    }

    private void Teleport(GameObject toPortal)
    {
        // Ensure the characterController is not null
        if (characterController != null)
        {
            // Calculate the position relative to the other portal
            Vector3 portalToPlayer = transform.position - toPortal.transform.position;

            // Update the characterController's position
            characterController.enabled = false;
            transform.position = toPortal.transform.position + portalToPlayer;
            characterController.enabled = true;

            // Rotate the player to match the other portal's forward direction
            transform.forward = toPortal.transform.forward;

            Debug.Log("Player teleported successfully.");
        }
        else
        {
            Debug.LogError("CharacterController is null.");
        }
    }

    // ... (Your existing methods)

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
            playerVelocity.y = -gravity * 0.5f;
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
}

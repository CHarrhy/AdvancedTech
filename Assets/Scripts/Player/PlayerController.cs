using System.Collections;
using UnityEngine;
using System.Collections.Generic;

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

    private float portalOffset = 0.5f; // Adjust this value as needed

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

            // Spawn the new portal
            portal = Instantiate(portalPrefab, spawnPosition, Quaternion.LookRotation(hit.normal));
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
                Teleport(toPortal, fromPortal);
            }
        }
    }

    private void Teleport(GameObject toPortal, GameObject fromPortal)
    {
        // Teleport the player to the destination portal based on the relative position and rotation difference
        Vector3 portalToPlayer = transform.position - fromPortal.transform.position;
        float rotationDifference = Quaternion.Angle(fromPortal.transform.rotation, toPortal.transform.rotation);

        // Rotate the relative position by the rotation difference and apply to the destination portal's position
        Vector3 rotatedPositionOffset = Quaternion.Euler(0f, rotationDifference, 0f) * portalToPlayer;
        transform.position = toPortal.transform.position + rotatedPositionOffset;

        // Rotate the player to match the destination portal's forward direction
        transform.forward = toPortal.transform.forward;
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
// PlayerController.cs
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

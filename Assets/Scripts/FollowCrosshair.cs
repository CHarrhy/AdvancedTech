using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Reference to the main camera
    private Transform cameraTransform;

    // Offset to adjust the position of the gun relative to the camera
    public Vector3 offset = new Vector3(0f, -0.5f, 1.5f);

    // Smoothness of the gun movement
    public float smoothSpeed = 10f;

    private void Start()
    {
        // Find the main camera in the scene
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        // Call the function to make the gun follow the camera
        FollowCameraPosition();
    }

    private void FollowCameraPosition()
    {
        // Calculate the target position by adding the camera position and the offset
        Vector3 targetPosition = cameraTransform.position + cameraTransform.TransformDirection(offset);

        // Use SmoothDamp to smoothly move the gun to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Rotate the gun to match the camera's rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraTransform.rotation, smoothSpeed * Time.deltaTime);
    }
}

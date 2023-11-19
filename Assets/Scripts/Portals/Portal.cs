using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject otherPortalPrefab;  // Reference to the other portal prefab

    private GameObject otherPortal;  // Reference to the other portal instance

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Teleport(other.gameObject);
        }
    }

    private void Teleport(GameObject player)
    {
        // Check if the other portal is set
        if (otherPortal != null)
        {
            // Calculate the teleportation destination based on the other portal's position and rotation
            Vector3 offsetFromPortal = Quaternion.Euler(0, otherPortal.transform.eulerAngles.y - transform.eulerAngles.y, 0) * (player.transform.position - transform.position);
            player.transform.position = otherPortal.transform.position + offsetFromPortal;
        }
        else
        {
            Debug.LogError("Other portal reference is null. Ensure portals are paired correctly.");
        }
    }

    public void CreateOtherPortal(Vector3 position, Quaternion rotation)
    {
        if (otherPortal != null)
        {
            Destroy(otherPortal);
        }

        // Check if the otherPortalPrefab is assigned
        if (otherPortalPrefab != null)
        {
            // Instantiate the other portal prefab at the specified position and rotation
            otherPortal = Instantiate(otherPortalPrefab, position, rotation);

            // Set the reference to this portal on the other portal
            otherPortal.GetComponent<Portal>().otherPortal = gameObject;
        }
        else
        {
            Debug.LogError("otherPortalPrefab is not assigned in the Portal script.");
        }
    }
}

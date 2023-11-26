using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal otherPortal;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered the portal is the player
        if (other.CompareTag("Player"))
        {
            TeleportObject(other.gameObject);
        }
    }

    public void TeleportObject(GameObject obj)
    {
        // Your existing teleportation logic
        Debug.Log("Teleporting object.");

        if (otherPortal != null)
        {
            Vector3 portalToPlayer = obj.transform.position - transform.position;
            obj.transform.position = otherPortal.transform.position + portalToPlayer;
            obj.transform.forward = otherPortal.transform.forward;

            Debug.Log("Object teleported successfully.");
        }
        else
        {
            Debug.LogError("Other portal is null.");
        }
    }
}

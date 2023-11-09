using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform exitPortal;
    private bool isPlayerInPortal = false;

    private Transform playerToTeleport; // Added variable to store the player's transform

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPortal = true;
            playerToTeleport = other.transform; // Store the player's transform
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPortal = false;
            playerToTeleport = null; // Reset the stored player's transform
        }
    }

    void Update()
    {
        if (isPlayerInPortal && Input.GetKeyDown(KeyCode.F) && playerToTeleport != null)
        {
            Teleport(playerToTeleport); // Pass the stored player's transform to Teleport
        }
    }

    void Teleport(Transform player)
    {
        Debug.Log("Teleporting!");
        player.position = exitPortal.position;
    }

    public void SetExitPortal(Transform exit)
    {
        exitPortal = exit;
    }
}
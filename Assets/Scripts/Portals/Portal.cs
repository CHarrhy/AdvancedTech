using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal otherPortal;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered the portal trigger.");

        if (otherPortal != null)
        {
            StartCoroutine(TeleportPlayer(other.gameObject));
        }
        else
        {
            Debug.LogError("Other portal is null.");
        }
    }

    private IEnumerator TeleportPlayer(GameObject player)
    {
        // Disable the player's collider temporarily
        player.GetComponent<Collider>().enabled = false;

        Vector3 portalToPlayer = player.transform.position - transform.position;

        // Ensure the CharacterController component is not null
        CharacterController playerController = player.GetComponent<CharacterController>();
        if (playerController != null)
        {
            // Move the player to the other portal
            playerController.enabled = false;
            player.transform.position = otherPortal.transform.position + portalToPlayer;
            playerController.enabled = true;
        }
        else
        {
            Debug.LogError("CharacterController is null on the player.");
        }

        // Rotate the player to match the other portal's forward direction
        player.transform.forward = otherPortal.transform.forward;

        // Wait for a short duration for the player to be teleported smoothly
        yield return new WaitForSeconds(0.1f);

        // Enable the player's collider again
        player.GetComponent<Collider>().enabled = true;

        Debug.Log("Player teleported successfully.");
    }
}

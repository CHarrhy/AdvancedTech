using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPortal : MonoBehaviour
{
    // The destination transform within the same scene
    public Transform destinationTransform;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the portal
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the portal");

            // Teleport the player to the destinationTransform position
            TeleportPlayer(other.gameObject);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        Debug.Log("Teleporting player");

        // Get the CharacterController component
        CharacterController characterController = player.GetComponent<CharacterController>();

        // Check if CharacterController component is present
        if (characterController != null)
        {
            // Teleport the player to the destinationTransform position
            characterController.enabled = false; // Disable CharacterController temporarily
            player.transform.position = destinationTransform.position;
            characterController.enabled = true; // Re-enable CharacterController
        }
        else
        {
            Debug.LogError("CharacterController component not found on player GameObject.");
        }
    }

}


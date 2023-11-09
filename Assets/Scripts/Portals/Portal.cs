using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private void Start()
    {
        PortalManager.instance.RegisterDestinationPortal(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming the player has a "Player" tag
        {
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform player)
    {
        Portal destinationPortal = FindActiveDestinationPortal(player);

        if (destinationPortal != null)
        {
            player.position = destinationPortal.transform.position + destinationPortal.transform.forward * 2f; // Move player a bit forward from the portal
            player.rotation = destinationPortal.transform.rotation;
        }
        else
        {
            Debug.LogError("No active destination portal found!");
        }
    }

    private Portal FindActiveDestinationPortal(Transform player)
    {
        Ray ray = new Ray(player.GetComponentInChildren<Camera>().transform.position, player.GetComponentInChildren<Camera>().transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("PortalDestination"))
            {
                return hit.collider.GetComponent<Portal>();
            }
        }

        return null; // Return null if no active destination portal is found in the player's view.
    }

    public bool IsActiveDestination { get; private set; }

    public void SetActiveDestination(bool isActive)
    {
        IsActiveDestination = isActive;
    }
}
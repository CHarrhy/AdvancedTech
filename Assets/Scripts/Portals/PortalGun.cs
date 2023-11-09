using UnityEngine;

public class PortalGun : MonoBehaviour
{
    public Camera playerCamera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootPortal();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            TeleportToDestination();
        }
    }

    private void ShootPortal()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                SpawnPortal(hit.point, hit.normal);
            }
        }
    }

    private void TeleportToDestination()
    {
        Portal activeDestinationPortal = PortalManager.Instance.GetActiveDestinationPortal();

        if (activeDestinationPortal != null)
        {
            Transform player = Camera.main.transform;
            player.position = activeDestinationPortal.transform.position + activeDestinationPortal.transform.forward * 2f;
            player.rotation = activeDestinationPortal.transform.rotation;
        }
        else
        {
            Debug.LogError("No active destination portal found!");
        }
    }

    private void SpawnPortal(Vector3 position, Vector3 normal)
    {
        if (PortalManager.Instance != null)
        {
            Quaternion rotation = Quaternion.LookRotation(normal);
            PortalManager.Instance.SpawnPortal(position, rotation);
        }
        else
        {
            Debug.LogError("PortalManager not found in the scene!");
        }
    }
}
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    public GameObject bluePortalPrefab;
    public GameObject orangePortalPrefab;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootPortal(bluePortalPrefab);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ShootPortal(orangePortalPrefab);
        }
    }

    private void ShootPortal(GameObject portalPrefab)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Instantiate the portal prefab at the hit point and rotate it to match the surface normal
            GameObject newPortal = Instantiate(portalPrefab, hit.point, Quaternion.LookRotation(hit.normal));

            // Access the Portal script on the newly created portal and create the other portal
            Portal portalScript = newPortal.GetComponent<Portal>();
            portalScript.CreateOtherPortal(hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}

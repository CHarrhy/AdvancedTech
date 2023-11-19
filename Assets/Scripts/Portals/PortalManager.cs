using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public GameObject bluePortalPrefab;
    public GameObject orangePortalPrefab;

    private GameObject bluePortal;
    private GameObject orangePortal;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootPortal(bluePortalPrefab, ref bluePortal);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ShootPortal(orangePortalPrefab, ref orangePortal);
        }
    }

    private void ShootPortal(GameObject portalPrefab, ref GameObject currentPortal)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            DestroyPortal(ref currentPortal, portalPrefab == bluePortalPrefab ? orangePortalPrefab : bluePortalPrefab);
            currentPortal = Instantiate(portalPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            SetPortalReferences(currentPortal);
        }
    }

    private void SetPortalReferences(GameObject portal)
    {
        portal.GetComponent<Portal>().CreateOtherPortal(portal.transform.position, portal.transform.rotation);
    }

    private void DestroyPortal(ref GameObject portal, GameObject portalPrefabToSpawn)
    {
        if (portal != null)
        {
            Destroy(portal);
            portal = Instantiate(portalPrefabToSpawn); // Spawn the opposite portal type
            SetPortalReferences(portal);
        }
    }
}

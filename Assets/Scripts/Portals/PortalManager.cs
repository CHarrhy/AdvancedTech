using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public GameObject portalPrefab;
    public GameObject destinationPrefab;

    protected static PortalManager instance; // Change 'private' to 'protected'

    private List<Portal> portals = new List<Portal>();

    public static PortalManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PortalManager>();
            }
            return instance;
        }
    }

    public Portal GetActiveDestinationPortal()
    {
        foreach (var portal in portals)
        {
            if (portal.IsActiveDestination)
            {
                return portal;
            }
        }
        return null;
    }

    public void RegisterPortal(Portal portal)
    {
        portals.Add(portal);
    }

    public void DestroyActivePortal()
    {
        Portal activePortal = GetActiveDestinationPortal();
        if (activePortal != null)
        {
            portals.Remove(activePortal);
            Destroy(activePortal.gameObject);
        }
    }

    public void SpawnPortal(Vector3 position, Quaternion rotation)
    {
        if (portalPrefab != null)
        {
            Instantiate(portalPrefab, position, rotation);
        }
        else
        {
            Debug.LogError("Portal prefab is not set in PortalManager!");
        }
    }
}
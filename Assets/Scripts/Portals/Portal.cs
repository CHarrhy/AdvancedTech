using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform exitPortal;
    private bool isPlayerInPortal = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPortal = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInPortal = false;
        }
    }

    void Update()
    {
        if (isPlayerInPortal && exitPortal != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Teleport(player.transform);
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
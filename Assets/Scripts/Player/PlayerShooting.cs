using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform gunBarrel;
    public float fireRate = 0.5f;
    public int damage = 10;
    public LayerMask enemyLayer;

    private float nextFireTime;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Shoot()
    {
        Vector3 raycastStart = gunBarrel.position;
        raycastStart.y = gunBarrel.position.y + gunBarrel.lossyScale.y;

        RaycastHit hit;
        if (Physics.Raycast(raycastStart, gunBarrel.forward, out hit, Mathf.Infinity, enemyLayer))
        {
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Debug.DrawLine(raycastStart, hit.point, Color.red, 0.1f); // Draw a red line from the gun to the hit point
        }
        else
        {
            Vector3 endOfRay = raycastStart + gunBarrel.forward * 100f; // Assuming a max length of 100 units
            Debug.DrawLine(raycastStart, endOfRay, Color.green, 0.1f); // Draw a green line to indicate the ray's direction
        }
    }
}

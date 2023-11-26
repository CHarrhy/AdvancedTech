using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 1f;
    public int maxAmmo = 5;
    public float reloadTime = 2f;
    public Camera fpsCamera;
    //public ParticleSystem muzzleFlash;
    public GameObject bulletPrefab; // Reference to the bullet prefab

    private int currentAmmo;
    private bool isReloading = false;
    private float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        //muzzleFlash.Play(); // Play muzzle flash particle effect
        currentAmmo--;
        Debug.Log("Ammo: " + currentAmmo);
        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * range, Color.red, 0.1f);

        int enemyLayerMask = LayerMask.GetMask("Enemy"); // Adjust to your enemy layer
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            // Handle hit target
            Debug.Log("Hit: " + hit.transform.name);

            // Apply damage to the target (you might want to handle this differently based on your game)
            Health targetHealth = hit.transform.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }
        }
        else
        {
            // Debug information if the shot didn't hit anything
            Debug.Log("Shot fired, but didn't hit anything.");
        }

        // Instantiate and shoot the bullet
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
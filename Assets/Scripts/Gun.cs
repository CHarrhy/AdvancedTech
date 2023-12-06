using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damage = 100f;
    public float range = 100f;
    public float fireRate = 0.1f;
    public int maxAmmo = 5;
    public float reloadTime = 2f;
    public Camera fpsCamera;
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public GameObject muzzleFlashPrefab; // Reference to the muzzle flash prefab

    // Parameters for muzzle flash position and rotation
    public Vector3 muzzleFlashPositionOffset = Vector3.zero;
    public Vector3 muzzleFlashRotationOffset = Vector3.zero;

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
        // Play shooting animation (removed for now)
        // gunAnimator.SetTrigger("Shoot");

        // Instantiate and play muzzle flash with user-defined position and rotation offsets
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, transform.position + muzzleFlashPositionOffset, Quaternion.Euler(transform.eulerAngles + muzzleFlashRotationOffset));

        // Destroy muzzle flash after 1 second
        Destroy(muzzleFlash, 1f);

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
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2f);
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

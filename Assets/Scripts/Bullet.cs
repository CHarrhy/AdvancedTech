using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    public float lifetime = 2f; // Time until the bullet is destroyed if it doesn't hit anything

    void Start()
    {
        // Set a lifetime for the bullet
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the bullet forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with a specific layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) // Adjust to the layer of your enemies
        {
            Debug.Log("Bullet hit enemy: " + other.gameObject.name);

            // Optionally handle collision with other objects (e.g., damage enemies)
            // Make sure to set up colliders on objects you want the bullets to interact with
            Health targetHealth = other.GetComponent<Health>();
            if (targetHealth != null)
            {
                // Replace  with the actual damage value you want to apply
                targetHealth.TakeDamage(100);
            }
        }

        // Destroy the bullet on collision
        Destroy(gameObject);
    }
}


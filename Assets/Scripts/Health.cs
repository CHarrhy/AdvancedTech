using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float healthRegenRate = 5f;
    public float timeToRegenAfterDamage = 5f;

    private float currentHealth;
    private float timeSinceLastDamage;

    void Start()
    {
        currentHealth = maxHealth;
        timeSinceLastDamage = Time.time;
    }

    void Update()
    {
        // Check if enough time has passed since the last damage to start health regeneration
        if (Time.time - timeSinceLastDamage > timeToRegenAfterDamage)
        {
            RegenerateHealth();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        timeSinceLastDamage = Time.time;
        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void RegenerateHealth()
    {
        // Increase current health over time
        currentHealth = Mathf.Min(currentHealth + (healthRegenRate * Time.deltaTime), maxHealth);
    }

    void Die()
    {
        // Perform actions when the object dies, e.g., play death animation, destroy object, etc.
        Destroy(gameObject);
    }
}
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            Destroy(gameObject);
        }
    }

    void Die()
    {
        // You can add death effects, animations, or other actions here.
        // In the context of your current EnemyPatrol script, you can disable or destroy the enemy GameObject.
        // For example, you can use gameObject.SetActive(false) to disable it, or Destroy(gameObject) to destroy it.
        // You may also handle other behaviors upon the enemy's death here.
    }
}

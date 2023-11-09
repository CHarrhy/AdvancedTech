using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Transform respawnPoint;
    public Text healthText;

    void Start()
    {
        currentHealth = maxHealth;
        //UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
        //UpdateHealthUI();
    }

    void Die()
    {
        // You can add death effects, animations, or other actions here.

        // Respawn the player.
        Respawn();
    }

    void Respawn()
    {
        currentHealth = maxHealth;
        //UpdateHealthUI();
        transform.position = respawnPoint.position;
        // You can also reset the player's rotation here if needed.
    }

    void UpdateHealthUI()
    {
        //healthText.text = "Health: " + currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}

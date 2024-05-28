using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Include the UI namespace

public class BossHP : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health points of the boss
    private int currentHealth; // Current health points of the boss

    public GameObject nextLevelObject; // Object to activate after the boss is destroyed
    public Animator animator; // Animator component for the boss
    public SpriteRenderer spriteRenderer; // SpriteRenderer component for the boss
    public float flashDuration = 0.1f; // Duration for each flash
    public int numberOfFlashes = 5; // Number of flashes
    public Text healthText; // UI Text component to display the health

    private void Start()
    {
        currentHealth = maxHealth; // Set the initial health points
        if (animator == null)
        {
            animator = GetComponent<Animator>(); // Get the Animator component if not assigned
        }
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component if not assigned
        }
        UpdateHealthUI(); // Initialize the health display
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Kula"))
        {
            animator.SetTrigger("Hurt"); // Trigger the hurt animation
            TakeDamage(1); // Reduce health by 1
        }
    }

    // Method to reduce the boss's health points
    public void TakeDamage(int amount)
    {
        currentHealth -= amount; // Reduce the health points
        UpdateHealthUI(); // Update the health display
        StartCoroutine(FlashSprite()); // Start the flashing coroutine

        // Check health thresholds and trigger appropriate methods
        if (currentHealth == 50%maxHealth)
        {
            Transform(); // Call the Transform method if health points drop to 5
        }
        else if (currentHealth <= 0)
        {
            Die(); // Call the Die method if health points drop to zero or below
        }
    }

    // Coroutine to flash the sprite
    private IEnumerator FlashSprite()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.enabled = false; // Disable the sprite renderer
            yield return new WaitForSeconds(flashDuration); // Wait for the flash duration
            spriteRenderer.enabled = true; // Enable the sprite renderer
            yield return new WaitForSeconds(flashDuration); // Wait for the flash duration
        }
    }

    // Method to handle the boss's transformation
    private void Transform()
    {
        animator.SetTrigger("Transform"); // Trigger the transformation animation
        // Optionally, handle any other transformation logic here
    }

    // Method to handle the boss's death
    private void Die()
    {
        animator.SetTrigger("Die"); // Trigger the death animation (if you have one)
        gameObject.SetActive(false); // Deactivate the boss object
        if (nextLevelObject != null)
        {
            nextLevelObject.SetActive(true); // Activate the object for the next level
        }
    }

    // Method to update the health display
    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Dark Fairy HP: " + currentHealth; // Update the text with the current health
        }
    }
}


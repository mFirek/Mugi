using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHPWugi : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health points of the boss
    private int currentHealth; // Current health points of the boss

    public GameObject nextLevelObject; // Object to activate after the boss is destroyed
    public Animator animator; // Animator component for the boss
    public SpriteRenderer spriteRenderer; // SpriteRenderer component for the boss
    public float flashDuration = 0.1f; // Duration for each flash
    public int numberOfFlashes = 5; // Number of flashes
    public TextMeshProUGUI healthText; // UI Text component to display the health

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Collider2D[] colliders; // Array to hold all Collider2D components

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

        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        colliders = GetComponents<Collider2D>(); // Get all Collider2D components attached to the game object

        UpdateHealthUI(); // Initialize the health display
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Kula"))
        {
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
        if (currentHealth == maxHealth / 2)
        {
            Transform(); // Call the Transform method if health points drop to 50% of max health
        }
        else if (currentHealth <= 0)
        {

            StartCoroutine(Die()); // Call the Die method if health points drop to zero or below
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
    }

    // Coroutine to handle the boss's death
    // Coroutine to handle the boss's death
    private IEnumerator Die()
    {
        Debug.Log("Boss is dying"); // Log message to debug

        // Disable Rigidbody2D to stop all movement
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Natychmiast zatrzymujemy ruch bossa
            rb.simulated = false; // Wy³¹czamy symulacjê fizyki, aby boss nie móg³ siê poruszaæ
        }

        // Disable all Collider2D components so the boss no longer interacts with the world
        foreach (var collider in colliders)
        {
            collider.enabled = false; // Wy³¹czamy ka¿dy Collider2D
        }

        // Disable the WugiBoss script to stop movement and attacks
        var movementScript = GetComponent<WugiBoss>(); // ZnajdŸ skrypt WugiBoss
        if (movementScript != null)
        {
            movementScript.enabled = false; // Wy³¹czamy skrypt odpowiedzialny za ruch i ataki
        }

        // Trigger death animation
        animator.SetTrigger("Die"); // Wyzwalamy animacjê œmierci

        // Wait until the death animation is complete (handled by Animation Event)
        yield return null; // No need to wait in the script; Animation Event will handle destruction
    }


    // Method to be called by Animation Event to remove the boss object
    public void RemoveBoss()
    {
        Destroy(gameObject); // Destroy the boss object

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
            healthText.text = "Wugi HP: " + currentHealth; // Update the text with the current health
        }
    }
}

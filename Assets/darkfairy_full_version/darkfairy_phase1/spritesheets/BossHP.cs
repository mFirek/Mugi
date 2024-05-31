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
        // Optionally, handle any other transformation logic here
    }

    // Coroutine to handle the boss's death
    private IEnumerator Die()
    {
        Debug.Log("Boss is dying"); // Log message to debug

        // Disable the Rigidbody2D and all Collider2D components
        if (rb != null)
        {
            rb.simulated = false; // Disable the Rigidbody2D component
        }

        foreach (var collider in colliders)
        {
            collider.enabled = false; // Disable each Collider2D component
        }

        animator.SetTrigger("Die"); // Trigger the death animation

        // Wait until the death animation is complete
        float animationLength = GetAnimationLength(animator, "Die");
        if (animationLength > 0)
        {
            yield return new WaitForSeconds(animationLength);
        }
        else
        {
            Debug.LogWarning("Death animation length is 0 or not found");
            yield return new WaitForSeconds(1); // Default wait time if animation length is not found
        }

        // Deactivate the boss object
        Destroy(gameObject);

        if (nextLevelObject != null)
        {
            nextLevelObject.SetActive(true); // Activate the object for the next level
        }
    }

    // Method to get the length of an animation clip
    private float GetAnimationLength(Animator animator, string clipName)
    {
        if (animator != null)
        {
            // Get the duration of the animation
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == clipName)
                {
                    return clip.length;
                }
            }
        }
        Debug.LogWarning($"Animation clip {clipName} not found in animator");
        return 0f;
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

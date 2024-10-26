using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHP : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    public GameObject nextLevelObject;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;
    public int numberOfFlashes = 5;

    public TextMeshProUGUI healthText;
    public Slider healthSlider; // Pasek zdrowia
    public GameObject healthBarUI; // UI paska zdrowia

    private Rigidbody2D rb;
    private Collider2D[] colliders;

    public AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.GetInstance();

        currentHealth = maxHealth;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();

        // Inicjalizacja UI paska zdrowia
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthBarUI.SetActive(true);

        UpdateHealthUI();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Kula"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UpdateHealthUI();
        StartCoroutine(FlashSprite());

        if (currentHealth == maxHealth / 2)
        {
            Transform();
        }
        else if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator FlashSprite()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void Transform()
    {
        animator.SetTrigger("Transform");
    }

    private IEnumerator Die()
    {
        Debug.Log("Boss is dying");

        audioManager.PlaySFX(audioManager.bossDefeat);

        if (rb != null)
        {
            rb.simulated = false;
        }

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        animator.SetTrigger("Die");

        float animationLength = GetAnimationLength(animator, "Die");
        if (animationLength > 0)
        {
            yield return new WaitForSeconds(animationLength);
        }
        else
        {
            Debug.LogWarning("Death animation length is 0 or not found");
            yield return new WaitForSeconds(1);
        }

        Destroy(gameObject);
        healthBarUI.SetActive(false); // Wy³¹czenie paska zdrowia po œmierci bossa

        if (nextLevelObject != null)
        {
            nextLevelObject.SetActive(true);
        }
    }

    private float GetAnimationLength(Animator animator, string clipName)
    {
        if (animator != null)
        {
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

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Boss HP: " + currentHealth;
        }
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }
}

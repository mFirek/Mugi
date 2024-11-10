using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Golem_Death : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D[] colliders;
    bool isDying = false;
    public GameObject nextLevelObject;

    public float blinkDuration = 0.5f;

    public Boss_Golem boss;

    public Color blinkColor = Color.white;

    public TextMeshProUGUI healthText;

    public int maxHealth = 10;
    private int currentHealth;

    public Slider healthSlider; // Pasek zdrowia
    public GameObject healthBarUI; // UI paska zdrowia

    public AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.GetInstance();

        rb = GetComponentInChildren<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();

        boss = GetComponent<Boss_Golem>();

        currentHealth = maxHealth;

        // Inicjalizacja UI paska zdrowia
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthBarUI.SetActive(true);

        UpdateHealthUI();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Kula") && !isDying)
        {
            HandleCollisionWithProjectile(collision);
        }
    }

    void HandleCollisionWithProjectile(Collider2D collision)
    {
        if (boss != null && boss.isImmune)
        {
            StartCoroutine(BlinkOnHit());
            return;
        }

        currentHealth--;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            HandleBossDeath();
        }
        else
        {
            StartCoroutine(BlinkOnHit());
        }
    }

    void HandleBossDeath()
    {
        Animator animator = GetComponentInChildren<Animator>();
        if (animator == null || !HasDeathAnimation(animator))
        {
            StartCoroutine(BlinkAndDestroy());
        }
        else
        {
            DestroyEnemy();
        }
    }

    IEnumerator BlinkOnHit()
    {
        float blinkTime = 0.1f;
        Color originalColor = Color.clear;
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            originalColor = renderer.material.color;
        }

        for (int i = 0; i < 3; i++)
        {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = blinkColor;
            }
            yield return new WaitForSeconds(blinkTime);

            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = originalColor;
            }
            yield return new WaitForSeconds(blinkTime);
        }
    }

    IEnumerator BlinkAndDestroy()
    {
        isDying = true;
        rb.velocity = Vector2.zero;

        float timer = 0f;
        while (timer < blinkDuration)
        {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = blinkColor;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = Color.clear;
            }

            yield return new WaitForSeconds(0.1f);
            timer += 0.2f;
        }

        gameObject.SetActive(false);
    }

    void DestroyEnemy()
    {
        Animator animator = GetComponentInChildren<Animator>();

        audioManager.PlaySFX(audioManager.bossDefeat);

        if (animator != null && HasDeathAnimation(animator))
        {
            if (rb != null)
            {
                rb.simulated = false;
            }

            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }

            animator.SetTrigger("Death");
            Destroy(gameObject, GetAnimationLength(animator, "Death"));
        }
        else
        {
            Destroy(gameObject);
        }

        healthBarUI.SetActive(false); // Wy³¹czenie paska zdrowia po œmierci

        if (nextLevelObject != null)
        {
            nextLevelObject.SetActive(true);
        }
        PlayerBossAnalytics.Instance.SendBossDefeatEvent();
    }

    bool HasDeathAnimation(Animator animator)
    {
        if (animator != null)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "Death")
                {
                    return true;
                }
            }
        }
        return false;
    }

    float GetAnimationLength(Animator animator, string triggerName)
    {
        if (animator != null)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == triggerName)
                {
                    return clip.length;
                }
            }
        }
        return 0f;
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Golem HP: " + currentHealth;
        }
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }
}

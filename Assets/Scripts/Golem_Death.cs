using UnityEngine;
using System.Collections;
using TMPro;

public class Golem_Death : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D[] colliders;
    bool isDying = false;
    public GameObject nextLevelObject;

    // Czas trwania migania postaci
    public float blinkDuration = 0.5f;

    // Odniesienie do skryptu Boss_Golem
    public Boss_Golem boss;

    // Kolor migania
    public Color blinkColor = Color.white;

    // Zmiana na TextMeshProUGUI
    public TextMeshProUGUI healthText;

    // Nowe zmienne zdrowia
    public int maxHealth = 10;
    private int currentHealth;

    // Odniesienie do AudioManager
    public AudioManager audioManager;
    void Start()
    {
        audioManager = AudioManager.GetInstance();

        rb = GetComponentInChildren<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();

        boss = GetComponent<Boss_Golem>();

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Kula") && !isDying)
        {
            HandleCollisionWithProjectile(collision);
        }
    }

    // Nowa metoda do obs≥ugi kolizji z pociskiem
    void HandleCollisionWithProjectile(Collider2D collision)
    {
        // Zniszcz pocisk


        // Jeúli boss jest odporny, migaj i wyjdü z metody
        if (boss != null && boss.isImmune)
        {
            StartCoroutine(BlinkOnHit());
            return;
        }

        // Zmniejsz zdrowie bossa
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

    // Nowa metoda do obs≥ugi úmierci bossa
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

        if (nextLevelObject != null)
        {
            nextLevelObject.SetActive(true);
        }
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
        healthText.text = "Golem HP: " + currentHealth;
    }
}

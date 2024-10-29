using UnityEngine;
using System.Collections;

public class Enemies_Death : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D[] colliders;
    bool isDying = false;

    private int collisionCount = 0; // Licznik zderzeñ z obiektem o tagu "Kula"
    public int maxCollisions = 3; // Maksymalna liczba zderzeñ przed znikniêciem obiektu

    public float blinkDuration = 0.5f; // Czas trwania migania postaci
    public EnemyAI enemyAI; // Skrypt "Enemy AI", który chcemy wy³¹czyæ podczas migania i po znikniêciu obiektu

    public Color blinkColor = Color.white; // Kolor migania

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDying && collision.CompareTag("Kula"))
        {
            // Logowanie kolizji i aktualnej liczby zderzeñ
            Debug.Log("Obiekt zderzy³ siê z 'Kula'. Liczba kolizji: " + (collisionCount + 1));

            Destroy(collision.gameObject); // Zniszcz pocisk

            collisionCount++;

            if (collisionCount >= maxCollisions)
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
            else
            {
                StartCoroutine(BlinkOnHit());
            }
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

        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

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
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        Animator animator = GetComponentInChildren<Animator>();

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
}

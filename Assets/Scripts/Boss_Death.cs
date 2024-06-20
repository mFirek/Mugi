using UnityEngine;
using System.Collections;

public class Boss_Death : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D[] colliders;
    bool isDying = false;

    // Licznik zderzeñ z obiektem o tagu "Kula"
    private int collisionCount = 0;

    // Maksymalna liczba zderzeñ przed znikniêciem obiektu
    public int maxCollisions = 3;

    // Czas trwania migania postaci
    public float blinkDuration = 0.5f;

    // Skrypt "Enemy AI", który chcemy wy³¹czyæ podczas migania i po znikniêciu obiektu
    public EnemyAI enemyAI;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDying && collision.gameObject.CompareTag("Kula"))
        {
            // Zniszcz pocisk
            Destroy(collision.gameObject);

            // Zwiêksz licznik zderzeñ
            collisionCount++;

            // Jeœli liczba zderzeñ przekroczy³a limit, zniszcz obiekt
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
        }
    }

    IEnumerator BlinkAndDestroy()
    {
        isDying = true;

        // Wy³¹cz skrypt "Enemy AI"
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        // Zatrzymaj ruch obiektu
        rb.velocity = Vector2.zero;

        // Zapêtlenie migania przez okreœlony czas
        float timer = 0f;
        while (timer < blinkDuration)
        {
            // Zmieñ kolor wszystkich renderów na bia³y
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = Color.white;
            }

            // Czekaj krótk¹ chwilê
            yield return new WaitForSeconds(0.1f);

            // Zmieñ kolor wszystkich renderów na pierwotny
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = Color.clear; // Tutaj zmieni³em na Color.clear, ale mo¿esz u¿yæ koloru pierwotnego
            }

            // Czekaj krótk¹ chwilê
            yield return new WaitForSeconds(0.1f);

            // Aktualizuj czas
            timer += 0.2f;
        }

        // Wy³¹cz postaæ po zakoñczeniu migania
        gameObject.SetActive(false);
    }

    void DestroyEnemy()
    {
        // Wy³¹cz skrypt "Enemy AI"
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        Animator animator = GetComponentInChildren<Animator>();

        if (animator != null && HasDeathAnimation(animator))
        {
            // Wy³¹cz fizykê obiektu
            if (rb != null)
            {
                rb.simulated = false;
            }

            // Wy³¹cz collidery obiektu
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }

            // Uruchom animacjê œmierci
            animator.SetTrigger("Death");

            // Zniszcz obiekt po zakoñczeniu animacji
            Destroy(gameObject, GetAnimationLength(animator, "Death"));
        }
        else
        {
            // Jeœli nie znaleziono animatora lub animacji "Death", zniszcz obiekt natychmiast
            Destroy(gameObject);
        }
    }

    bool HasDeathAnimation(Animator animator)
    {
        if (animator != null)
        {
            // SprawdŸ, czy animacja "Death" istnieje
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
            // Pobierz czas trwania animacji
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

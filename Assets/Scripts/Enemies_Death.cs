using UnityEngine;
using System.Collections;

public class Enemies_Death : MonoBehaviour
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

    // Odniesienie do skryptu Boss_Golem
    public Boss_Golem boss;

    // Kolor migania
    public Color blinkColor = Color.white;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();

        // ZnajdŸ skrypt Boss_Golem na obiekcie
        boss = GetComponent<Boss_Golem>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDying && collision.CompareTag("Kula"))
        {
            // Zniszcz pocisk
            Destroy(collision.gameObject);

            // SprawdŸ, czy boss jest odporny
            if (boss != null && boss.isImmune)
            {
                StartCoroutine(BlinkOnHit()); // Miga na wybrany kolor, jeœli boss jest odporny
                return; // WyjdŸ z metody bez zmniejszania liczby kolizji
            }

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
            else
            {
                StartCoroutine(BlinkOnHit()); // Miga na wybrany kolor, gdy otrzymuje obra¿enia
            }
        }
    }

    IEnumerator BlinkOnHit()
    {
        float blinkTime = 0.1f;
        Color originalColor = Color.clear; // Zak³adamy, ¿e pierwotny kolor to Color.clear
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            originalColor = renderer.material.color;
        }

        // Miga na wybrany kolor
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
            // Zmieñ kolor wszystkich renderów na wybrany kolor
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = blinkColor;
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

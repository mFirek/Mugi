using UnityEngine;
using System.Collections;
using TMPro;  // Dodaj to, aby uzyska� dost�p do TextMeshPro

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
    public TextMeshProUGUI healthText; // Zmiana z Text na TextMeshProUGUI

    // Nowe zmienne zdrowia
    public int maxHealth = 10; // Maksymalne zdrowie bossa
    private int currentHealth;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();

        // Znajd� skrypt Boss_Golem na obiekcie
        boss = GetComponent<Boss_Golem>();

        // Ustawienie pocz�tkowego zdrowia
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDying && collision.CompareTag("Kula"))
        {
            // Zniszcz pocisk
            Destroy(collision.gameObject);

            // Sprawd�, czy boss jest odporny
            if (boss != null && boss.isImmune)
            {
                StartCoroutine(BlinkOnHit()); // Miga na wybrany kolor, je�li boss jest odporny
                return; // Wyjd� z metody bez zmniejszania zdrowia
            }

            // Zmniejsz zdrowie bossa
            currentHealth--;

            // Zaktualizuj UI zdrowia
            UpdateHealthUI();

            // Je�li zdrowie bossa spad�o do zera, zniszcz obiekt
            if (currentHealth <= 0)
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
                StartCoroutine(BlinkOnHit()); // Miga na wybrany kolor, gdy otrzymuje obra�enia
            }
        }
    }

    IEnumerator BlinkOnHit()
    {
        float blinkTime = 0.1f;
        Color originalColor = Color.clear; // Zak�adamy, �e pierwotny kolor to Color.clear
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

        // Zatrzymaj ruch obiektu
        rb.velocity = Vector2.zero;

        // Zap�tlenie migania przez okre�lony czas
        float timer = 0f;
        while (timer < blinkDuration)
        {
            // Zmie� kolor wszystkich render�w na wybrany kolor
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = blinkColor;
            }

            // Czekaj kr�tk� chwil�
            yield return new WaitForSeconds(0.1f);

            // Zmie� kolor wszystkich render�w na pierwotny
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = Color.clear; // Tutaj zmieni�em na Color.clear, ale mo�esz u�y� koloru pierwotnego
            }

            // Czekaj kr�tk� chwil�
            yield return new WaitForSeconds(0.1f);

            // Aktualizuj czas
            timer += 0.2f;
        }

        // Wy��cz posta� po zako�czeniu migania
        gameObject.SetActive(false);
    }

    void DestroyEnemy()
    {
        Animator animator = GetComponentInChildren<Animator>();

        if (animator != null && HasDeathAnimation(animator))
        {
            // Wy��cz fizyk� obiektu
            if (rb != null)
            {
                rb.simulated = false;
            }

            // Wy��cz collidery obiektu
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }

            // Uruchom animacj� �mierci
            animator.SetTrigger("Death");

            // Zniszcz obiekt po zako�czeniu animacji
            Destroy(gameObject, GetAnimationLength(animator, "Death"));
        }
        else
        {
            // Je�li nie znaleziono animatora lub animacji "Death", zniszcz obiekt natychmiast
            Destroy(gameObject);
        }
        if (nextLevelObject != null)
        {
            nextLevelObject.SetActive(true); // Activate the object for the next level
        }
    }

    bool HasDeathAnimation(Animator animator)
    {
        if (animator != null)
        {
            // Sprawd�, czy animacja "Death" istnieje
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

    private void UpdateHealthUI()
    {
        healthText.text = "Golem HP: " + currentHealth;
    }
}

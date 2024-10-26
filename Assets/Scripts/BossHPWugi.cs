using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHPWugi : MonoBehaviour
{
    public int maxHealth = 10; // Maksymalne zdrowie bossa
    private int currentHealth; // Aktualne zdrowie bossa

    public GameObject nextLevelObject; // Obiekt do aktywacji po zniszczeniu bossa
    public Animator animator; // Komponent Animatora bossa
    public SpriteRenderer spriteRenderer; // Komponent SpriteRenderer bossa
    public float flashDuration = 0.1f; // Czas trwania ka¿dego migniêcia
    public int numberOfFlashes = 5; // Liczba migniêæ
    public TextMeshProUGUI healthText; // Tekst UI do wyœwietlania zdrowia
    public Slider healthSlider; // Pasek zdrowia bossa
    public GameObject healthBarUI; // Ca³y UI paska zdrowia

    private Rigidbody2D rb; // Odpowiednik Rigidbody2D
    private Collider2D[] colliders; // Tablica do przechowywania wszystkich Collider2D

    public AudioManager audioManager;

    private void Start()
    {
        currentHealth = maxHealth; // Ustawienie pocz¹tkowego zdrowia
        healthSlider.maxValue = maxHealth; // Ustaw maksymaln¹ wartoœæ paska zdrowia
        healthSlider.value = currentHealth; // Ustaw pocz¹tkow¹ wartoœæ paska zdrowia

        audioManager = AudioManager.GetInstance();

        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();

        healthBarUI.SetActive(true); // W³¹cz UI paska zdrowia na pocz¹tku walki
        UpdateHealthUI();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Kula"))
        {
            TakeDamage(1); // Zmniejsz zdrowie o 1
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UpdateHealthUI();
        StartCoroutine(FlashSprite());

        if (currentHealth == maxHealth / 2) Transform();
        else if (currentHealth <= 0) StartCoroutine(Die());
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
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        foreach (var collider in colliders) collider.enabled = false;

        var movementScript = GetComponent<WugiBoss>();
        if (movementScript != null) movementScript.enabled = false;

        animator.SetTrigger("Die");

        yield return new WaitForSeconds(1f); // Czekaj na zakoñczenie animacji œmierci

        healthBarUI.SetActive(false); // Wy³¹cz pasek zdrowia po œmierci bossa
    }

    public void RemoveBoss()
    {
        Destroy(gameObject);
        if (nextLevelObject != null) nextLevelObject.SetActive(true);
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Wugi HP: " + currentHealth;
        }
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }
}

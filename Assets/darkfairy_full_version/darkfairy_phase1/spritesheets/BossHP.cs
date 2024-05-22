using UnityEngine;

public class BossHP : MonoBehaviour
{
    public int maxHealth = 10; // Maksymalna iloœæ punktów ¿ycia bossa
    private int currentHealth; // Bie¿¹ca iloœæ punktów ¿ycia bossa

    public GameObject nextLevelObject; // Obiekt do aktywacji po zniszczeniu bossa

    private void Start()
    {
        currentHealth = maxHealth; // Ustawienie pocz¹tkowej iloœci punktów ¿ycia
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Kula"))
        {
            TakeDamage(1);
        }
    }

    // Metoda do zmniejszania punktów ¿ycia bossa
    public void TakeDamage(int amount)
    {
        currentHealth -= amount; // Zmniejszenie iloœci punktów ¿ycia
        if (currentHealth <= 0)
        {
            Die(); // Jeœli iloœæ punktów ¿ycia spadnie do zera, wywo³ujemy metodê Die
        }
    }

    // Metoda do obs³ugi znikniêcia bossa
    private void Die()
    {
        gameObject.SetActive(false); // Deaktywacja obiektu bossa
        if (nextLevelObject != null)
        {
            nextLevelObject.SetActive(true); // Aktywacja obiektu do przejœcia na kolejny poziom
        }
    }
}

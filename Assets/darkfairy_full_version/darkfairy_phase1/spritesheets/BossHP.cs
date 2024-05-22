using UnityEngine;

public class BossHP : MonoBehaviour
{
    public int maxHealth = 10; // Maksymalna ilo�� punkt�w �ycia bossa
    private int currentHealth; // Bie��ca ilo�� punkt�w �ycia bossa

    public GameObject nextLevelObject; // Obiekt do aktywacji po zniszczeniu bossa

    private void Start()
    {
        currentHealth = maxHealth; // Ustawienie pocz�tkowej ilo�ci punkt�w �ycia
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Kula"))
        {
            TakeDamage(1);
        }
    }

    // Metoda do zmniejszania punkt�w �ycia bossa
    public void TakeDamage(int amount)
    {
        currentHealth -= amount; // Zmniejszenie ilo�ci punkt�w �ycia
        if (currentHealth <= 0)
        {
            Die(); // Je�li ilo�� punkt�w �ycia spadnie do zera, wywo�ujemy metod� Die
        }
    }

    // Metoda do obs�ugi znikni�cia bossa
    private void Die()
    {
        gameObject.SetActive(false); // Deaktywacja obiektu bossa
        if (nextLevelObject != null)
        {
            nextLevelObject.SetActive(true); // Aktywacja obiektu do przej�cia na kolejny poziom
        }
    }
}

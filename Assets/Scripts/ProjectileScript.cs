using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 10f; // Pr�dko�� pocisku
    public int damage = 1; // Obra�enia zadawane przez pocisk

    private Rigidbody2D rb; // Komponent Rigidbody pocisku

    void Start()
    {
        // Pobierz komponent Rigidbody pocisku
        rb = GetComponent<Rigidbody2D>();
        // Wy��cz SpriteRenderer na pocz�tku
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Metoda ustawiaj�ca kierunek ruchu pocisku
    public void SetDirection(Vector3 direction)
    {
        // Je�li chcesz, mo�esz zmodyfikowa� ten kod, aby nada� kierunek pociskowi
        // Na przyk�ad mo�na u�y� tego kierunku, aby nada� pociskowi pr�dko�� w odpowiednim kierunku
        // Ta implementacja zak�ada, �e kierunek ruchu pocisku jest znormalizowany
        rb.velocity = direction.normalized * speed;
    }

    // Metoda wywo�ywana, gdy pocisk trafia w co�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tutaj mo�na doda� kod obs�ugi zderze�, np. sprawdzanie, czy pocisk trafia w gracza lub inne obiekty
        // W tym przyk�adzie, po trafieniu w obiekt, pocisk po prostu si� niszczy
        GetComponent<SpriteRenderer>().enabled = false; // Wy��cz SpriteRenderer
        Destroy(gameObject); // Zniszcz pocisk
    }
}

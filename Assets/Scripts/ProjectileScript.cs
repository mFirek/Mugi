using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 10f; // Prêdkoœæ pocisku
    public int damage = 1; // Obra¿enia zadawane przez pocisk

    private Rigidbody2D rb; // Komponent Rigidbody pocisku

    void Start()
    {
        // Pobierz komponent Rigidbody pocisku
        rb = GetComponent<Rigidbody2D>();
        // Wy³¹cz SpriteRenderer na pocz¹tku
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Metoda ustawiaj¹ca kierunek ruchu pocisku
    public void SetDirection(Vector3 direction)
    {
        // Jeœli chcesz, mo¿esz zmodyfikowaæ ten kod, aby nadaæ kierunek pociskowi
        // Na przyk³ad mo¿na u¿yæ tego kierunku, aby nadaæ pociskowi prêdkoœæ w odpowiednim kierunku
        // Ta implementacja zak³ada, ¿e kierunek ruchu pocisku jest znormalizowany
        rb.velocity = direction.normalized * speed;
    }

    // Metoda wywo³ywana, gdy pocisk trafia w coœ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tutaj mo¿na dodaæ kod obs³ugi zderzeñ, np. sprawdzanie, czy pocisk trafia w gracza lub inne obiekty
        // W tym przyk³adzie, po trafieniu w obiekt, pocisk po prostu siê niszczy
        GetComponent<SpriteRenderer>().enabled = false; // Wy³¹cz SpriteRenderer
        Destroy(gameObject); // Zniszcz pocisk
    }
}

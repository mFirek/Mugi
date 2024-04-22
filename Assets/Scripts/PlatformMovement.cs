using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    // Nowa warto�� skali grawitacji
    public float newGravityScale = 1f;

    // Metoda wywo�ywana, gdy inny obiekt wchodzi w obszar wykrywania kolizji
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Sprawd�, czy obiekt, kt�ry wszed� w obszar, to gracz
        if (collision.CompareTag("Player"))
        {
            // Pobierz komponent Rigidbody2D obiektu gracza
            Rigidbody2D playerRigidbody = collision.GetComponent<Rigidbody2D>();

            // Je�li Rigidbody2D zosta�o znalezione
            if (playerRigidbody != null)
            {
                // Zmiana skali grawitacji
                playerRigidbody.gravityScale = newGravityScale;
            }

            // Wy��cz collider platformy
         
        }
        // Sprawd�, czy obiekt, kt�ry wszed� w obszar, ma tag "teren"
        else if (collision.CompareTag("Teren"))
        {
            // Zniszcz obiekt platformy
            Destroy(gameObject);
        }
    }
}

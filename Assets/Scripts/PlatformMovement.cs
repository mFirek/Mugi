using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    // Nowa wartoœæ skali grawitacji
    public float newGravityScale = 1f;

    // Metoda wywo³ywana, gdy inny obiekt wchodzi w obszar wykrywania kolizji
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // SprawdŸ, czy obiekt, który wszed³ w obszar, to gracz
        if (collision.CompareTag("Player"))
        {
            // Pobierz komponent Rigidbody2D obiektu gracza
            Rigidbody2D playerRigidbody = collision.GetComponent<Rigidbody2D>();

            // Jeœli Rigidbody2D zosta³o znalezione
            if (playerRigidbody != null)
            {
                // Zmiana skali grawitacji
                playerRigidbody.gravityScale = newGravityScale;
            }

            // Wy³¹cz collider platformy
         
        }
        // SprawdŸ, czy obiekt, który wszed³ w obszar, ma tag "teren"
        else if (collision.CompareTag("Teren"))
        {
            // Zniszcz obiekt platformy
            Destroy(gameObject);
        }
    }
}

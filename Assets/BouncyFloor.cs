using UnityEngine;

public class BouncyFloor : MonoBehaviour
{
    public float bounceForce = 20f; // Si³a wybicia gracza w górê

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Sprawdzamy, czy obiekt, który dotkn¹³ pod³ogi, ma tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Pobieramy Rigidbody2D gracza
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Nadajemy graczowi si³ê w górê
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
            }
        }
    }
}

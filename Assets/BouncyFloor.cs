using UnityEngine;

public class BouncyFloor : MonoBehaviour
{
    public float bounceForce = 20f; // Si�a wybicia gracza w g�r�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Sprawdzamy, czy obiekt, kt�ry dotkn�� pod�ogi, ma tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Pobieramy Rigidbody2D gracza
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Nadajemy graczowi si�� w g�r�
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);
            }
        }
    }
}

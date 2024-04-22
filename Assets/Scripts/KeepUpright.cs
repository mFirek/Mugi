using UnityEngine;

public class KeepUpright : MonoBehaviour
{
    // Referencja do Rigidbody obiektu, którego chcemy utrzymaæ w pozycji odwróconej
    public Rigidbody2D rb;

    // Referencja do SpriteRenderer obiektu
    public SpriteRenderer spriteRenderer;
    public float newGravity = 1f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdzamy, czy obiekt wszed³ w trigger
        if (other.CompareTag("Player"))
        {
            // Sprawdzamy kierunek grawitacji
            Vector2 gravityDirection = Physics2D.gravity.normalized;
            rb.gravityScale = newGravity;
            // Jeœli grawitacja skierowana jest w dó³ (grawitacja = 1), to obracamy obiekt w odpowiedni sposób
            if (gravityDirection == Vector2.down)
            {
              
                spriteRenderer.flipY = false; // Zmiana na false
            }
        }
    }
}

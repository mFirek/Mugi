using UnityEngine;

public class KeepUpright : MonoBehaviour
{
    // Referencja do Rigidbody obiektu, kt�rego chcemy utrzyma� w pozycji odwr�conej
    public Rigidbody2D rb;

    // Referencja do SpriteRenderer obiektu
    public SpriteRenderer spriteRenderer;
    public float newGravity = 1f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdzamy, czy obiekt wszed� w trigger
        if (other.CompareTag("Player"))
        {
            // Sprawdzamy kierunek grawitacji
            Vector2 gravityDirection = Physics2D.gravity.normalized;
            rb.gravityScale = newGravity;
            // Je�li grawitacja skierowana jest w d� (grawitacja = 1), to obracamy obiekt w odpowiedni spos�b
            if (gravityDirection == Vector2.down)
            {
              
                spriteRenderer.flipY = false; // Zmiana na false
            }
        }
    }
}

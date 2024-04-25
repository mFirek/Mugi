using UnityEngine;

public class ChangeGravity : MonoBehaviour
{
    // Referencja do Rigidbody obiektu, którego grawitacjê chcemy zmieniæ
    public Rigidbody2D rb;

    // Referencja do SpriteRenderer obiektu, którego sprite'a chcemy odwróciæ
    public SpriteRenderer spriteRenderer;

    // Grawitacja, któr¹ chcemy ustawiæ po wejœciu w trigger
    public float newGravity = -1f;

    // Czy sprite jest obecnie odwrócony
    private bool flipped = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdzamy, czy obiekt, który wszed³ w trigger, to ten, którego szukamy
        if (other.CompareTag("Player"))
        {
            // Jeœli tak, zmieniamy grawitacjê
            rb.gravityScale = newGravity;

            // Odwracamy sprite'a w pionie
            FlipSprite();
        }
    }

    // Metoda do odwrócenia sprite'a w pionie
    private void FlipSprite()
    {
        // Odwracamy sprite'a w pionie
        spriteRenderer.flipY = !spriteRenderer.flipY;
        // Zapisujemy informacjê o aktualnym stanie odwrócenia
        flipped = !flipped;
    }

    // Pamiêtaj, aby ustawiæ flipped na false na pocz¹tku, jeœli sprite na starcie jest w normalnej orientacji
    private void Start()
    {
        flipped = false;
    }
}

using UnityEngine;

public class ChangeGravity : MonoBehaviour
{
    // Referencja do Rigidbody obiektu, kt�rego grawitacj� chcemy zmieni�
    public Rigidbody2D rb;

    // Referencja do SpriteRenderer obiektu, kt�rego sprite'a chcemy odwr�ci�
    public SpriteRenderer spriteRenderer;

    // Grawitacja, kt�r� chcemy ustawi� po wej�ciu w trigger
    public float newGravity = -1f;

    // Czy sprite jest obecnie odwr�cony
    private bool flipped = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdzamy, czy obiekt, kt�ry wszed� w trigger, to ten, kt�rego szukamy
        if (other.CompareTag("Player"))
        {
            // Je�li tak, zmieniamy grawitacj�
            rb.gravityScale = newGravity;

            // Odwracamy sprite'a w pionie
            FlipSprite();
        }
    }

    // Metoda do odwr�cenia sprite'a w pionie
    private void FlipSprite()
    {
        // Odwracamy sprite'a w pionie
        spriteRenderer.flipY = !spriteRenderer.flipY;
        // Zapisujemy informacj� o aktualnym stanie odwr�cenia
        flipped = !flipped;
    }

    // Pami�taj, aby ustawi� flipped na false na pocz�tku, je�li sprite na starcie jest w normalnej orientacji
    private void Start()
    {
        flipped = false;
    }
}

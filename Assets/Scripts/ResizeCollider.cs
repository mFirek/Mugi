using UnityEngine;

public class ResizeCollider : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private new BoxCollider2D collider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();

        // Jeúli collider nie istnieje, dodaj nowy
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }
    }

    void Update()
    {
        // Sprawdü, czy obiekt ma SpriteRenderer i BoxCollider2D
        if (spriteRenderer != null && collider != null)
        {
            // Dostosuj rozmiar collidera do rozmiaru sprite'a
            collider.size = spriteRenderer.bounds.size;
        }
    }
}

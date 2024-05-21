using UnityEngine;

public class ResizeCollider2 : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private new BoxCollider2D collider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();

        // If collider does not exist, add a new one
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }
    }

    void Update()
    {
        // Check if the object has a SpriteRenderer and BoxCollider2D
        if (spriteRenderer != null && collider != null)
        {
            // Adjust the size of the collider to match the size of the sprite's local bounds
            Vector3 localSize = spriteRenderer.sprite.bounds.size;
            collider.size = new Vector2(localSize.x * transform.localScale.x, localSize.y * transform.localScale.y);

            // Adjust the offset so the collider is centered on the sprite
            collider.offset = spriteRenderer.sprite.bounds.center;
        }
    }
}

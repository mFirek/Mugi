using UnityEngine;

public class SpikeToggle : MonoBehaviour
{
    [SerializeField] private float toggleInterval = 1f;

    private SpriteRenderer spriteRenderer;
    private Collider2D[] colliders;

    private void Start()
    {
        // Pobieramy referencje do SpriteRenderer i wszystkich Collider2D
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliders = GetComponents<Collider2D>();

        // Uruchamiamy powtarzaj¹ce siê w³¹czanie/wy³¹czanie
        InvokeRepeating(nameof(ToggleSpike), toggleInterval, toggleInterval);
    }

    private void ToggleSpike()
    {
        // Odwracamy aktualny stan aktywnoœci
        bool isActive = spriteRenderer.enabled;

        spriteRenderer.enabled = !isActive; // W³¹cz/wy³¹cz widocznoœæ

        // W³¹cz/wy³¹cz wszystkie collidery na obiekcie
        foreach (var collider in colliders)
        {
            collider.enabled = !isActive;
        }
    }

    private void OnDestroy()
    {
        // Anulujemy InvokeRepeating, aby unikn¹æ b³êdów przy niszczeniu obiektu
        CancelInvoke(nameof(ToggleSpike));
    }
}

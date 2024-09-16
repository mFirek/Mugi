using System.Collections;
using UnityEngine;

public class AutoInvisibleSpike : MonoBehaviour
{
    public float visibleDuration = 3f; // Czas, przez jaki kolec ma by� widoczny
    public float invisibleDuration = 2f; // Czas, przez jaki kolec ma by� niewidzialny
    private SpriteRenderer spriteRenderer;
    private Collider2D[] colliders; // Przechowujemy wszystkie Collidery2D

    void Start()
    {
        // Pobieramy komponent SpriteRenderer i wszystkie Collidery2D na obiekcie
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliders = GetComponents<Collider2D>();

        // Rozpoczynamy cykliczne ukrywanie i wy�wietlanie kolc�w
        StartCoroutine(CycleSpikeVisibility());
    }

    IEnumerator CycleSpikeVisibility()
    {
        while (true)
        {
            // Widoczne kolce
            spriteRenderer.enabled = true;
            foreach (Collider2D col in colliders)
            {
                col.enabled = true;
            }

            // Czekamy przez czas, kiedy kolce s� widoczne
            yield return new WaitForSeconds(visibleDuration);

            // Niewidoczne kolce
            spriteRenderer.enabled = false;
            foreach (Collider2D col in colliders)
            {
                col.enabled = false;
            }

            // Czekamy przez czas, kiedy kolce s� niewidoczne
            yield return new WaitForSeconds(invisibleDuration);
        }
    }
}

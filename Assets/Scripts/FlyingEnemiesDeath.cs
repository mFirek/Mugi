using UnityEngine;
using System.Collections;

public class FlyingEnemiesDeath : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D[] colliders;
    bool isDying = false;

    // Licznik zderzeñ z obiektem o tagu "Kula"
    private int collisionCount = 0;

    // Maksymalna liczba zderzeñ przed znikniêciem obiektu
    public int maxCollisions = 3;

    // Czas trwania migania postaci
    public float blinkDuration = 0.5f;

    // Skrypt "FlyingEnemyAI", który chcemy wy³¹czyæ podczas migania i po znikniêciu obiektu
    public FlyingEnemyAI flyingEnemyAI;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDying && collision.gameObject.CompareTag("Kula"))
        {
            // Zniszcz pocisk
            Destroy(collision.gameObject);

            // Zwiêksz licznik zderzeñ
            collisionCount++;

            // Jeœli liczba zderzeñ przekroczy³a limit, zniszcz obiekt
            if (collisionCount >= maxCollisions)
            {
                StartCoroutine(BlinkAndDestroy());
            }
        }
    }

    IEnumerator BlinkAndDestroy()
    {
        isDying = true;

        // Wy³¹cz skrypt podczas migania
        if (flyingEnemyAI != null)
        {
            flyingEnemyAI.enabled = false;
        }

        // Zatrzymaj ruch obiektu
        rb.velocity = Vector2.zero;

        // Zapêtlenie migania przez okreœlony czas
        float timer = 0f;
        while (timer < blinkDuration)
        {
            // Zmieñ aktywnoœæ renderera na przeciwn¹
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = !renderer.enabled;
            }

            // Czekaj krótk¹ chwilê
            yield return new WaitForSeconds(0.1f);

            // Aktualizuj czas
            timer += 0.1f;
        }

        // Wy³¹cz postaæ po zakoñczeniu migania
        gameObject.SetActive(false);
    }
}

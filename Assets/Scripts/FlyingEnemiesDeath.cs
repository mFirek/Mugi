using UnityEngine;
using System.Collections;

public class FlyingEnemiesDeath : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D[] colliders;
    bool isDying = false;

    // Licznik zderze� z obiektem o tagu "Kula"
    private int collisionCount = 0;

    // Maksymalna liczba zderze� przed znikni�ciem obiektu
    public int maxCollisions = 3;

    // Czas trwania migania postaci
    public float blinkDuration = 0.5f;

    // Skrypt "FlyingEnemyAI", kt�ry chcemy wy��czy� podczas migania i po znikni�ciu obiektu
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

            // Zwi�ksz licznik zderze�
            collisionCount++;

            // Je�li liczba zderze� przekroczy�a limit, zniszcz obiekt
            if (collisionCount >= maxCollisions)
            {
                StartCoroutine(BlinkAndDestroy());
            }
        }
    }

    IEnumerator BlinkAndDestroy()
    {
        isDying = true;

        // Wy��cz skrypt podczas migania
        if (flyingEnemyAI != null)
        {
            flyingEnemyAI.enabled = false;
        }

        // Zatrzymaj ruch obiektu
        rb.velocity = Vector2.zero;

        // Zap�tlenie migania przez okre�lony czas
        float timer = 0f;
        while (timer < blinkDuration)
        {
            // Zmie� aktywno�� renderera na przeciwn�
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = !renderer.enabled;
            }

            // Czekaj kr�tk� chwil�
            yield return new WaitForSeconds(0.1f);

            // Aktualizuj czas
            timer += 0.1f;
        }

        // Wy��cz posta� po zako�czeniu migania
        gameObject.SetActive(false);
    }
}

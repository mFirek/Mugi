using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    // Czas migania pocisku
    public float blinkDuration = 0.5f;

    // Flaga informuj¹ca, czy pocisk jest w trakcie miganie
    private bool isBlinking = false;

    // Kolizja pocisku z innym obiektem
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // SprawdŸ, czy pocisk zderzy³ siê z obiektem o tagu "Kula"
        if (collision.gameObject.CompareTag("Kula"))
        {
            // Jeœli pocisk nie miga, rozpocznij proces migania
            if (!isBlinking)
            {
                isBlinking = true;
                StartCoroutine(BlinkAndDestroy());
            }
        }
    }

    // Migotanie pocisku i zniszczenie go
    private System.Collections.IEnumerator BlinkAndDestroy()
    {
        // Zapêtlenie migania przez okreœlony czas
        float timer = 0f;
        while (timer < blinkDuration)
        {
            // Migaj na bia³o
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;

            // Czekaj krótk¹ chwilê
            yield return new WaitForSeconds(0.1f);

            // Aktualizuj czas
            timer += 0.1f;
        }

        // Wy³¹cz pocisk po zakoñczeniu migania
        gameObject.SetActive(false);
    }
}

using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    // Czas migania pocisku
    public float blinkDuration = 0.5f;

    // Flaga informuj�ca, czy pocisk jest w trakcie miganie
    private bool isBlinking = false;

    // Kolizja pocisku z innym obiektem
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Sprawd�, czy pocisk zderzy� si� z obiektem o tagu "Kula"
        if (collision.gameObject.CompareTag("Kula"))
        {
            // Je�li pocisk nie miga, rozpocznij proces migania
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
        // Zap�tlenie migania przez okre�lony czas
        float timer = 0f;
        while (timer < blinkDuration)
        {
            // Migaj na bia�o
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;

            // Czekaj kr�tk� chwil�
            yield return new WaitForSeconds(0.1f);

            // Aktualizuj czas
            timer += 0.1f;
        }

        // Wy��cz pocisk po zako�czeniu migania
        gameObject.SetActive(false);
    }
}

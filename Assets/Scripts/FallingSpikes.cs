using System.Collections;
using UnityEngine;

public class FallingSpikes : MonoBehaviour
{
    public GameObject spikes; // Przypisz obiekt kolc�w w Inspectorze
    public float fallSpeed = 5f; // Pr�dko�� opadania kolc�w
    public float resetTime = 1f; // Czas po jakim kolce maj� by� resetowane

    private Vector3 initialPosition; // Pozycja pocz�tkowa kolc�w
    private bool isTriggered = false;

    void Start()
    {
        // Zapami�taj pocz�tkow� pozycj� kolc�w
        if (spikes != null)
        {
            initialPosition = spikes.transform.position;
            Rigidbody2D rb = spikes.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic; // Ustaw Kinematic na pocz�tku
                rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Ustaw interpolacj�
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Ustaw ci�g�� detekcj� kolizji
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTriggered = true;
            Rigidbody2D rb = spikes.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic; // Ustaw Dynamic, aby umo�liwi� spadanie
            }

            // Rozpocznij coroutine do resetowania pozycji po okre�lonym czasie
            StartCoroutine(ResetSpikeAfterTime());
        }
    }

    void Update()
    {
        if (isTriggered && spikes != null)
        {
            // Przesuwanie kolc�w w d�
            spikes.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    private IEnumerator ResetSpikeAfterTime()
    {
        yield return new WaitForSeconds(resetTime);
        ResetSpikePosition();
    }

    void ResetSpikePosition()
    {
        // Przenie� kolce z powrotem do pozycji pocz�tkowej
        spikes.transform.position = initialPosition;
        // Resetuj Rigidbody2D, aby zatrzyma� ruch
        Rigidbody2D rb = spikes.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // Ustaw ponownie na Kinematic
            rb.velocity = Vector2.zero; // Zatrzymaj wszelki ruch
        }
        // Resetuj stan wyzwalania
        isTriggered = false;
    }
}

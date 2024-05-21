using System.Collections;
using UnityEngine;

public class FallingSpikes : MonoBehaviour
{
    public GameObject spikes; // Przypisz obiekt kolców w Inspectorze
    public float fallSpeed = 5f; // Prêdkoœæ opadania kolców
    public float resetTime = 1f; // Czas po jakim kolce maj¹ byæ resetowane

    private Vector3 initialPosition; // Pozycja pocz¹tkowa kolców
    private bool isTriggered = false;

    void Start()
    {
        // Zapamiêtaj pocz¹tkow¹ pozycjê kolców
        if (spikes != null)
        {
            initialPosition = spikes.transform.position;
            Rigidbody2D rb = spikes.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic; // Ustaw Kinematic na pocz¹tku
                rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Ustaw interpolacjê
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Ustaw ci¹g³¹ detekcjê kolizji
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
                rb.bodyType = RigidbodyType2D.Dynamic; // Ustaw Dynamic, aby umo¿liwiæ spadanie
            }

            // Rozpocznij coroutine do resetowania pozycji po okreœlonym czasie
            StartCoroutine(ResetSpikeAfterTime());
        }
    }

    void Update()
    {
        if (isTriggered && spikes != null)
        {
            // Przesuwanie kolców w dó³
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
        // Przenieœ kolce z powrotem do pozycji pocz¹tkowej
        spikes.transform.position = initialPosition;
        // Resetuj Rigidbody2D, aby zatrzymaæ ruch
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

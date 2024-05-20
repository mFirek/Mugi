using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikes : MonoBehaviour
{
    public GameObject spikes; // Przypisz obiekt kolców w Inspectorze
    public float fallSpeed = 5f; // Prêdkoœæ opadania kolców

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
                Debug.Log("Rigidbody2D ustawiony na Kinematic z interpolacj¹ i ci¹g³¹ detekcj¹ kolizji.");
            }
            else
            {
                Debug.LogError("Brak komponentu Rigidbody2D na obiekcie kolców.");
            }
        }
        else
        {
            Debug.LogError("Obiekt kolców nie zosta³ przypisany.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Gracz aktywowa³ spadanie kolców");
            isTriggered = true;
            Rigidbody2D rb = spikes.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic; // Ustaw Dynamic, aby umo¿liwiæ spadanie
                Debug.Log("Rigidbody2D ustawiony na Dynamic.");
            }
            else
            {
                Debug.LogError("Brak komponentu Rigidbody2D na obiekcie kolców.");
            }
        }
    }

    void Update()
    {
        if (isTriggered && spikes != null)
        {
            // Przesuwanie kolców w dó³
            spikes.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            Debug.Log("Kolce spadaj¹. Pozycja: " + spikes.transform.position);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Kolce zderzy³y siê z: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Teren"))
        {
            Debug.Log("Kolce dotknê³y terenu, resetowanie pozycji");
            ResetSpikePosition();
        }
        else
        {
            Debug.Log("Kolce zderzy³y siê z czymœ innym: " + collision.gameObject.tag);
        }
    }

    void ResetSpikePosition()
    {
        // Przenieœ kolce z powrotem do pozycji pocz¹tkowej
        Debug.Log("Resetowanie pozycji kolców do: " + initialPosition);
        spikes.transform.position = initialPosition;
        // Resetuj Rigidbody2D, aby zatrzymaæ ruch
        Rigidbody2D rb = spikes.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // Ustaw ponownie na Kinematic
            rb.velocity = Vector2.zero; // Zatrzymaj wszelki ruch
            Debug.Log("Rigidbody2D ustawiony na Kinematic po resetowaniu.");
        }
        else
        {
            Debug.LogError("Brak komponentu Rigidbody2D na obiekcie kolców.");
        }
        // Resetuj stan wyzwalania
        isTriggered = false;
    }
}

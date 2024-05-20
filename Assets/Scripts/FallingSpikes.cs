using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikes : MonoBehaviour
{
    public GameObject spikes; // Przypisz obiekt kolc�w w Inspectorze
    public float fallSpeed = 5f; // Pr�dko�� opadania kolc�w

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
                Debug.Log("Rigidbody2D ustawiony na Kinematic z interpolacj� i ci�g�� detekcj� kolizji.");
            }
            else
            {
                Debug.LogError("Brak komponentu Rigidbody2D na obiekcie kolc�w.");
            }
        }
        else
        {
            Debug.LogError("Obiekt kolc�w nie zosta� przypisany.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Gracz aktywowa� spadanie kolc�w");
            isTriggered = true;
            Rigidbody2D rb = spikes.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic; // Ustaw Dynamic, aby umo�liwi� spadanie
                Debug.Log("Rigidbody2D ustawiony na Dynamic.");
            }
            else
            {
                Debug.LogError("Brak komponentu Rigidbody2D na obiekcie kolc�w.");
            }
        }
    }

    void Update()
    {
        if (isTriggered && spikes != null)
        {
            // Przesuwanie kolc�w w d�
            spikes.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            Debug.Log("Kolce spadaj�. Pozycja: " + spikes.transform.position);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Kolce zderzy�y si� z: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Teren"))
        {
            Debug.Log("Kolce dotkn�y terenu, resetowanie pozycji");
            ResetSpikePosition();
        }
        else
        {
            Debug.Log("Kolce zderzy�y si� z czym� innym: " + collision.gameObject.tag);
        }
    }

    void ResetSpikePosition()
    {
        // Przenie� kolce z powrotem do pozycji pocz�tkowej
        Debug.Log("Resetowanie pozycji kolc�w do: " + initialPosition);
        spikes.transform.position = initialPosition;
        // Resetuj Rigidbody2D, aby zatrzyma� ruch
        Rigidbody2D rb = spikes.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // Ustaw ponownie na Kinematic
            rb.velocity = Vector2.zero; // Zatrzymaj wszelki ruch
            Debug.Log("Rigidbody2D ustawiony na Kinematic po resetowaniu.");
        }
        else
        {
            Debug.LogError("Brak komponentu Rigidbody2D na obiekcie kolc�w.");
        }
        // Resetuj stan wyzwalania
        isTriggered = false;
    }
}

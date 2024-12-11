using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public int startingPoint;
    public Transform[] points;
    public float travelTime = 2f; // Czas podró¿y miêdzy punktami w sekundach
    private float speed;
    private int i;

    void Start()
    {
        if (points.Length > 0)
        {
            i = startingPoint % points.Length; // Obs³uga bezpiecznego indeksu
            transform.position = points[i].position;
            CalculateSpeed();
        }
        else
        {
            Debug.LogError("Brak przypisanych punktów do zmiennej points!");
        }
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.01f) // Tolerancja bliska zera
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
            CalculateSpeed(); // Obliczenie prêdkoœci dla nowego punktu
        }

        // Przesuwanie platformy
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    private void CalculateSpeed()
    {
        if (points.Length > 1)
        {
            // Oblicz dystans do nastêpnego punktu
            float distance = Vector2.Distance(transform.position, points[i].position);
            speed = distance / travelTime; // Prêdkoœæ = odleg³oœæ / czas
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);

            foreach (ContactPoint2D contact in contacts)
            {
                if (contact.point.y > transform.position.y) // Sprawdzenie, czy punkt kolizji jest powy¿ej platformy
                {
                    collision.transform.SetParent(transform);
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}

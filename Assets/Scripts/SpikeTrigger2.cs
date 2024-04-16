using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger2 : MonoBehaviour
{
    private GameObject player; // Za��my, �e posta� gracza to obiekt GameObject

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Sprawdzamy, czy obiekt, kt�ry wszed� w trigger, to posta� gracza
        {
            // Znajd� obiekt startowy
            GameObject startObject = GameObject.FindGameObjectWithTag("Start");

            if (startObject != null)
            {
                // Je�li znaleziono obiekt startowy, cofnij gracza do jego pozycji
                other.transform.position = startObject.transform.position;
            }
            else
            {
                Debug.LogWarning("Nie znaleziono obiektu startowego!");
            }
        }
    }
}

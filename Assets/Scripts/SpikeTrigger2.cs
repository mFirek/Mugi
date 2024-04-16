using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger2 : MonoBehaviour
{
    private GameObject player; // Za³ó¿my, ¿e postaæ gracza to obiekt GameObject

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Sprawdzamy, czy obiekt, który wszed³ w trigger, to postaæ gracza
        {
            // ZnajdŸ obiekt startowy
            GameObject startObject = GameObject.FindGameObjectWithTag("Start");

            if (startObject != null)
            {
                // Jeœli znaleziono obiekt startowy, cofnij gracza do jego pozycji
                other.transform.position = startObject.transform.position;
            }
            else
            {
                Debug.LogWarning("Nie znaleziono obiektu startowego!");
            }
        }
    }
}

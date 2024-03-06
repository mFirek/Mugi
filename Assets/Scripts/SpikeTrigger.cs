using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    private Vector3 startingPosition;
    private GameObject player; // Załóżmy, że postać gracza to obiekt GameObject

    void Start()
    {
        // Zapisz początkową pozycję gracza
        player = GameObject.FindGameObjectWithTag("Player"); // Odnajdujemy postać gracza po tagu "Player"
        startingPosition = player.transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Sprawdzamy, czy obiekt, który wszedł w trigger, to postać gracza
        {
            // Jeśli tak, cofnij gracza do początkowej pozycji
            player.transform.position = startingPosition;
        }
    }
}

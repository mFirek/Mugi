using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
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
*/



public class SpikeTrigger : MonoBehaviour
{
    private GameObject player;
    private bool isDead = false;  // Flag to prevent multiple counting of deaths

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            // Protect against multiple billing deaths
            isDead = true;

            // Get a player's revival point
            Vector2 respawnPoint = GameManager.Instance.GetSpawnPoint();

            // Move the player to the revival point
            player.transform.position = respawnPoint;

            // Invoke the player death event through the PlayerDied method in GameEventsManager
            if (GameEventsManager.instance != null)
            {
                GameEventsManager.instance.PlayerDied();
            }
            else
            {
                Debug.LogError("Nie znaleziono instancji GameEventsManager!");
            }

            // Reset flag before 1 sec
            StartCoroutine(ResetDeathFlag());
        }
    }

    // Coroutine, for reseting the flag
    IEnumerator ResetDeathFlag()
    {
        yield return new WaitForSeconds(1f);  // Wait 1 sec before flag reset
        isDead = false;  // Reset flag 
    }
}

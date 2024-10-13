
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileScript1 : MonoBehaviour
{
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
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Nie znaleziono instancji GameEventsManager!");
            }

            // Reset flag before 1 sec
            StartCoroutine(ResetDeathFlag());
        }
        if (other.CompareTag("Teren") || other.CompareTag("platform"))
            {
            Destroy(gameObject);
        }
    }




    private GameObject player;
    private bool isDead = false;  // Flag to prevent multiple counting of deaths


   
    // Coroutine, for reseting the flag
    IEnumerator ResetDeathFlag()
    {
        yield return new WaitForSeconds(1f);  // Wait 1 sec before flag reset
        isDead = false;  // Reset flag 
    }
}

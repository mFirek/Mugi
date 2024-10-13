using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger2 : MonoBehaviour
{
    private bool isDead = false;
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
            if (other.CompareTag("Player") && !isDead)
            {
                isDead = true;
                if (GameEventsManager.instance != null)
                {
                    GameEventsManager.instance.PlayerDied();
                }
                else
                {
                    Debug.LogError("Nie znaleziono instancji GameEventsManager!");
                }
                StartCoroutine(ResetDeathFlag());
            }
        }

    }
    IEnumerator ResetDeathFlag()
    {
        yield return new WaitForSeconds(0.2f);  // Wait 1 sec before flag reset
        isDead = false;  // Reset flag 
    }
}

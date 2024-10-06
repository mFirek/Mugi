// SpikeTrigger.cs
using System.Collections;
using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    private GameObject player;
    private bool isDead = false;  // Flaga zapobiegająca wielokrotnemu zliczaniu zgonów

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            // Zapobiegaj wielokrotnemu zliczaniu zgonów
            isDead = true;

            // Uzyskaj punkt respawnu gracza
            Vector2 respawnPoint = GameManager.Instance.GetSpawnPoint();

            // Przenieś gracza do punktu respawnu
            player.transform.position = respawnPoint;

            // Wywołaj zdarzenie śmierci gracza
            if (GameEventsManager.instance != null)
            {
                GameEventsManager.instance.PlayerDied();  // Wywołaj zdarzenie
                GlobalDeathCounter.IncrementGlobalDeathCount();  // Zwiększ globalny licznik
            }
            else
            {
                Debug.LogError("Nie znaleziono instancji GameEventsManager!");
            }

            // Zresetuj flagę po 1 sekundzie
            StartCoroutine(ResetDeathFlag());
        }
    }

    // Coroutine do resetowania flagi
    IEnumerator ResetDeathFlag()
    {
        yield return new WaitForSeconds(1f);  // Poczekaj 1 sekundę przed resetowaniem flagi
        isDead = false;  // Resetuj flagę
    }
}

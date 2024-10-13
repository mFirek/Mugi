//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SpikeTrigger : MonoBehaviour
//{
//    private GameObject player;
//    public bool isDead = false;  // Flag to prevent multiple counting of deaths

//    void Start()
//    {
//        player = GameObject.FindGameObjectWithTag("Player");
//    }

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player") && !isDead)
//        {
//            // Protect against multiple billing deaths
//            isDead = true;

//            // Get a player's revival point
//            Vector2 respawnPoint = GameManager.Instance.GetSpawnPoint();

//            // Move the player to the revival point
//            player.transform.position = respawnPoint;

//            // Invoke the player death event through the PlayerDied method in GameEventsManager
//            if (GameEventsManager.instance != null)
//            {
//                GameEventsManager.instance.PlayerDied();
//            }
//            else
//            {
//                Debug.LogError("Nie znaleziono instancji GameEventsManager!");
//            }

//            // Reset flag before 1 sec
//            StartCoroutine(ResetDeathFlag());
//        }
//    }

//    // Coroutine, for reseting the flag
//    IEnumerator ResetDeathFlag()
//    {
//        yield return new WaitForSeconds(1f);  // Wait 1 sec before flag reset
//        isDead = false;  // Reset flag 
//    }
//}

//public class SpikeTrigger : MonoBehaviour
//{
//    public static bool isPlayerDead = false;  // Statyczna flaga monitorująca śmierć gracza

//    private GameObject player;

//    void Start()
//    {
//        player = GameObject.FindGameObjectWithTag("Player");
//    }

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player") && !isPlayerDead)
//        {
//            isPlayerDead = true;  // Ustawienie globalnej flagi

//            // Pobieranie punktu respawnu
//            Vector2 respawnPoint = GameManager.Instance.GetSpawnPoint();
//            player.transform.position = respawnPoint;

//            // Emitowanie zdarzenia śmierci gracza
//            if (GameEventsManager.instance != null)
//            {
//                GameEventsManager.instance.PlayerDied();  // Wywołanie zdarzenia śmierci
//            }
//            else
//            {
//                Debug.LogError("Nie znaleziono instancji GameEventsManager!");
//            }

//            StartCoroutine(ResetDeathFlag());
//        }
//    }

//    IEnumerator ResetDeathFlag()
//    {
//        yield return new WaitForSeconds(0.5f);  // Czekanie 1 sekundy
//        isPlayerDead = false;  // Resetowanie flagi
//    }
//}

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


using System.Collections;
using UnityEngine;

public class SpikeTrigger2 : MonoBehaviour
{
    private bool isDead = false;
    private GameObject player; // Za³ó¿my, ¿e postaæ gracza to obiekt GameObject

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            // Zapobiegaj wielokrotnemu zliczaniu zgonów
            isDead = true;

            // Uzyskaj punkt respawnu gracza
            Vector2 respawnPoint = GameManager.Instance.GetSpawnPoint();

            // Przenieœ gracza do punktu respawnu
            player.transform.position = respawnPoint;

            // Pobierz aktualn¹ nazwê poziomu
            string level = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            // Pobierz pozycjê gracza w momencie œmierci
            Vector3 deathPosition = transform.position;

            // Pobierz tag obiektu, który spowodowa³ œmieræ (np. kolce, pu³apki, wrogowie)
            string causeOfDeathTag = gameObject.tag;  // Zamiast other.gameObject.tag, u¿ywamy tagu tego obiektu (np. kolce)

            // Pobierz nazwê obiektu, który spowodowa³ kolizjê (np. kolce, pu³apka)
            string causeOfDeath = causeOfDeathTag; // Przyczyna œmierci jest oparta na tagu obiektu

            // Ustaw dane œmierci w GameEventsManager
            GameEventsManager.SetDeathData(level, deathPosition, causeOfDeath, causeOfDeathTag);

            // Wywo³aj zdarzenie œmierci
            if (GameEventsManager.instance != null)
            {
                GameEventsManager.instance.PlayerDied();
            }
            else
            {
                Debug.LogError("Nie znaleziono instancji GameEventsManager!");
            }

            // Zresetuj flagê po 1 sekundzie
            StartCoroutine(ResetDeathFlag());
        }
    }

    // Coroutine do resetowania flagi
    IEnumerator ResetDeathFlag()
    {
        yield return new WaitForSeconds(0.2f);  // Poczekaj 0.2 sekundy przed resetowaniem flagi
        isDead = false;  // Resetuj flagê
    }
}

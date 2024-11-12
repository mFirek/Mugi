using System.Collections;
using UnityEngine;

public class Lavatrigger : MonoBehaviour
{
    private GameObject player;
    private bool isDead = false;  // Flaga zapobiegaj¹ca wielokrotnemu zliczaniu zgonów
    public DeactivateObjectOnTrigger keyScript; // Referencja do skryptu klucza

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

            // Przenieœ gracza do punktu respawnu
            player.transform.position = respawnPoint;

            // Pobierz aktualn¹ nazwê poziomu
            string level = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            // Pobierz pozycjê gracza w momencie œmierci
            Vector3 deathPosition = transform.position;

            // Pobierz nazwê obiektu, z którym gracz zderzy³ siê (w tym przypadku lava)
            string causeOfDeath = "Lava";

            // Ustaw dane œmierci w GameEventsManager
            GameEventsManager.SetDeathData(level, deathPosition, causeOfDeath);

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
        yield return new WaitForSeconds(1f);  // Poczekaj 1 sekundê przed resetowaniem flagi
        isDead = false;  // Resetuj flagê
    }
}

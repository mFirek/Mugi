// SpikeTrigger.cs
using System.Collections;
using UnityEngine;

public class Lavatrigger : MonoBehaviour
{
    private GameObject player;
    private bool isDead = false;  // Flaga zapobiegaj�ca wielokrotnemu zliczaniu zgon�w
    public DeactivateObjectOnTrigger keyScript; // Referencja do skryptu klucza

    // Funkcja wywo�ywana, gdy gracz umiera

    // Resurrect key
    // Wywo�aj metod�, kt�ra ponownie aktywuje klucz

    // Dodaj inne logiki zwi�zane ze �mierci� gracza, np. resetowanie pozycji, licznik �y� itp.

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            // Zapobiegaj wielokrotnemu zliczaniu zgon�w
            isDead = true;

            // Uzyskaj punkt respawnu gracza
            Vector2 respawnPoint = GameManager.Instance.GetSpawnPoint();

            // Przenie� gracza do punktu respawnu
            player.transform.position = respawnPoint;

            // Wywo�aj zdarzenie �mierci gracza
            if (GameEventsManager.instance != null)
            {
                GameEventsManager.instance.PlayerDied();  // Wywo�aj zdarzenie

                GlobalDeathCounter.IncrementGlobalDeathCount();  // Zwi�ksz globalny licznik
                keyScript.RespawnKey();
            }
            else
            {
                Debug.LogError("Nie znaleziono instancji GameEventsManager!");
            }

            // Zresetuj flag� po 1 sekundzie
            StartCoroutine(ResetDeathFlag());
        }
    }

    // Coroutine do resetowania flagi
    IEnumerator ResetDeathFlag()
    {
        yield return new WaitForSeconds(1f);  // Poczekaj 1 sekund� przed resetowaniem flagi
        isDead = false;  // Resetuj flag�
    }
}

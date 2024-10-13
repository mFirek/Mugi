// SpikeTrigger.cs
using System.Collections;
using UnityEngine;

public class Lavatrigger : MonoBehaviour
{
    private GameObject player;
    private bool isDead = false;  // Flaga zapobiegaj¹ca wielokrotnemu zliczaniu zgonów
    public DeactivateObjectOnTrigger keyScript; // Referencja do skryptu klucza

    // Funkcja wywo³ywana, gdy gracz umiera

    // Resurrect key
    // Wywo³aj metodê, która ponownie aktywuje klucz

    // Dodaj inne logiki zwi¹zane ze œmierci¹ gracza, np. resetowanie pozycji, licznik ¿yæ itp.

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

            // Wywo³aj zdarzenie œmierci gracza
            if (GameEventsManager.instance != null)
            {
                GameEventsManager.instance.PlayerDied();  // Wywo³aj zdarzenie

                GlobalDeathCounter.IncrementGlobalDeathCount();  // Zwiêksz globalny licznik
                keyScript.RespawnKey();
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

using System.Collections;
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

            if (!isDead) // Zapobiegamy wielokrotnemu zliczaniu œmierci
            {
                isDead = true;

                // Pobierz aktualn¹ nazwê poziomu
                string level = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

                // Pobierz pozycjê gracza w momencie œmierci
                Vector3 deathPosition = transform.position;

                // Pobierz nazwê obiektu, z którym gracz zderzy³ siê
                string causeOfDeath = "Spikes"; // Mo¿esz ustawiæ na "Spikes" lub u¿yæ nazwy obiektu

                // U¿yj tagu obiektu, który spowodowa³ kolizjê (gracza)
                string causeOfDeathTag = other.gameObject.tag;

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

                StartCoroutine(ResetDeathFlag());
            }
        }
    }

    // Coroutine do resetowania flagi
    IEnumerator ResetDeathFlag()
    {
        yield return new WaitForSeconds(0.2f);  // Poczekaj 0.2 sekundy przed resetowaniem flagi
        isDead = false;  // Resetuj flagê
    }
}

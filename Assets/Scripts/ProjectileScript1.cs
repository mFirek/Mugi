using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ProjectileScript1 : MonoBehaviour
{
    private GameObject player;
    private bool isDead = false;  // Flaga zapobiegaj¹ca wielokrotnemu zliczaniu zgonów

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdzamy, czy kula trafi³a w gracza i czy gracz nie jest ju¿ martwy
        if (other.CompareTag("Player") && !isDead)
        {
            // Zaznaczamy, ¿e gracz jest martwy
            isDead = true;

            // Prze³adowujemy scenê
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);

            // Rejestrujemy œmieræ gracza w GameEventsManager
            if (GameEventsManager.instance != null)
            {
                string causeOfDeathTag = gameObject.tag;  // U¿ywamy tagu obiektu (w tym przypadku "WugieSpecialAttack")

                // Pobieramy nazwê obiektu, który spowodowa³ kolizjê (np. kula)
                string causeOfDeath = causeOfDeathTag;  // Przyczyna œmierci to tag kuli
                // Przekazujemy dane o œmierci
                // Zmieniamy dostêp do metody na statyczny
                GameEventsManager.SetDeathData(currentSceneName, other.transform.position, causeOfDeath, causeOfDeathTag);

                GameEventsManager.instance.PlayerDied(); // Wywo³anie zdarzenia œmierci
            }
            else
            {
                Debug.LogError("Nie znaleziono instancji GameEventsManager!");
            }

            // Resetujemy flagê œmierci po krótkiej przerwie (1 sekunda)
            StartCoroutine(ResetDeathFlag());
        }
        else if (other.CompareTag("Teren") || other.CompareTag("platform"))
        {
            // Zniszczenie kuli po uderzeniu w teren lub platformê
            Destroy(gameObject);
        }
    }

    // Coroutine do resetowania flagi isDead
    private IEnumerator ResetDeathFlag()
    {
        yield return new WaitForSeconds(1f);  // Poczekaj 1 sekundê przed resetowaniem flagi
        isDead = false;  // Resetuj flagê
    }
}

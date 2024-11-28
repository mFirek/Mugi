using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Ink.Parsed;

public class ReloadSceneOnTrigger : MonoBehaviour
{
    private bool isDead = false;  // Flaga zapobiegaj¹ca wielokrotnemu zliczaniu zgonów

    // Ta funkcja zostanie wywo³ana, gdy inny obiekt wejdzie w kolizjê z triggerem
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            isDead = true;  // Zapobiegaj wielokrotnemu zliczaniu zgonów

            // Prze³adowanie sceny
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);

            // Zarejestruj zdarzenie œmierci w GameEventsManager
            if (GameEventsManager.instance != null)
            {
                // Przekazujemy dane o œmierci
                // Zmieniamy dostêp do metody na statyczny
                GameEventsManager.SetDeathData(currentSceneName, other.transform.position, "Cause of death", "CauseTag");

                GameEventsManager.instance.PlayerDied(); // Wywo³anie zdarzenia œmierci
            }
            else
            {
                Debug.LogError("Nie znaleziono instancji GameEventsManager!");
            }

            StartCoroutine(ResetDeathFlag());
        }
    }


    // Zmieniamy metodê, aby przyjmowa³a 'Collider2D other' jako argument
    public void HandlePlayerDeath(Collider2D other)
    {
        // 1. Zwiêkszamy liczbê globalnych zgonów
        if (GlobalDeathCounter.instance != null)
        {
            GlobalDeathCounter.IncrementGlobalDeathCount();  // Zwiêkszamy globaln¹ liczbê zgonów
        }
        else
        {
            Debug.LogError("GlobalDeathCounter: Nie znaleziono instancji!");
        }

        // 2. Zwiêkszamy liczbê zgonów w lokalnym liczniku
        if (DeathCountText.instance != null)
        {
            DeathCountText.instance.OnPlayerDeath(); // Zwiêkszamy lokalny licznik
        }
        else
        {
            Debug.LogError("DeathCountText: Nie znaleziono instancji!");
        }

        // 3. Wysy³amy zdarzenie analityczne
        if (PlayerDeathAnalytics.instance != null)
        {
            PlayerDeathAnalytics.instance.ReportPlayerDeath(other); // Wysy³amy dane o œmierci gracza
        }
        else
        {
            Debug.LogError("PlayerDeathAnalytics: Nie znaleziono instancji!");
        }
    }

    private IEnumerator ResetDeathFlag()
    {
        yield return new WaitForSeconds(1f);  // Poczekaj 1 sekundê przed resetowaniem flagi
        isDead = false;  // Resetuj flagê
    }
}

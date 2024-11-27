using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class ReloadSceneOnTrigger : MonoBehaviour
{


    // Ta funkcja zostanie wywo³ana, gdy inny obiekt wejdzie w kolizjê z triggerem
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            isDead = true;  // Zapobiegaj wielokrotnemu zliczaniu zgonów
            // Pobiera nazwê aktualnej sceny
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Prze³adowuje aktualn¹ scenê
            SceneManager.LoadScene(currentSceneName);


            // Mo¿esz dodaæ warunki, które sprawdz¹, czy obiekt, który wszed³ w trigger, to odpowiedni obiekt
            // Przyk³ad: if (other.CompareTag("Player"))
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

    private bool isDead = false;  // Flaga zapobiegaj¹ca wielokrotnemu zliczaniu zgonów

    IEnumerator ResetDeathFlag()
    {
        yield return new WaitForSeconds(1f);  // Poczekaj 1 sekundê przed resetowaniem flagi
        isDead = false;  // Resetuj flagê
    }
}


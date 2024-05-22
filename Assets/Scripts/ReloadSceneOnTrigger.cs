using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneOnTrigger : MonoBehaviour
{
    // Ta funkcja zostanie wywo�ana, gdy inny obiekt wejdzie w kolizj� z triggerem
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Mo�esz doda� warunki, kt�re sprawdz�, czy obiekt, kt�ry wszed� w trigger, to odpowiedni obiekt
        // Przyk�ad: if (other.CompareTag("Player"))
        if (other.CompareTag("Player"))
        {
            // Pobiera nazw� aktualnej sceny
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Prze�adowuje aktualn� scen�
            SceneManager.LoadScene(currentSceneName);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneOnTrigger : MonoBehaviour
{
    // Ta funkcja zostanie wywo³ana, gdy inny obiekt wejdzie w kolizjê z triggerem
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Mo¿esz dodaæ warunki, które sprawdz¹, czy obiekt, który wszed³ w trigger, to odpowiedni obiekt
        // Przyk³ad: if (other.CompareTag("Player"))
        if (other.CompareTag("Player"))
        {
            // Pobiera nazwê aktualnej sceny
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Prze³adowuje aktualn¹ scenê
            SceneManager.LoadScene(currentSceneName);
        }
    }
}

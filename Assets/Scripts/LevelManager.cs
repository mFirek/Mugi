using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string nextLevelName; // Nazwa kolejnej sceny
    public string playerTag = "Player"; // Tag gracza

    void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdü, czy obiekt kolidujπcy ma tag "Player"
        if (other.CompareTag(playerTag))
        {
            // Jeúli tak, za≥aduj kolejny poziom
            SceneManager.LoadScene(nextLevelName);
        }
    }
}

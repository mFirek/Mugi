using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string nextLevelName; // Nazwa kolejnej sceny
    public string playerTag = "Player"; // Tag gracza

    void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawd�, czy obiekt koliduj�cy ma tag "Player"
        if (other.CompareTag(playerTag))
        {
            // Je�li tak, za�aduj kolejny poziom
            SceneManager.LoadScene(nextLevelName);
        }
    }
}

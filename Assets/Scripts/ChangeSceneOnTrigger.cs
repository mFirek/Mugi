using UnityEngine;
using UnityEngine.SceneManagement;

// Definicja enuma reprezentuj¹cego dostêpne sceny w grze
public enum SceneName
{
    Level1,
    Level2,
    
}

public class ChangeSceneOnTrigger : MonoBehaviour
{
    public SceneName sceneToLoad; // Enum reprezentuj¹cy scenê do za³adowania
    public string triggerTag = "Player"; // Tag obiektu gracza

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            // Pobierz nazwê sceny na podstawie wybranego enuma
            string sceneName = sceneToLoad.ToString();
            // Za³aduj wybran¹ scenê
            SceneManager.LoadScene(sceneName);
            Debug.Log("Trigger Entered");
        }
    }
}

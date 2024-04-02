using UnityEngine;
using UnityEngine.SceneManagement;

// Definicja enuma reprezentuj�cego dost�pne sceny w grze
public enum SceneName
{
    Level1,
    Level2,
    
}

public class ChangeSceneOnTrigger : MonoBehaviour
{
    public SceneName sceneToLoad; // Enum reprezentuj�cy scen� do za�adowania
    public string triggerTag = "Player"; // Tag obiektu gracza

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            // Pobierz nazw� sceny na podstawie wybranego enuma
            string sceneName = sceneToLoad.ToString();
            // Za�aduj wybran� scen�
            SceneManager.LoadScene(sceneName);
            Debug.Log("Trigger Entered");
        }
    }
}

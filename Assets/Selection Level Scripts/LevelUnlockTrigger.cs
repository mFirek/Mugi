using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUnlockTrigger : MonoBehaviour
{
    // Zmienna do trzymania obecnego poziomu
    private int currentLevelIndex;
    private bool hasUnlockedLevel = false; // Flaga, aby zapobiec wielokrotnemu odblokowywaniu poziomu w jednej kolizji

    void Start()
    {
        // Za³aduj obecny poziom z PlayerPrefs
        currentLevelIndex = PlayerPrefs.GetInt("currentLevelIndex", 2); // Domyœlnie poziom 2
        Debug.Log("Aktualny poziom: " + currentLevelIndex);

        // Zarejestruj metodê resetuj¹c¹ flagê, gdy scena zostanie za³adowana
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasUnlockedLevel) // Jeœli gracz wchodzi w portal i poziom jeszcze nie zosta³ odblokowany
        {
            // Odblokowanie poziomu tylko raz podczas jednej kolizji
            UnlockNextLevel();
            hasUnlockedLevel = true; // Ustaw flagê, aby zapobiec dalszemu odblokowywaniu
        }
    }

    void UnlockNextLevel()
    {
        // Zwiêkszenie currentLevelIndex, odblokowanie kolejnego poziomu
        currentLevelIndex++;

        // Zapisz nowy stan poziomu w PlayerPrefs
        PlayerPrefs.SetInt("currentLevelIndex", currentLevelIndex);
        PlayerPrefs.Save();

        Debug.Log("Odblokowany poziom: " + currentLevelIndex);

        // Za³aduj nastêpny poziom
        string nextLevelName = "Level" + currentLevelIndex.ToString(); // Za³ó¿my, ¿e sceny nazywaj¹ siê "Level1", "Level2", itd.
        SceneManager.LoadScene(nextLevelName);
    }

    // Resetowanie flagi po za³adowaniu nowej sceny
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Resetowanie flagi po za³adowaniu nowej sceny
        hasUnlockedLevel = false;
        Debug.Log("Flaga resetowana po za³adowaniu nowej sceny.");
    }

    // Dodatkowa metoda, aby rêcznie usun¹æ nas³uchiwanie zdarzenia po zakoñczeniu dzia³ania obiektu
    private void OnDestroy()
    {
        // Upewnij siê, ¿e usuwamy nas³uchiwanie, gdy obiekt zostanie zniszczony
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

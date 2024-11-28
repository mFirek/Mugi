using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathCountText : MonoBehaviour
{
    public static DeathCountText instance; // Dodajemy statyczn¹ instancjê

    public int deathCount = 0;  // Zmieniamy na publiczne, ¿eby mog³y byæ zmieniane z innych skryptów
    public TextMeshProUGUI deathCountText;

    private void Awake()
    {
        // Ustawiamy instancjê skryptu
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Zniszcz instancjê, jeœli ju¿ istnieje
        }

        deathCountText = GetComponent<TextMeshProUGUI>();

        if (deathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta³ znaleziony na obiekcie!");
        }
    }

    public void Start()
    {
        // Za³aduj licznik zgonów z PlayerPrefs
        LoadDeathCount();

        // Rejestracja na zdarzenie œmierci gracza
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.onPlayerDeath += OnPlayerDeath;
        }

        UpdateDeathCountText();

        // Rejestracja na zdarzenie zmiany sceny (gdy nowa scena jest za³adowana)
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDestroy()
    {
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.onPlayerDeath -= OnPlayerDeath;
        }

        // Wyrejestrowanie z zdarzenia zmiany poziomu
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Publiczna metoda wywo³ywana przy œmierci gracza
    public void OnPlayerDeath()
    {
        deathCount++;
        UpdateDeathCountText();
        SaveLocalDeathCount();  // Zapisz licznik po œmierci gracza
    }

    // Publiczna metoda aktualizuj¹ca tekst liczby zgonów
    public void UpdateDeathCountText()
    {
        deathCountText.text = "Deaths: " + deathCount;
    }

    // Publiczna metoda wczytuj¹ca licznik zgonów z PlayerPrefs
    public void LoadDeathCount()
    {
        // Wczytujemy licznik zgonów z zapisów (PlayerPrefs), domyœlnie ustawiamy na 0
        deathCount = PlayerPrefs.GetInt("LocalDeathCount", 0);
        UpdateDeathCountText();
    }

    // Publiczna metoda zapisuj¹ca licznik zgonów do PlayerPrefs
    public void SaveLocalDeathCount()
    {
        PlayerPrefs.SetInt("LocalDeathCount", deathCount);
    }

    // Publiczna metoda do pobrania liczby zgonów
    public int GetDeathCount()
    {
        return deathCount;
    }

    // Metoda wywo³ywana po za³adowaniu nowej sceny
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Je¿eli scena jest za³adowana (nie restartujemy liczby w tej samej scenie)
        if (scene.name != SceneManager.GetActiveScene().name)
        {
            LoadDeathCount();  // Przywracamy licznik z `PlayerPrefs` po przejœciu do nowego poziomu
        }
    }
    public void ResetDeathCount()
    {
        deathCount = 0;
        UpdateDeathCountText();
        SaveLocalDeathCount();  // Zapisz 0 zgonów do PlayerPrefs
    }

}

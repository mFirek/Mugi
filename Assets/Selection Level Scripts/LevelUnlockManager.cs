using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockManager : MonoBehaviour
{
    public static LevelUnlockManager Instance; // Singleton

    public Button[] levelButtons; // Przycisk dla ka¿dego poziomu przypisany w inspektorze
    private bool[] unlockedLevels; // Tablica przechowuj¹ca stan odblokowania poziomów
    private int totalLevels = 10; // Liczba poziomów (do dostosowania)

    private void Awake()
    {
        
        // Singleton instancji
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Zatrzymaj instancjê miêdzy scenami
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeLevels();
    }

    // Inicjalizacja poziomów, odblokowuj¹c domyœlnie poziomy 0 i 1
    private void InitializeLevels()
    {
        unlockedLevels = new bool[totalLevels];

        // Ustawienie poziomów 0 i 1 jako odblokowanych
        unlockedLevels[0] = true;
        unlockedLevels[1] = true;

        // Za³aduj stan poziomów z PlayerPrefs, nadpisuj¹c tylko odblokowane poziomy powy¿ej pierwszych dwóch
        LoadLevels();

        // Aktualizacja interaktywnoœci przycisków
        UpdateButtonInteractivity();
    }

    // Funkcja do sprawdzenia, czy poziom jest odblokowany
    public bool IsLevelUnlocked(int levelIndex)
    {
        return levelIndex >= 0 && levelIndex < unlockedLevels.Length && unlockedLevels[levelIndex];
    }

    // Funkcja odblokowuj¹ca nastêpny poziom
    public void UnlockNextLevel(int currentLevelIndex)
    {
        int nextLevel = currentLevelIndex + 1;
        if (nextLevel < unlockedLevels.Length && !unlockedLevels[nextLevel])
        {
            unlockedLevels[nextLevel] = true;
            SaveLevels();
            Debug.Log("Odblokowano nowy poziom: " + nextLevel);
        }

        UpdateButtonInteractivity();
    }

    // Funkcja zapisuj¹ca stan poziomów do PlayerPrefs
    private void SaveLevels()
    {
        for (int i = 0; i < unlockedLevels.Length; i++)
        {
            PlayerPrefs.SetInt("LevelUnlocked_" + i, unlockedLevels[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    // Funkcja ³aduj¹ca stan poziomów z PlayerPrefs
    private void LoadLevels()
    {
        for (int i = 2; i < unlockedLevels.Length; i++)
        {
            // £adowanie poziomów zaczynaj¹c od indeksu 2, ¿eby pierwsze dwa pozosta³y odblokowane
            unlockedLevels[i] = PlayerPrefs.GetInt("LevelUnlocked_" + i, 0) == 1;
        }
    }

    // Aktualizacja interaktywnoœci przycisków na podstawie stanu odblokowania poziomów
    private void UpdateButtonInteractivity()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i < unlockedLevels.Length)
            {
                levelButtons[i].interactable = unlockedLevels[i];
            }
        }
    }

    // Funkcja testowa - blokowanie wszystkich poziomów oprócz dwóch pierwszych
    public void LockAllLevelsExceptFirstTwo()
    {
        for (int i = 2; i < unlockedLevels.Length; i++)
        {
            unlockedLevels[i] = false;
            PlayerPrefs.SetInt("LevelUnlocked_" + i, 0); // Zapisz do PlayerPrefs
        }
        PlayerPrefs.Save();
        UpdateButtonInteractivity(); // Aktualizuj przyciski
        Debug.Log("Zablokowano wszystkie poziomy oprócz pierwszych dwóch.");
    }
}

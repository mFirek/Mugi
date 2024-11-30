using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockManager : MonoBehaviour
{
    public static LevelUnlockManager Instance; // Singleton
    public Button[] levelButtons; // Przycisk dla ka¿dego poziomu przypisany w inspektorze
    private bool[] unlockedLevels; // Tablica przechowuj¹ca stan odblokowania poziomów
    private int totalLevels = 13; // Liczba poziomów (limit do 13)
    private int currentLevelIndex; // Bie¿¹cy indeks poziomu, odczytany z PlayerPrefs

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
            return; // Zapewnia, ¿e reszta kodu siê nie wykona w przypadku zduplikowania instancji
        }

        // Za³aduj stan poziomów i aktualny poziom
        LoadLevels();
        InitializeLevels();
    }

    // Inicjalizacja poziomów
    private void InitializeLevels()
    {
        unlockedLevels = new bool[totalLevels + 1]; // Tablica od 1 do 13 (indeks 0 ignorowany)

        // Ustawienie odblokowanych poziomów do currentLevelIndex
        for (int i = 1; i <= currentLevelIndex; i++)
        {
            if (i <= totalLevels)
            {
                unlockedLevels[i] = true;
            }
        }

        // Aktualizacja interaktywnoœci przycisków
        UpdateButtonInteractivity();
    }

    // Funkcja do sprawdzenia, czy poziom jest odblokowany
    public bool IsLevelUnlocked(int levelIndex)
    {
        return levelIndex > 0 && levelIndex <= totalLevels && unlockedLevels[levelIndex];
    }

    // Funkcja odblokowuj¹ca nastêpny poziom
    public void UnlockNextLevel()
    {
        // Jeœli poziom jest wiêkszy lub równy 13, nie odblokowuj kolejnych
        if (currentLevelIndex >= totalLevels)
        {
            Debug.LogWarning("Osi¹gniêto maksymalny poziom: " + totalLevels + ". Nie mo¿na odblokowaæ kolejnych poziomów.");
            return; // Zatrzymuje funkcjê
        }

        // Odblokowanie nowego poziomu
        currentLevelIndex++;
        unlockedLevels[currentLevelIndex] = true;
        SaveLevels();
        Debug.Log("Odblokowano nowy poziom: " + currentLevelIndex);

        // Zaktualizuj interaktywnoœæ przycisków
        UpdateButtonInteractivity();
    }

    // Funkcja zapisuj¹ca stan poziomów i bie¿¹cego poziomu do PlayerPrefs
    private void SaveLevels()
    {
        for (int i = 1; i <= totalLevels; i++)
        {
            PlayerPrefs.SetInt("LevelUnlocked_" + i, unlockedLevels[i] ? 1 : 0);
        }
        PlayerPrefs.SetInt("currentLevelIndex", currentLevelIndex);
        PlayerPrefs.Save();
    }

    // Funkcja ³aduj¹ca stan poziomów i bie¿¹cego poziomu z PlayerPrefs
    private void LoadLevels()
    {
        currentLevelIndex = PlayerPrefs.GetInt("currentLevelIndex", 2);

        unlockedLevels = new bool[totalLevels + 1];
        for (int i = 1; i <= totalLevels; i++)
        {
            unlockedLevels[i] = PlayerPrefs.GetInt("LevelUnlocked_" + i, i <= 2 ? 1 : 0) == 1;
        }
    }

    // Aktualizacja interaktywnoœci przycisków na podstawie stanu odblokowania poziomów
    private void UpdateButtonInteractivity()
    {
        for (int i = 1; i <= totalLevels; i++)
        {
            if (i - 1 < levelButtons.Length)
            {
                levelButtons[i - 1].interactable = unlockedLevels[i];
            }
        }
    }
}

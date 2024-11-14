using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockManager : MonoBehaviour
{
    public static LevelUnlockManager Instance; // Singleton
    public Button[] levelButtons; // Przycisk dla ka¿dego poziomu przypisany w inspektorze
    private bool[] unlockedLevels; // Tablica przechowuj¹ca stan odblokowania poziomów
    private int totalLevels = 12; // Liczba poziomów (do dostosowania)
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
        }

        // Za³aduj stan poziomów i aktualny poziom
        LoadLevels();
        InitializeLevels();
    }

    // Inicjalizacja poziomów
    private void InitializeLevels()
    {
        unlockedLevels = new bool[totalLevels + 1]; // Tablica od 1 do 12 (indeks 0 ignorowany)

        // Ustawienie odblokowanych poziomów do currentLevelIndex
        for (int i = 1; i <= currentLevelIndex; i++)
        {
            unlockedLevels[i] = true; // Odblokowanie poziomu do currentLevelIndex
        }

        // Aktualizacja interaktywnoœci przycisków
        UpdateButtonInteractivity();
    }

    // Funkcja do sprawdzenia, czy poziom jest odblokowany
    public bool IsLevelUnlocked(int levelIndex)
    {
        return levelIndex > 0 && levelIndex < unlockedLevels.Length && unlockedLevels[levelIndex];
    }

    // Funkcja odblokowuj¹ca nastêpny poziom
    public void UnlockNextLevel()
    {
        // Po prostu zwiêkszamy currentLevelIndex
        if (currentLevelIndex < totalLevels) // Zwiêkszamy tylko, jeœli nie przekroczymy maksymalnej liczby poziomów
        {
            currentLevelIndex++;
            unlockedLevels[currentLevelIndex] = true; // Odblokowujemy poziom
            SaveLevels(); // Zapisz zmiany w PlayerPrefs
            Debug.Log("Odblokowano nowy poziom: " + currentLevelIndex);
        }

        // Zaktualizuj interaktywnoœæ przycisków
        UpdateButtonInteractivity();
    }

    // Funkcja zapisuj¹ca stan poziomów i bie¿¹cego poziomu do PlayerPrefs
    private void SaveLevels()
    {
        // Zapisz stan poziomów
        for (int i = 1; i <= totalLevels; i++)
        {
            PlayerPrefs.SetInt("LevelUnlocked_" + i, unlockedLevels[i] ? 1 : 0);
            Debug.Log("Zapisano LevelUnlocked_" + i + ": " + (unlockedLevels[i] ? 1 : 0));
        }

        // Zapisz bie¿¹cy indeks poziomu
        PlayerPrefs.SetInt("currentLevelIndex", currentLevelIndex);
        Debug.Log("Zapisano currentLevelIndex: " + currentLevelIndex);

        PlayerPrefs.Save();
    }

    // Funkcja ³aduj¹ca stan poziomów i bie¿¹cego poziomu z PlayerPrefs
    private void LoadLevels()
    {
        // Pobierz currentLevelIndex z PlayerPrefs (domyœlnie zaczynaj od poziomu 2)
        currentLevelIndex = PlayerPrefs.GetInt("currentLevelIndex", 2);
        Debug.Log("Za³adowano currentLevelIndex: " + currentLevelIndex);

        // Inicjalizacja tablicy unlockedLevels
        unlockedLevels = new bool[totalLevels + 1]; // Tablica od 1 do 12 (indeks 0 ignorowany)

        // Za³aduj stan odblokowania poziomów
        for (int i = 1; i <= totalLevels; i++)
        {
            unlockedLevels[i] = PlayerPrefs.GetInt("LevelUnlocked_" + i, i <= 2 ? 1 : 0) == 1;
            Debug.Log("Wczytano LevelUnlocked_" + i + ": " + (unlockedLevels[i] ? 1 : 0));
        }
    }

    // Aktualizacja interaktywnoœci przycisków na podstawie stanu odblokowania poziomów
    private void UpdateButtonInteractivity()
    {
        for (int i = 1; i <= totalLevels; i++) // i = 1, aby numeracja zgadza³a siê z przyciskami
        {
            if (i - 1 < levelButtons.Length) // Dopasowanie indeksów levelButtons (0-11) do unlockedLevels (1-12)
            {
                levelButtons[i - 1].interactable = unlockedLevels[i];
                Debug.Log("Przycisk poziomu " + i + " interaktywny: " + unlockedLevels[i]);
            }
        }
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector2 initialSpawnPoint;
    private Vector2 currentSpawnPoint;

    // Dodajemy zmienn¹ currentLevel, która przechowuje poziom gry
    private int currentLevel = 1; // Przyk³adowa wartoœæ pocz¹tkowa, mo¿e byæ zmieniana w trakcie gry

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Utrzymujemy obiekt pomiêdzy scenami
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Metoda do ustawiania pocz¹tkowego punktu respawnu
    public void SetInitialSpawnPoint(Vector2 point)
    {
        initialSpawnPoint = point;
        currentSpawnPoint = initialSpawnPoint;
    }

    // Metoda do aktualizacji punktu respawnu
    public void UpdateSpawnPoint(Vector2 point)
    {
        currentSpawnPoint = point;
    }

    // Metoda do pobierania bie¿¹cego punktu respawnu
    public Vector2 GetSpawnPoint()
    {
        return currentSpawnPoint;
    }

    // Dodajemy metodê do uzyskiwania aktualnego poziomu
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    // Dodajemy metodê do zmiany poziomu
    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    // Metoda, aby zapisaæ globaln¹ liczbê zgonów
    public void SaveGlobalDeathCount()
    {
        GlobalDeathCounter.SaveGlobalDeathCount(); // Wywo³anie metody statycznej
    }
}

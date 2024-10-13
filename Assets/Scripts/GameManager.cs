using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector2 initialSpawnPoint;
    private Vector2 currentSpawnPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetInitialSpawnPoint(Vector2 point)
    {
        initialSpawnPoint = point;
        currentSpawnPoint = initialSpawnPoint;
    }

    public void UpdateSpawnPoint(Vector2 point)
    {
        currentSpawnPoint = point;
    }

    public Vector2 GetSpawnPoint()
    {
        return currentSpawnPoint;
    }

    // Dodaj tê metodê, aby wywo³aæ zapis globalnych zgonów
    public void SaveGlobalDeathCount()
    {
        GlobalDeathCounter.SaveGlobalDeathCount(); // Wywo³anie metody statycznej
    }
}

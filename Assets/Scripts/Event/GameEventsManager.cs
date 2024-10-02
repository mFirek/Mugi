using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
            Destroy(gameObject); // Zniszcz duplikat
            return; // Zako�cz metod�
        }
        instance = this; // Przypisz instancj�
        DontDestroyOnLoad(gameObject); // Nie niszcz przy zmianie scen
    }

    public event Action onPlayerDeath;

    public void PlayerDeath()
    {
        Debug.Log("Wywo�ano zdarzenie �mierci gracza."); // Debug log
        onPlayerDeath?.Invoke(); // Wywo�aj zdarzenie
    }
}

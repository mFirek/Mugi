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
            return; // Zakoñcz metodê
        }
        instance = this; // Przypisz instancjê
        DontDestroyOnLoad(gameObject); // Nie niszcz przy zmianie scen
    }

    public event Action onPlayerDeath;

    public void PlayerDeath()
    {
        Debug.Log("Wywo³ano zdarzenie œmierci gracza."); // Debug log
        onPlayerDeath?.Invoke(); // Wywo³aj zdarzenie
    }
}

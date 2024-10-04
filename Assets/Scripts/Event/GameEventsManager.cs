using System;  // Add this to have access to Action
using System.Collections;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance;

    public event Action onPlayerDeath;  // We use Action for events without parameters

    private void Awake()
    {
        // Ensures that there is only one instance of GameEventsManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to trigger player death event
    public void PlayerDied()
    {
        if (onPlayerDeath != null)
        {
            onPlayerDeath.Invoke();
        }
    }
}

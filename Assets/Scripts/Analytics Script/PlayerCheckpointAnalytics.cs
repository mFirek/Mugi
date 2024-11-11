using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class PlayerCheckpointAnalytics : MonoBehaviour
{
    private static float checkpoint1Time = -1f; // Czas dotarcia do "Checkpoint1"
    private static float checkpoint2Time = -1f; // Czas dotarcia do "Checkpoint2"
    private static float checkpoint3Time = -1f; // Czas dotarcia do "Checkpoint3" (jeœli istnieje)
    private static bool checkpoint3Exists = false; // Czy istnieje "Checkpoint3"
    private static bool checkpoint4Exists = false; // Czy istnieje "Checkpoint4"

    private void Start()
    {
        // Sprawdza, czy w scenie istniej¹ obiekty o nazwie "Checkpoint3" i "Checkpoint4"
        checkpoint3Exists = GameObject.Find("Checkpoint3") != null;
        checkpoint4Exists = GameObject.Find("Checkpoint4") != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.name == "Checkpoint1")
            {
                // Rozpocznij pomiar czasu
                checkpoint1Time = Time.time;
                Debug.Log("Checkpoint1 osi¹gniêty, czas pomiaru rozpoczêty.");
            }
            else if (gameObject.name == "Checkpoint2" && checkpoint1Time >= 0)
            {
                if (checkpoint4Exists)
                {
                    // Jeœli istnieje "Checkpoint4", zapisujemy czas dotarcia do "Checkpoint2" i czekamy dalej
                    checkpoint2Time = Time.time;
                    Debug.Log("Checkpoint2 osi¹gniêty, czas zapisany, oczekiwanie na Checkpoint4.");
                }
                else if (checkpoint3Exists)
                {
                    // Jeœli istnieje "Checkpoint3", ale nie "Checkpoint4", czekamy na dotarcie do Checkpoint3
                    checkpoint2Time = Time.time;
                    Debug.Log("Checkpoint2 osi¹gniêty, czas zapisany, oczekiwanie na Checkpoint3.");
                }
                else
                {
                    // Jeœli brak "Checkpoint3" i "Checkpoint4", koñczymy pomiar przy Checkpoint2
                    float timeBetweenCheckpoints = Time.time - checkpoint1Time;
                    SendAnalytics("checkpoint_time_1_2", timeBetweenCheckpoints);
                    ResetTimes();
                }
            }
            else if (gameObject.name == "Checkpoint3" && checkpoint1Time >= 0 && checkpoint2Time >= 0)
            {
                if (checkpoint4Exists)
                {
                    // Jeœli istnieje "Checkpoint4", zapisujemy czas dotarcia do "Checkpoint3"
                    checkpoint3Time = Time.time;
                    Debug.Log("Checkpoint3 osi¹gniêty, czas zapisany, oczekiwanie na Checkpoint4.");
                }
                else
                {
                    // Jeœli brak "Checkpoint4", koñczymy pomiar przy Checkpoint3
                    float timeBetweenCheckpoints = Time.time - checkpoint1Time;
                    SendAnalytics("checkpoint_time_1_3", timeBetweenCheckpoints);
                    ResetTimes();
                }
            }
            else if (gameObject.name == "Checkpoint4" && checkpoint1Time >= 0 && checkpoint2Time >= 0 && checkpoint3Time >= 0)
            {
                // Jeœli dotarliœmy do "Checkpoint4", koñczymy pomiar przy nim
                float timeBetweenCheckpoints = Time.time - checkpoint1Time;
                SendAnalytics("checkpoint_time_1_4", timeBetweenCheckpoints);
                ResetTimes();
            }
        }
    }

    private void SendAnalytics(string eventName, float time)
    {
        // Wysy³a dane analityczne z czasem
        Dictionary<string, object> checkpointData = new Dictionary<string, object>()
        {
            { "time_between_checkpoints", time }
        };
        AnalyticsResult result = Analytics.CustomEvent(eventName, checkpointData);
        Debug.Log($"{eventName}: {time} s.");
    }

    private void ResetTimes()
    {
        // Resetuje czasy po zakoñczeniu pomiaru
        checkpoint1Time = -1f;
        checkpoint2Time = -1f;
        checkpoint3Time = -1f;
    }
}

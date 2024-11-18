using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;
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
                checkpoint1Time = Time.time;
                Debug.Log("Checkpoint1 osi¹gniêty, czas pomiaru rozpoczêty.");
            }
            else if (gameObject.name == "Checkpoint2" && checkpoint1Time >= 0)
            {
                checkpoint2Time = Time.time;
                Debug.Log("Checkpoint2 osi¹gniêty.");
                ProcessCheckpoint("checkpoint_time_1_2", checkpoint1Time, checkpoint2Time);
            }
            else if (gameObject.name == "Checkpoint3" && checkpoint1Time >= 0 && checkpoint2Time >= 0)
            {
                checkpoint3Time = Time.time;
                Debug.Log("Checkpoint3 osi¹gniêty.");
                ProcessCheckpoint("checkpoint_time_1_3", checkpoint1Time, checkpoint3Time);
            }
            else if (gameObject.name == "Checkpoint4" && checkpoint1Time >= 0 && checkpoint2Time >= 0 && checkpoint3Time >= 0)
            {
                float totalTime = Time.time - checkpoint1Time;
                SendAnalytics("checkpoint_time_1_4", totalTime);
                ResetTimes();
            }
        }
    }

    private void ProcessCheckpoint(string eventName, float startTime, float endTime)
    {
        if (checkpoint4Exists || (checkpoint3Exists && eventName == "checkpoint_time_1_2"))
        {
            Debug.Log($"Oczekiwanie na kolejne punkty kontrolne po {eventName}.");
        }
        else
        {
            float timeBetweenCheckpoints = endTime - startTime;
            SendAnalytics(eventName, timeBetweenCheckpoints);
            ResetTimes();
        }
    }

    private void SendAnalytics(string eventName, float time)
    {
        // Pobierz nazwê bie¿¹cej sceny
        string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Dodaj informacjê o scenie do danych analitycznych
        Dictionary<string, object> checkpointData = new Dictionary<string, object>()
        {
            { "time_between_checkpoints", time }, // Dopasuj nazwê do Unity Dashboard
            { "level_name", levelName } // Dodaj nazwê sceny
        };

        // Wysy³anie zdarzenia do Unity Analytics
        AnalyticsService.Instance.CustomData(eventName, checkpointData);
        AnalyticsService.Instance.Flush(); // Opcjonalne: wys³anie danych natychmiast
        Debug.Log($"{eventName}: {time} s in scene {levelName} sent to Unity Analytics.");
    }

    private void ResetTimes()
    {
        checkpoint1Time = -1f;
        checkpoint2Time = -1f;
        checkpoint3Time = -1f;
    }
}

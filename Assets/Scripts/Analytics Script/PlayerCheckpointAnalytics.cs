using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class PlayerCheckpointAnalytics : MonoBehaviour
{
    private float lastCheckpointTime;

    // Wywo³aj tê metodê przy wejœciu na checkpoint
    public void OnCheckpointReached(int checkpointID)
    {
        float currentTime = Time.time;
        float timeBetweenCheckpoints = currentTime - lastCheckpointTime;
        lastCheckpointTime = currentTime;

        Dictionary<string, object> checkpointData = new Dictionary<string, object>()
        {
            { "checkpoint_id", checkpointID },
            { "time_between_checkpoints", timeBetweenCheckpoints }
        };

        AnalyticsResult result = Analytics.CustomEvent("checkpoint_time", checkpointData);
        Debug.Log("Czas miêdzy checkpointami: " + timeBetweenCheckpoints + " s.");
    }
}

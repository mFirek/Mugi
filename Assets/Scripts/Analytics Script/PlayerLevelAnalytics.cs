using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class PlayerLevelAnalytics : MonoBehaviour
{
    public void SendLevelEndEvent(int level, float completionTime, int deaths, int itemsCollected)
    {
        Dictionary<string, object> levelEndData = new Dictionary<string, object>()
        {
            { "level", level },
            { "completion_time", completionTime },
            { "deaths", deaths },
            { "items_collected", itemsCollected }
        };

        AnalyticsResult result = Analytics.CustomEvent("level_end", levelEndData);
        Debug.Log("Poziom " + level + " zakoñczony.");
    }
}

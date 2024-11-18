using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsInitializer : MonoBehaviour
{
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection(); // Rozpocznij zbieranie danych
            Debug.Log("Unity Services initialized and data collection started.");
        }
        catch (ConsentCheckException e)
        {
            Debug.LogError($"Consent check failed: {e.Message}");
        }
    }
}

using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Threading.Tasks;

public class AnalyticsInitializer : MonoBehaviour
{
    private async void Start()
    {
        await InitializeUnityServices();
    }

    private async Task InitializeUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
            if (Unity.Services.Analytics.AnalyticsService.Instance != null)
            {
                Debug.Log("Unity Analytics zosta³o zainicjalizowane.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("B³¹d inicjalizacji Unity Services: " + e.Message);
        }
    }
}

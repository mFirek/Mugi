using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;

public class PlayerDeathAnalytics : MonoBehaviour
{
    private async void Start()
    {
        // Inicjalizacja us³ug Unity
        await InitializeUnityServices();
    }

    private async System.Threading.Tasks.Task InitializeUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("Unity Services zosta³y pomyœlnie zainicjalizowane.");
        }
        catch (ConsentCheckException e)
        {
            Debug.LogError($"B³¹d inicjalizacji Unity Services: {e.Message}");
        }
    }

    // Metoda obs³uguj¹ca œmieræ gracza
    public void ReportPlayerDeath(Collision2D collision)
    {
        // Pobieranie szczegó³ów kolizji
        string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Vector3 deathPosition = collision.transform.position;
        GameObject killerObject = collision.otherCollider.gameObject;

        string causeOfDeathName = killerObject.name;
        string causeOfDeathTag = killerObject.tag;

        // Log szczegó³ów
        Debug.Log($"Gracz zgin¹³ na poziomie: {levelName}");
        Debug.Log($"Pozycja œmierci: {deathPosition}");
        Debug.Log($"Przyczyna œmierci - Obiekt: {causeOfDeathName}, Tag: {causeOfDeathTag}");

        // Wys³anie zdarzenia do Unity Analytics
        SendPlayerDeathEvent(levelName, deathPosition, causeOfDeathName, causeOfDeathTag);
    }

    public void SendPlayerDeathEvent(string levelName, Vector3 deathPosition, string causeOfDeathName, string causeOfDeathTag)
    {
        Dictionary<string, object> deathData = new Dictionary<string, object>
        {
            { "level_name", levelName },
            { "position_x", deathPosition.x },
            { "position_y", deathPosition.y },
            { "position_z", deathPosition.z },
            { "cause_of_death_name", causeOfDeathName },
            { "cause_of_death_tag", causeOfDeathTag }
        };

        try
        {
            AnalyticsService.Instance.CustomData("player_death", deathData);
            AnalyticsService.Instance.Flush();
            Debug.Log("Zdarzenie 'player_death' zosta³o wys³ane.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"B³¹d podczas wysy³ania zdarzenia 'player_death': {e.Message}");
        }
    }
}

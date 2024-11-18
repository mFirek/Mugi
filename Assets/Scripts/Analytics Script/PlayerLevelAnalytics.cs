using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PlayerLevelAnalytics : MonoBehaviour
{
    private TimerText timer;
    private DeathCountText deathCounter;

    private async void Start()
    {
        // Inicjalizowanie Unity Services
        await InitializeUnityServices();

        // Znajdujemy obiekt o nazwie „Timer” i pobieramy jego komponent TimerText
        GameObject timerObject = GameObject.Find("Timer");
        if (timerObject != null)
        {
            timer = timerObject.GetComponent<TimerText>();
        }
        else
        {
            Debug.LogError("Nie znaleziono obiektu o nazwie 'Timer' na scenie!");
        }

        // Znajdujemy obiekt o nazwie „Death Counter” i pobieramy jego komponent DeathCountText
        GameObject deathCounterObject = GameObject.Find("Death Counter");
        if (deathCounterObject != null)
        {
            deathCounter = deathCounterObject.GetComponent<DeathCountText>();
        }
        else
        {
            Debug.LogError("Nie znaleziono obiektu o nazwie 'Death Counter' na scenie!");
        }
    }

    private async Task InitializeUnityServices()
    {
        try
        {
            // Inicjalizowanie Unity Services
            await UnityServices.InitializeAsync();
            GiveConsent(); // Udziel zgody na zbieranie danych
            Debug.Log("Unity Services zosta³y pomyœlnie zainicjalizowane.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"B³¹d inicjalizacji Unity Services: {e.Message}");
        }
    }

    private void GiveConsent()
    {
        // Rozpoczynamy zbieranie danych analitycznych
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("Zgoda na zbieranie danych zosta³a udzielona.");
    }

    public void SendLevelEndEvent()
    {
        // Pobranie informacji o poziomie
        string levelName = SceneManager.GetActiveScene().name; // Pobierz nazwê sceny (poziomu)

        // Pobieranie licznika zgonów
        int deaths = deathCounter.GetDeathCount();

        // Pobieranie czasu ukoñczenia poziomu
        float completionTime = timer.GetElapsedTime();

        // Tworzymy s³ownik, w którym przechowamy dane
        Dictionary<string, object> levelEndData = new Dictionary<string, object>()
        {
            { "level_name", levelName },
            { "completion_time", completionTime },
            { "deaths", deaths }
        };

        // Wysy³anie danych analitycznych do Unity Analytics Cloud
        try
        {
            Debug.Log("Próba wys³ania zdarzenia 'level_end'...");
            AnalyticsService.Instance.CustomData("level_end", levelEndData);
            AnalyticsService.Instance.Flush(); // Natychmiastowe wysy³anie zdarzenia
            Debug.Log($"Zdarzenie 'level_end' zosta³o wys³ane: {levelEndData}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"B³¹d podczas wysy³ania zdarzenia 'level_end': {e.Message}");
        }
    }
}

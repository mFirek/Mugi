using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;

public class PlayerDeathAnalytics : MonoBehaviour
{
    private async void Start()
    {
        // Inicjalizacja Unity Services
        await InitializeUnityServices();
    }

    private async System.Threading.Tasks.Task InitializeUnityServices()
    {
        try
        {
            // Inicjalizowanie us³ug Unity
            await UnityServices.InitializeAsync();
            GiveConsent(); // Udziel zgody na zbieranie danych
            Debug.Log("Unity Services zosta³y pomyœlnie zainicjalizowane.");
        }
        catch (ConsentCheckException e)
        {
            Debug.LogError($"B³¹d inicjalizacji Unity Services: {e.Message}");
        }
    }

    private void GiveConsent()
    {
        // Rozpoczêcie zbierania danych po udzieleniu zgody
        AnalyticsService.Instance.StartDataCollection();
        Debug.Log("Zgoda na zbieranie danych zosta³a udzielona.");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Lava"))
            {
                // Pobierz aktualn¹ nazwê poziomu
                string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                Debug.Log($"Gracz zgin¹³ na poziomie: {levelName}");

                // Pobierz pozycjê gracza w momencie œmierci
                Vector3 deathPosition = collision.transform.position;
                Debug.Log($"Pozycja œmierci: {deathPosition}");

                // Pobierz nazwê obiektu, z którym gracz zderzy³ siê
                string causeOfDeath = collision.gameObject.name;
                Debug.Log($"Przyczyna œmierci: {causeOfDeath}");

                // Wyœlij zdarzenie analityczne
                SendPlayerDeathEvent(levelName, deathPosition, causeOfDeath);
            }
        }
    }

    public void SendPlayerDeathEvent(string levelName, Vector3 deathPosition, string causeOfDeath)
    {
        // Przygotowanie danych zdarzenia
        Dictionary<string, object> deathData = new Dictionary<string, object>
        {
            { "level_name", levelName },
            { "position_x", deathPosition.x },
            { "position_y", deathPosition.y },
            { "position_z", deathPosition.z },
            { "cause_of_death", causeOfDeath }
        };

        // Log przed wys³aniem zdarzenia
        Debug.Log("Przygotowanie do wys³ania zdarzenia 'player_death'...");

        // Próba wys³ania zdarzenia do Unity Analytics
        try
        {
            // Wysy³anie zdarzenia
            AnalyticsService.Instance.CustomData("player_death", deathData);
            AnalyticsService.Instance.Flush(); // Natychmiastowe wys³anie zdarzenia

            // Log po wys³aniu
            Debug.Log($"Zdarzenie 'player_death' zosta³o wys³ane pomyœlnie. Dane: {deathData}");
        }
        catch (System.Exception e)
        {
            // B³¹d podczas wysy³ania
            Debug.LogError($"B³¹d podczas wysy³ania zdarzenia 'player_death': {e.Message}");
        }
    }
}

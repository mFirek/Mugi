using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PlayerBossAnalytics : MonoBehaviour
{
    private static PlayerBossAnalytics instance;

    public static PlayerBossAnalytics Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerBossAnalytics>();
            }
            return instance;
        }
    }

    private int attemptsCount = 0; // Liczba prób (zgony)
    private int currentLevel = 1;  // Numer poziomu
    private string levelName;      // Nazwa poziomu

    // Referencja do skryptu licz¹cego zgony
    private DeathCountText deathCountTextScript;

    private async void Start()
    {
        // Inicjalizowanie Unity Services
        await InitializeUnityServices();

        // Pobranie numeru poziomu z nazwy sceny
        levelName = SceneManager.GetActiveScene().name;
        if (int.TryParse(levelName.Substring(levelName.Length - 1), out currentLevel))
        {
            Debug.Log($"Aktualny poziom: {currentLevel}");
        }
        else
        {
            Debug.LogError("Nie uda³o siê uzyskaæ numeru poziomu z nazwy sceny!");
            currentLevel = 1; // Domyœlny numer poziomu
        }

        // Pobranie referencji do skryptu licz¹cego zgony
        deathCountTextScript = FindObjectOfType<DeathCountText>();
        if (deathCountTextScript != null)
        {
            attemptsCount = deathCountTextScript.GetDeathCount();
            Debug.Log($"Pobrana liczba zgonów (prób): {attemptsCount}");
        }
        else
        {
            Debug.LogError("Nie znaleziono skryptu DeathCountText!");
        }
    }

    private async Task InitializeUnityServices()
    {
        try
        {
            // Inicjalizowanie Unity Services
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services zosta³y pomyœlnie zainicjalizowane.");
            GiveConsent(); // Udziel zgody na zbieranie danych
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

    // Wywo³ywana po pokonaniu bossa
    public void SendBossDefeatEvent()
    {
        if (deathCountTextScript != null)
        {
            attemptsCount = deathCountTextScript.GetDeathCount(); // Aktualizacja liczby prób
        }

        // Przygotowanie danych zdarzenia
        Dictionary<string, object> bossDefeatData = new Dictionary<string, object>
        {
            { "level_name", levelName },    // Numer poziomu
            { "attempts", attemptsCount }   // Liczba podejœæ
        };

        try
        {
            // Wysy³anie danych analitycznych do Unity Analytics
            Debug.Log("Próba wys³ania zdarzenia 'boss_defeated'...");
            AnalyticsService.Instance.CustomData("boss_defeated", bossDefeatData);
            AnalyticsService.Instance.Flush(); // Natychmiastowe wysy³anie danych
            Debug.Log($"Zdarzenie 'boss_defeated' zosta³o wys³ane: Poziom {currentLevel}, Próby {attemptsCount}.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"B³¹d podczas wysy³ania zdarzenia 'boss_defeated': {e.Message}");
        }

        // Opcjonalne: Zapisz lokalne dane (np. w PlayerPrefs)
        PlayerPrefs.SetInt($"BossDefeatAttempts_Level{currentLevel}", attemptsCount);
    }
}

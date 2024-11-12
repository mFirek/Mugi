using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class PlayerLevelAnalytics : MonoBehaviour
{
    private TimerText timer;
    private DeathCountText deathCounter;

    private void Start()
    {
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

    public void SendLevelEndEvent()
    {
        // Pobranie informacji o poziomie
        string levelName = SceneManager.GetActiveScene().name; // Pobierz nazwê sceny (poziomu)

        // Pobieranie licznika zgonów za pomoc¹ nazwy obiektu "Death Counter"
        GameObject deathCounterObj = GameObject.Find("Death Counter");
        DeathCountText deathCountText = deathCounterObj.GetComponent<DeathCountText>();
        int deaths = deathCountText.GetDeathCount();

        // Pobieranie czasu ukoñczenia poziomu za pomoc¹ nazwy obiektu "Timer"
        GameObject timerObj = GameObject.Find("Timer");
        TimerText timerText = timerObj.GetComponent<TimerText>();
        float completionTime = timerText.GetElapsedTime();

        // Tworzymy s³ownik, w którym przechowamy dane
        Dictionary<string, object> levelEndData = new Dictionary<string, object>()
        {
            { "level", levelName },
            { "completion_time", completionTime },
            { "deaths", deaths }
        };

        // Wysy³amy dane analityczne
        AnalyticsResult result = Analytics.CustomEvent("level_end", levelEndData);
        Debug.Log($"Poziom {levelName} zakoñczony. Czas: {completionTime}s, Zgony: {deaths}");
    }
}

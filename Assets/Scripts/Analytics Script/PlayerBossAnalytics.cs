using UnityEngine;
using UnityEngine.SceneManagement;

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

    private int attemptsCount = 0; // Liczba prób (równa liczbie zgonów)
    private int currentLevel = 1; // Numer poziomu

    // Referencja do skryptu licz¹cego zgony
    private DeathCountText deathCountTextScript;

    private void Start()
    {
        // Pobierz numer poziomu z nazwy sceny
        string sceneName = SceneManager.GetActiveScene().name;
        if (int.TryParse(sceneName.Substring(sceneName.Length - 1), out currentLevel))
        {
            Debug.Log("Aktualny poziom: " + currentLevel);
        }
        else
        {
            Debug.LogError("Nie uda³o siê uzyskaæ numeru poziomu z nazwy sceny!");
            currentLevel = 1; // Domyœlny poziom, jeœli nie uda³o siê wyci¹gn¹æ numeru
        }

        // Pobierz referencjê do skryptu licz¹cego zgony
        deathCountTextScript = FindObjectOfType<DeathCountText>();
        if (deathCountTextScript != null)
        {
            // Ustaw liczbê prób na liczbê zgonów
            attemptsCount = deathCountTextScript.GetDeathCount();
            Debug.Log("Pobrana liczba zgonów (prób): " + attemptsCount);
        }
        else
        {
            Debug.LogError("Nie znaleziono skryptu DeathCountText!");
        }
    }

    // Metoda wywo³ywana przy pokonaniu bossa
    public void SendBossDefeatEvent()
    {
        // Pobierz aktualn¹ liczbê zgonów, aby odœwie¿yæ attemptsCount
        if (deathCountTextScript != null)
        {
            attemptsCount = deathCountTextScript.GetDeathCount();
        }

        // Wyœwietl dane
        Debug.Log($"Boss pokonany po {attemptsCount} podejœciu na poziomie {currentLevel}.");

        // Zapisz dane do PlayerPrefs, jeœli jest to potrzebne
        PlayerPrefs.SetInt($"BossDefeatAttempts_Level{currentLevel}", attemptsCount);
    }
}

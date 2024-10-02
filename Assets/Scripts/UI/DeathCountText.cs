using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCountText : MonoBehaviour, IDataPresistence
{
    private int deathCount = 0;  // Licznik zgon�w
    private TextMeshProUGUI deathCountText;  // Referencja do komponentu TextMeshProUGUI

    private void Awake()
    {
        // Pobierz komponent TextMeshProUGUI
        deathCountText = GetComponent<TextMeshProUGUI>();

        if (deathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta� znaleziony na obiekcie!");
        }
    }

    // Metoda do �adowania danych
    public void LoadData(GameData gameData)
    {
        this.deathCount = gameData.deathCount;  // Przypisz warto�� zgon�w z GameData
        Debug.Log("Za�adowano dane: liczba zgon�w = " + this.deathCount);
        UpdateDeathCountText();  // Zaktualizuj tekst
    }

    // Metoda do zapisywania danych
    public void SaveData(ref GameData data)
    {
        data.deathCount = this.deathCount;  // Zapisz warto�� zgon�w do GameData
    }

    private void Start()
    {
        // Subskrybuj zdarzenie �mierci gracza
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.onPlayerDeath += OnPlayerDeath;
            Debug.Log("Zarejestrowano OnPlayerDeath w GameEventsManager.");
        }
        else
        {
            Debug.LogError("Nie znaleziono instancji GameEventsManager!");
        }

        // Inicjalizuj tekst na starcie
        UpdateDeathCountText();
    }

    private void OnDestroy()
    {
        // Odsubskrybuj zdarzenie �mierci gracza
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.onPlayerDeath -= OnPlayerDeath;
        }
    }

    private void OnPlayerDeath()
    {
        deathCount++;  // Zwi�ksz licznik zgon�w
        Debug.Log("Gracz zgin��. Liczba zgon�w: " + deathCount);  // Loguj liczb� zgon�w
        UpdateDeathCountText();  // Zaktualizuj tekst
    }

    private void UpdateDeathCountText()
    {
        // Ustaw tekst na "Deaths: X"
        deathCountText.text = "Deaths: " + deathCount;
    }
}

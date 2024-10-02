using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCountText : MonoBehaviour, IDataPresistence
{
    private int deathCount = 0;  // Licznik zgonów
    private TextMeshProUGUI deathCountText;  // Referencja do komponentu TextMeshProUGUI

    private void Awake()
    {
        // Pobierz komponent TextMeshProUGUI
        deathCountText = GetComponent<TextMeshProUGUI>();

        if (deathCountText == null)
        {
            Debug.LogError("TextMeshProUGUI nie zosta³ znaleziony na obiekcie!");
        }
    }

    // Metoda do ³adowania danych
    public void LoadData(GameData gameData)
    {
        this.deathCount = gameData.deathCount;  // Przypisz wartoœæ zgonów z GameData
        Debug.Log("Za³adowano dane: liczba zgonów = " + this.deathCount);
        UpdateDeathCountText();  // Zaktualizuj tekst
    }

    // Metoda do zapisywania danych
    public void SaveData(ref GameData data)
    {
        data.deathCount = this.deathCount;  // Zapisz wartoœæ zgonów do GameData
    }

    private void Start()
    {
        // Subskrybuj zdarzenie œmierci gracza
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
        // Odsubskrybuj zdarzenie œmierci gracza
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.onPlayerDeath -= OnPlayerDeath;
        }
    }

    private void OnPlayerDeath()
    {
        deathCount++;  // Zwiêksz licznik zgonów
        Debug.Log("Gracz zgin¹³. Liczba zgonów: " + deathCount);  // Loguj liczbê zgonów
        UpdateDeathCountText();  // Zaktualizuj tekst
    }

    private void UpdateDeathCountText()
    {
        // Ustaw tekst na "Deaths: X"
        deathCountText.text = "Deaths: " + deathCount;
    }
}

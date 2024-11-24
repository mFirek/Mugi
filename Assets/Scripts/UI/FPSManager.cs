using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown fpsDropdown; // Dropdown dla wyboru limitu FPS
    private List<int> fpsOptions = new List<int> { 120, 60, 30 }; // Dostêpne opcje FPS
    private int defaultFPSIndex = 2; // Indeks domyœlnej wartoœci FPS (30 FPS)

    void Start()
    {
        // Ustawienie dostêpnych opcji w dropdownie
        fpsDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (int fps in fpsOptions)
        {
            options.Add(fps + " FPS");
        }

        fpsDropdown.AddOptions(options);

        // Odczytanie zapisanej wartoœci FPS lub ustawienie domyœlnego indeksu
        int savedFPSIndex = PlayerPrefs.GetInt("SelectedFPSIndex", defaultFPSIndex);
        fpsDropdown.value = savedFPSIndex;
        fpsDropdown.RefreshShownValue();
        SetFPSLimit(savedFPSIndex);

        // Subskrypcja zdarzenia zmiany wartoœci dropdowna
        fpsDropdown.onValueChanged.AddListener(SetFPSLimit);
    }

    public void SetFPSLimit(int fpsIndex)
    {
        int selectedFPS = fpsOptions[fpsIndex];
        Application.targetFrameRate = selectedFPS; // Ustawienie limitu FPS
        PlayerPrefs.SetInt("SelectedFPSIndex", fpsIndex); // Zapisanie wyboru
        PlayerPrefs.Save(); // Zapisanie do trwa³ego magazynu
        Debug.Log("FPS limit set to: " + selectedFPS);
    }
}

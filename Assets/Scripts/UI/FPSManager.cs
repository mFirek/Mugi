using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown fpsDropdown; // Dropdown dla wyboru limitu FPS
    private List<int> fpsOptions = new List<int> { 30, 60, 120 }; // Dostêpne opcje FPS
    private int currentFPSIndex = 0;

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

        // Ustawienie domyœlnej wartoœci dropdowna i wywo³anie metody ustawiaj¹cej limit FPS
        fpsDropdown.value = currentFPSIndex;
        fpsDropdown.RefreshShownValue();
        SetFPSLimit(currentFPSIndex);

        // Subskrypcja zdarzenia zmiany wartoœci dropdowna
        fpsDropdown.onValueChanged.AddListener(SetFPSLimit);
    }

    public void SetFPSLimit(int fpsIndex)
    {
        int selectedFPS = fpsOptions[fpsIndex];
        Application.targetFrameRate = selectedFPS; // Ustawienie limitu FPS
        Debug.Log("FPS limit set to: " + selectedFPS);
    }
}

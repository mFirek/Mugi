using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;

    private void Start()
    {
        if(!DataPresistenceManager.instance.HasGameData())
        {
            loadGameButton.interactable = false;
        }
    }
    public void NewGame()
    {
        
        DisableMenuButtons();
        //stwórz now¹ grê co rozpocznie inicjalizacjê danych gry
        DataPresistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("SampleScene");
    }


    public void QuitGame()
        {
        Application.Quit();

    }

    public void LoadGame()
    {   
        DisableMenuButtons();
        SceneManager.LoadSceneAsync("SampleScene");
    }
    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        loadGameButton.interactable = false;
    }
}

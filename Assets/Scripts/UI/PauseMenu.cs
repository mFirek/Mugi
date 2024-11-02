using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;  // Dodane: okno opcji

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // Jeœli okno opcji jest aktywne, wy³¹cz je i wznow grê
            if (optionsMenuUI.activeSelf)
            {
                CloseOptionsMenu();
                Resume();
            }
            else
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void OpenOptionsMenu()
    {
        optionsMenuUI.SetActive(true); // W³¹cza okno opcji
    }

    public void CloseOptionsMenu()
    {
        optionsMenuUI.SetActive(false); // Wy³¹cza okno opcji
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public int level { get; private set; }
    public int lives { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
    private void Start()
    {
        Application.targetFrameRate = 60;

        NewGame();
    }
    private void NewGame()
    {
        lives = 1;
        loadlevel(1);
    }
    public void loadlevel(int level)
    {
        this.level = level;
        SceneManager.LoadScene($"{level}");
    }


}


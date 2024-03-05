using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }

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
        lives = 3;
        coins = 0;
        loadlevel(1,1);
    }
    public void loadlevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
    }
    
    public void ResetLevel(float delay)
    {
        Invoke(nameof(ResetLevel),delay);
    }

    public void NextLevel()
    {
        loadlevel(world, stage + 1);
    }

    public void ResetLevel()
    {
        lives--;
        if (lives>0)
        {
            loadlevel(world, stage);
        }
        else
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        NewGame();
    }
    public void AddCoin()
    {
        coins++;
        if (coins==100)
        {
            AddLife();
            coins = 0;
        }
    }
    public void AddLife()
    {
        lives++;
    }
}


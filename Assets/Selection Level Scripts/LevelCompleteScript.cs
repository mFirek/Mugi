using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteScript : MonoBehaviour
{ 
    public void OnLevelComplete()
    {
        if(LevelSelectionMenuManager.currLevel == LevelSelectionMenuManager.UnlockedLevels)
        {
            LevelSelectionMenuManager.UnlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", LevelSelectionMenuManager.UnlockedLevels);
        }
        SceneManager.LoadScene("Menu");
    }
   
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}

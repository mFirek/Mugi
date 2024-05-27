using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuManager : MonoBehaviour
{
    public LevelObject[] levelObjects;
    public static int currLevel;
    public static int UnlockedLevels;
    public void OnClickLevel(int levelNum)
    {   
        currLevel = levelNum;
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickBack()
    {
        this.gameObject.SetActive(false);
    }
   
    void Start()
    {
        UnlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        for(int i = 0;i<levelObjects.Length;i++)
        {
            if (UnlockedLevels>=i)
            {
                levelObjects[i].levelButton.interactable = true;
            }
        }
    }

   
    void Update()
    {
        
    }
}

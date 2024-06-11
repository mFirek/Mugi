using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuManager : MonoBehaviour
{
    public LevelObject[] levelObjects; // Tablica z obiektami poziom�w
    public string[] levelSceneNames; // Tablica z nazwami scen dla poziom�w
    public static int currLevel;
    public static int UnlockedLevels;

    public void OnClickLevel(int levelNum)
    {
        currLevel = levelNum;
        if (levelNum >= 0 && levelNum < levelSceneNames.Length)
        {
            string sceneName = levelSceneNames[levelNum]; // Pobieramy nazw� sceny z tablicy
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Nieprawid�owy numer poziomu: " + levelNum);
        }
    }

    public void OnClickBack()
    {
        this.gameObject.SetActive(false);
    }

    void Start()
    {
        UnlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        for (int i = 0; i < levelObjects.Length; i++)
        {
            if (UnlockedLevels >= i)
            {
                levelObjects[i].levelButton.interactable = true;
                int levelIndex = i; // Tworzymy lokaln� kopi� indeksu, aby unikn�� problem�w z przekazywaniem lambdy
                levelObjects[i].levelButton.onClick.AddListener(() => OnClickLevel(levelIndex));
            }
        }
    }
}

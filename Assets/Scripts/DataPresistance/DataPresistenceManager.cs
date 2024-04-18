using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPresistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    private GameData gameData;

    private List <IDataPresistence> dataPresistencesObjects;
    private FileDataHandler dataHandler;
 public static DataPresistenceManager instance {  get; private set; }
    private void Awake()
    {   
        if (instance != null)
        {
            Debug.Log("Found more than one Data Presistance Manager in the scene. Destroying the newest one. ");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.productName, fileName, useEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        this.dataPresistencesObjects = FindAllDataPresistenceObjects();
        LoadGame();
    }
    public void OnSceneUnloaded(Scene scene)
    {
        
        SaveGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //Za³aduj ka¿de zapisane dane z pliku u¿ywaj¹c data handler
        this.gameData = dataHandler.Load();

        //zacznij now¹ gê jeœli dane s¹ puste i jeœli zkonfigurujemy do inicjalizacji danych z powodu debuggowania
        if(this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        //je¿eli nie ma ¿adnych danych, wyœwietl log o braku danych
        if(this.gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded. ");
            return;
        }
        //pchnij za³adowane dane do wszystkich skryptów, które tego potrzebuj¹
        foreach(IDataPresistence dataPresistenceObj in dataPresistencesObjects)
        {
            dataPresistenceObj.LoadData(gameData);
        }
      
    }
    public void SaveGame()
    {   
        //je¿eli nie ma ¿adnych danych do zapisania, wyœwietl ostrze¿enie w logu
        if(this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved. ");
            return;
        }

        //  udostêpnij dane do innych skryptów w celu ich aktualizacji
        foreach(IDataPresistence dataPresistanceObj in dataPresistencesObjects)
        {
            dataPresistanceObj.SaveData(ref gameData);
        }
        
        //zapisz te dane do pliku u¿ywaj¹c data handler
        dataHandler.Save(gameData);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<IDataPresistence> FindAllDataPresistenceObjects()
    {
        IEnumerable<IDataPresistence> dataPresistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPresistence>();

        return new List<IDataPresistence>(dataPresistanceObjects);
    }
    public bool HasGameData()
    {
        return gameData != null;
    }
}

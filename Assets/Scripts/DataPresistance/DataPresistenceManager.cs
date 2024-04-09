using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPresistenceManager : MonoBehaviour
{
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
            Debug.LogError("Found more than one Data Presistance Manager in the scene.");
        }
        instance = this; 
    }

    private void Start()
    {   
        this.dataHandler = new FileDataHandler(Application.productName, fileName, useEncryption);
        this.dataPresistencesObjects = FindAllDataPresistenceObjects();
        LoadGame();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //Za³aduj ka¿de zapisane dane z pliku u¿ywaj¹c data handler
        this.gameData = dataHandler.Load();
        //je¿eli nie ma ¿adnych danych za³aduj now¹ grê
        if(this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }
        //pchnij za³adowane dane do wszystkich skryptów, które tego potrzebuj¹
        foreach(IDataPresistence dataPresistenceObj in dataPresistencesObjects)
        {
            dataPresistenceObj.LoadData(gameData);
        }
      
    }
    public void SaveGame()
    {
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
}

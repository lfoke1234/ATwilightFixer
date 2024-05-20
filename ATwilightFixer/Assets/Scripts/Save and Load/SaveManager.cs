using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;

    private GameData gameData;

    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("DeleteSaveFile")]
    private void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataHandler.Delete();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        DontDestroyOnLoad(this);

        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        saveManagers = FindAllSaveManagers();

        LoadGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SaveGame();
        }

    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No saved data found!");
            NewGame();
        }

        saveManagers = FindAllSaveManagers();
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

        // saveManagers = FindAllSaveManagers(); // saveManagers¸¦ °»½Å
        // foreach (ISaveManager saveManager in saveManagers)
        // {
        //     saveManager.LoadData(gameData);
        // }
    }

    public void SaveGame()
    {
        saveManagers = FindAllSaveManagers();
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    public void SaveSpecificScript(string scriptName)
    {
        saveManagers = FindAllSaveManagers();
        ISaveManager specificSaveManager = saveManagers.FirstOrDefault(sm => sm.GetType().Name == scriptName);

        if (specificSaveManager != null)
        {
            specificSaveManager.SaveData(ref gameData);
            dataHandler.Save(gameData);
        }
        else
        {
            Debug.LogWarning("No SaveManager found with the script name: " + scriptName);
        }
    }

    private void OnApplicationQuit()
    {
        if (SceneManager.sceneCount != 0)
        {
            SaveGame();
        }
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }
}

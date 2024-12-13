using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName; // ���� ���� �̸�

    private GameData gameData;
    private List<ISaveManager> saveManagers; // ISaveManager �������̽��� ��ӹ޴� ��� ��ü ���
    private FileDataHandler dataHandler;

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
            SaveGame(); // U Ű�� ���� ���� �����͸� �����մϴ�.
        }
    }

    // ���� ������ ����
    [ContextMenu("DeleteSaveFile")]
    private void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataHandler.Delete();
    }

    // ù ����� ���ο� ���� �����͸� ����
    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        // ���������� ������� ������ ����
        if (this.gameData == null)
        {
            Debug.Log("No saved data found!");
            NewGame();
        }

        // ISaveManager�� ��ӹ��� ��ũ��Ʈ Ž���� ����
        saveManagers = FindAllSaveManagers();

        // ������ �ҷ�����
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // ISaveManager�� ��ӹ��� ��ũ��Ʈ Ž���� ����
        saveManagers = FindAllSaveManagers();

        // ������ ����
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    // Ư�� ��ũ��Ʈ�� �����͸� ����
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

    // ���ø����̼��� ����� �� �����͸� ����
    private void OnApplicationQuit()
    {
        if (SceneManager.sceneCount != 0)
        {
            SaveGame();
        }
    }

    // ��� ISaveManager�� ��ӹ��� ��ü���� ã�� ����Ʈ�� ��ȯ
    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }
}

using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName; // 저장 파일 이름

    private GameData gameData;
    private List<ISaveManager> saveManagers; // ISaveManager 인터페이스를 상속받는 모든 객체 목록
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
            SaveGame(); // U 키를 눌러 게임 데이터를 저장합니다.
        }
    }

    // 저장 파일을 삭제
    [ContextMenu("DeleteSaveFile")]
    private void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataHandler.Delete();
    }

    // 첫 실행시 새로운 게임 데이터를 생성
    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        // 저장파일이 없을경우 데이터 생성
        if (this.gameData == null)
        {
            Debug.Log("No saved data found!");
            NewGame();
        }

        // ISaveManager를 상속받은 스크립트 탐색후 저장
        saveManagers = FindAllSaveManagers();

        // 데이터 불러오기
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // ISaveManager를 상속받은 스크립트 탐색후 저장
        saveManagers = FindAllSaveManagers();

        // 데이터 저장
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    // 특정 스크립트의 데이터만 저장
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

    // 애플리케이션이 종료될 때 데이터를 저장
    private void OnApplicationQuit()
    {
        if (SceneManager.sceneCount != 0)
        {
            SaveGame();
        }
    }

    // 모든 ISaveManager를 상속받은 객체들을 찾아 리스트로 반환
    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }
}

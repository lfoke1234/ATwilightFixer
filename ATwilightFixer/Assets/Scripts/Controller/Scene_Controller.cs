using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Controller : MonoBehaviour, ISaveManager
{
    [SerializeField] private int nextSceneNum;
    [SerializeField] private int clearGold;
    protected string currentStageName;
    protected bool clear;

    private void Start()
    {
        currentStageName = SceneManager.GetActiveScene().name;
        SaveManager.instance.LoadGame();
    }

    public void LoadData(GameData _data)
    {
        if (!string.IsNullOrEmpty(currentStageName) && _data.clearStage.ContainsKey(currentStageName))
        {
            clear = _data.clearStage[currentStageName];
        }
        else
        {
            Debug.Log("Current stage name is null or empty, or the key does not exist in the dictionary.");
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (!string.IsNullOrEmpty(currentStageName))
        {
            bool success = _data.clearStage.TryAdd(currentStageName, clear);
            if (!success)
            {
                _data.clearStage[currentStageName] = clear;
            }
        }
        else
        {
            Debug.Log("Current stage name is null or empty.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (!clear)
            {
                PlayerManager.instance.currency += clearGold;
            }
            else
            {
                PlayerManager.instance.currency += clearGold / 5;
            }

            clear = true;

            SaveManager.instance.SaveGame();
            SceneManager.LoadScene("MainMenu");
        }
    }
}

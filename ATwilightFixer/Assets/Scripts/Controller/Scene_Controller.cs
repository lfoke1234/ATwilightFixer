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
    }

    public void LoadData(GameData _data)
    {
    }

    public void SaveData(ref GameData _data)
    {
        _data.clearStage.Add(currentStageName, clear);
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

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour, ISaveManager
{
    [SerializeField] private Button start;
    [SerializeField] private Button option;
    [SerializeField] private Button quit;
    private bool isClearTutorial;

    public void LoadData(GameData _data)
    {
        isClearTutorial = _data.clearStage.ContainsKey("Stage 1") && _data.clearStage["Stage 1"];
    }

    public void SaveData(ref GameData _data)
    {
    }

    void Start()
    {
        start.onClick.AddListener(() => StartGame());
        quit.onClick.AddListener(() => Application.Quit());
    }

    private void StartGame()
    {
        if (isClearTutorial == false)
        {
            SceneManager.LoadScene("Prologue");
        }
        else
        {
            SaveManager.instance.LoadGame();
            SceneManager.LoadScene("MainMenu");
        }
    }
}

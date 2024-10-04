using RPG.VisualNovel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour, ISaveManager
{
    [SerializeField] private Button start;
    [SerializeField] private Button option;
    [SerializeField] private Button quit;

    [SerializeField] private bool nextSceneisTitle;
    [SerializeField] private StoryScene nextNovelScript;
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
            NovelScriptManager.Instance.nextPlayScene = nextNovelScript;
            NovelScriptManager.Instance.nextSceneisTitle = false;
            NovelScriptManager.Instance.nextSceneName = "Stage 1";
            SceneManager.LoadScene("VisualNovel");
        }
        else
        {
            SaveManager.instance.LoadGame();
            SceneManager.LoadScene("MainMenu");
        }
    }
}

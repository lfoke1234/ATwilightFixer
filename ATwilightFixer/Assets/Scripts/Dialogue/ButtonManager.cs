using RPG.VisualNovel;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{
    [SerializeField] private StoryScene stage01;
    [SerializeField] private StoryScene stage02;
    [SerializeField] private StoryScene stage03;
    [SerializeField] private StoryScene stage04;
    [SerializeField] private StoryScene stage05;
    [SerializeField] private StoryScene stage06;
    [SerializeField] private StoryScene stage07;
    [SerializeField] private StoryScene stage08;
    [SerializeField] private StoryScene stage09;
    [SerializeField] private StoryScene stage10;

    public void Stage1()
    {
        NovelInfoSet(false, "Stage 1", stage01);
        SaveManager.instance.LoadGame();
    }
    public void Stage2()
    {
        NovelInfoSet(false, "Stage 2", stage02);
        SaveManager.instance.LoadGame();
    }
    public void Stage3()
    {
        NovelInfoSet(false, "Stage 3", stage03);
        SaveManager.instance.LoadGame();
    }
    public void Stage4()
    {
        NovelInfoSet(false, "Stage 4", stage04);
        SaveManager.instance.LoadGame();
    }
    public void Stage5()
    {
        NovelInfoSet(false, "Stage 5", stage05);
        SaveManager.instance.LoadGame();
    }
    public void Stage6()
    {
        NovelInfoSet(false, "Stage 6", stage06);
        SaveManager.instance.LoadGame();
    }
    public void Stage7()
    {
        NovelInfoSet(false, "Stage 7", stage07);
        SaveManager.instance.LoadGame();
    }
    public void Stage8()
    {
        NovelInfoSet(false, "Stage 8", stage08);
        SaveManager.instance.LoadGame();
    }
    public void Stage9()
    {
        NovelInfoSet(false, "Stage 9", stage09);
        SaveManager.instance.LoadGame();
    }
    public void Stage10()
    {
        NovelInfoSet(false, "Stage 10", stage10);
        SaveManager.instance.LoadGame();
    }

    public void Title()
    {
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene("Title");
    }

    private void NovelInfoSet(bool title, string stageName, StoryScene scene)
    {
        NovelScriptManager.Instance.nextSceneisTitle = title;
        NovelScriptManager.Instance.nextPlayScene = scene;
        NovelScriptManager.Instance.nextSceneName = stageName;
        SceneManager.LoadScene("VisualNovel");
    }


    public void QuitApp()
    {
        Application.Quit();
    }
}

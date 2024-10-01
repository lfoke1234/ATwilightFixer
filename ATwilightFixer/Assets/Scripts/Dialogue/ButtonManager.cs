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
    [SerializeField] private StoryScene stage11;
    [SerializeField] private StoryScene stage12;
    [SerializeField] private StoryScene stage13;
    [SerializeField] private StoryScene stage14;
    [SerializeField] private StoryScene stage15;
    

    public void Stage1(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 1", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 1");
        }
    }
    public void Stage2(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 2", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 2");
        }
    }
    public void Stage3(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 3", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 3");
        }
    }

    public void Stage4(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 4", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 4");
        }
    }

    public void Stage5(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 5", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 5");
        }
    }
    public void Stage6(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 6", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 6");
        }
    }
    public void Stage7(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 7", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 7");
        }
    }
    public void Stage8(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 8", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 8");
        }
    }
    public void Stage9(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 9", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 9");
        }
    }
    public void Stage10(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 10", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 10");
        }
    }
    public void Stage11(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 11", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 11");
        }
    }
    public void Stage12(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 12", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 12");
        }
    }
    public void Stage13(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 13", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 13");
        }
    }
    public void Stage14(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 14", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 14");
        }
    }
    public void Stage15(bool skip)
    {
        if (skip == false)
        {
            NovelInfoSet(false, "Stage 15", stage01);
            SaveManager.instance.LoadGame();
        }
        else
        {
            SkipStoryScene("Stage 15");
        }
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

    private void SkipStoryScene(string stageName)
    {
        SceneManager.LoadScene(stageName);
        SaveManager.instance.LoadGame();
    }


    public void QuitApp()
    {
        Application.Quit();
    }
}

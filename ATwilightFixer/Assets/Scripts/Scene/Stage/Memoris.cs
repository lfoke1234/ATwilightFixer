using RPG.VisualNovel;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Memoris : MonoBehaviour
{
    private StoryScene scene;
    [SerializeField] GameObject Skip; 

    [SerializeField] private StoryScene stage01_1;
    [SerializeField] private StoryScene stage01_2;

    [SerializeField] private StoryScene stage02_1;
    [SerializeField] private StoryScene stage02_2;

    [SerializeField] private StoryScene stage03_1;
    [SerializeField] private StoryScene stage03_2;

    [SerializeField] private StoryScene stage04_1;
    [SerializeField] private StoryScene stage04_2;

    [SerializeField] private StoryScene stage05_1;
    [SerializeField] private StoryScene stage05_2;

    [SerializeField] private StoryScene stage06_1;
    [SerializeField] private StoryScene stage06_2;

    [SerializeField] private StoryScene stage07_1;
    [SerializeField] private StoryScene stage07_2;

    [SerializeField] private StoryScene stage08_1;
    [SerializeField] private StoryScene stage08_2;

    [SerializeField] private StoryScene stage09_1;
    [SerializeField] private StoryScene stage09_2;

    [SerializeField] private StoryScene stage10_1;
    [SerializeField] private StoryScene stage10_2;

    [SerializeField] private StoryScene stage11_1;
    [SerializeField] private StoryScene stage11_2;

    [SerializeField] private StoryScene stage12_1;
    [SerializeField] private StoryScene stage12_2;

    [SerializeField] private StoryScene stage13_1;
    [SerializeField] private StoryScene stage13_2;

    [SerializeField] private StoryScene stage14_1;
    [SerializeField] private StoryScene stage14_2;

    [SerializeField] private StoryScene stage15_1;
    [SerializeField] private StoryScene stage15_2;

    public void Stage01_1()
    {
        Skip.SetActive(true);
        scene = stage01_1;
    }
    public void Stage01_2()
    {
        Skip.SetActive(true);
        scene = stage01_2;
    }

    public void Stage02_1()
    {
        Skip.SetActive(true);
        scene = stage02_1;
    }
    public void Stage02_2()
    {
        Skip.SetActive(true);
        scene = stage02_2;
    }

    public void Stage03_1()
    {
        Skip.SetActive(true);
        scene = stage03_1;
    }
    public void Stage03_2()
    {
        Skip.SetActive(true);
        scene = stage03_2;
    }

    public void Stage04_1()
    {
        Skip.SetActive(true);
        scene = stage04_1;
    }
    public void Stage04_2()
    {
        Skip.SetActive(true);
        scene = stage04_2;
    }

    public void Stage05_1()
    {
        Skip.SetActive(true);
        scene = stage05_1;
    }
    public void Stage05_2()
    {
        Skip.SetActive(true);
        scene = stage05_2;
    }

    public void Stage06_1()
    {
        Skip.SetActive(true);
        scene = stage06_1;
    }
    public void Stage06_2()
    {
        Skip.SetActive(true);
        scene = stage06_2;
    }

    public void Stage07_1()
    {
        Skip.SetActive(true);
        scene = stage07_1;
    }
    public void Stage07_2()
    {
        Skip.SetActive(true);
        scene = stage07_2;
    }

    public void Stage08_1()
    {
        Skip.SetActive(true);
        scene = stage08_1;
    }
    public void Stage08_2()
    {
        Skip.SetActive(true);
        scene = stage08_2;
    }

    public void Stage09_1()
    {
        Skip.SetActive(true);
        scene = stage09_1;
    }
    public void Stage09_2()
    {
        Skip.SetActive(true);
        scene = stage09_2;
    }

    public void Stage10_1()
    {
        Skip.SetActive(true);
        scene = stage10_1;
    }
    public void Stage10_2()
    {
        Skip.SetActive(true);
        scene = stage10_2;
    }

    public void Stage11_1()
    {
        Skip.SetActive(true);
        scene = stage11_1;
    }
    public void Stage11_2()
    {
        Skip.SetActive(true);
        scene = stage11_2;
    }

    public void Stage12_1()
    {
        Skip.SetActive(true);
        scene = stage12_1;
    }
    public void Stage12_2()
    {
        Skip.SetActive(true);
        scene = stage12_2;
    }

    public void Stage13_1()
    {
        Skip.SetActive(true);
        scene = stage13_1;
    }
    public void Stage13_2()
    {
        Skip.SetActive(true);
        scene = stage13_2;
    }

    public void Stage14_1()
    {
        Skip.SetActive(true);
        scene = stage14_1;
    }
    public void Stage14_2()
    {
        Skip.SetActive(true);
        scene = stage14_2;
    }

    public void Stage15_1()
    {
        Skip.SetActive(true);
        scene = stage15_1;
    }
    public void Stage15_2()
    {
        Skip.SetActive(true);
        scene = stage15_2;
    }

    public void Set()
    {
        if (scene.IsUnityNull())
        {
            return;
        }

        NovelInfoSet(true, "MainMenu", scene);
    }

    private void NovelInfoSet(bool title, string stageName, StoryScene scene)
    {
        NovelScriptManager.Instance.nextSceneisTitle = title;
        NovelScriptManager.Instance.nextPlayScene = scene;
        NovelScriptManager.Instance.nextSceneName = stageName;
        SceneManager.LoadScene("VisualNovel");
    }
}

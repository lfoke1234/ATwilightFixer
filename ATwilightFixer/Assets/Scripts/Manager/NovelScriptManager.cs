using RPG.VisualNovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelScriptManager : MonoBehaviour
{
    public static NovelScriptManager Instance;

    public StoryScene nextPlayScene;
    public bool nextSceneisTitle;
    public string nextSceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }
}

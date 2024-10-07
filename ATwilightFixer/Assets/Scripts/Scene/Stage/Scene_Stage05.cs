using RPG.VisualNovel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Stage05 : Scene_Controller
{
    [Header("C")]
    [SerializeField] private bool nextSceneisTitle2;
    [SerializeField] private StoryScene nextNovelScript2;

    protected override void OnTriggerEnter2D(Collider2D collision)
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
            if (PlayerManager.instance.player.stats.currentHealth < PlayerManager.instance.player.stats.GetMaxHealthValue() * 0.5f)
            {
                LoadStageIntro();
            }
            else
            {
                LoadStageIntro2();
            }
            SceneManager.LoadScene("VisualNovel");
        }
    }

    protected void LoadStageIntro2()
    {
        NovelScriptManager.Instance.nextPlayScene = nextNovelScript2;
        NovelScriptManager.Instance.nextSceneisTitle = nextSceneisTitle2;
        NovelScriptManager.Instance.nextSceneName = "Null";
    }
}

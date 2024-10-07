using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Stage05 : Scene_Controller
{
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
            LoadStageIntro();
            SceneManager.LoadScene("VisualNovel");
        }
    }
}

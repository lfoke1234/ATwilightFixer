using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{
    public void Stage1()
    {
        SceneManager.LoadScene("Stage 1");
        SaveManager.instance.LoadGame();
    }
    public void Stage2()
    {
        SceneManager.LoadScene("Stage 2");
        SaveManager.instance.LoadGame();
    }
    public void Stage3()
    {
        SceneManager.LoadScene(3);
    }
    public void Stage4()
    {
        SceneManager.LoadScene(4);
    }
}

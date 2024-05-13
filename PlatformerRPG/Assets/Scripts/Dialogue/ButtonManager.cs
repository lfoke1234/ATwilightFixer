using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{
    public void Stage1()
    {
        SceneManager.LoadScene("Stage 1");
    }
    public void Stage2()
    {
        SceneManager.LoadScene("Stage 2");
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

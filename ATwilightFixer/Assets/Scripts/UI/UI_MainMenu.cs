using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "TestAction";
    [SerializeField] private UI_FadeScreen fadeScreen;

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneName);
    }

}

using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int[] nonPauseSceneIndexes;
    public bool isPlayCutScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoadAudio;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerManager.instance.player.stateMachine.ChangeState(PlayerManager.instance.player.deadState);
        }

    }

    public void RestartScene()
    {
        int aa = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(aa);
    }

    private void OnSceneLoadAudio(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 0:
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.DontPlayBGM();
                break;
            case 1:
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.DontPlayBGM();
                break;
            case 2:
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.DontPlayBGM();
                break;
            case 3:
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.DontPlayBGM();
                break;
        }
    }

    public void PausueGame(bool _pause)
    {
        if (!nonPauseSceneIndexes.Contains(SceneManager.GetActiveScene().buildIndex))
        {
            Time.timeScale = _pause ? 0 : 1;
        }
    }

    public void StopCutScene() => isPlayCutScene = false;
    public void PlayCutScene() => isPlayCutScene = true;
}

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


    public void RestartScene()
    {
        int aa = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(aa);
    }

    private void OnSceneLoadAudio(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 0: // Title
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(0);
                break;
            case 1: // Main
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(0);
                break;
            case 2: // novel
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.DontPlayBGM();
                break;
            case 3:// novel
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.DontPlayBGM();
                break;
            case 4:// stage 01
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(1);
                break;
            case 5:// stage 02
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(1);
                break;
            case 6:// stage 03
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(2);
                break;
            case 7:// stage 04
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(2);
                break;
            case 8:// stage 05
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(3);
                break;
            case 9:// stage 06
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(4);
                break;
            case 10:// stage 07
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(4);
                break;
            case 11:// stage 08
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(4);
                break;
            case 12:// stage 09
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(5);
                break;
            case 13:// stage 10
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(5);
                break;
            case 14:// stage 11
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(4);
                break;
            case 15:// stage 12
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(4);
                break;
            case 16:// stage 13
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(4);
                break;
            case 17:// stage 14
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(5);
                break;
            case 18:// stage 15
                AudioManager.instance.StopAllBgm();
                AudioManager.instance.PlayBGM(5);
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

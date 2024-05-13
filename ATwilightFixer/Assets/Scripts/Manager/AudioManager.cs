using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    private Dictionary<int, int> sceneBGMMap = new Dictionary<int, int>();

    public bool playBGM;
    private int bgmIndex;

    [SerializeField] private bool canPlaySFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        //ConfigureSceneBGMMap();
        //SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(EnableSFXAfterDelay(1f));
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (!playBGM)
            StopAllBgm();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                PlayBGM(bgmIndex);
            }
        }
    }

    // private void ConfigureSceneBGMMap()
    // {
    //     sceneBGMMap[3] = 6;
    // }
    // 
    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     if (sceneBGMMap.TryGetValue(scene.buildIndex, out int bgmIndex))
    //     {
    //         PlayBGM(bgmIndex);
    //     }
    // }

    public void PlaySFX(int _sfxIndex, Transform _transform)
    {
        if (canPlaySFX == false)
            return;

        if (sfx[_sfxIndex].isPlaying)
        {
            StopSFX(_sfxIndex);
        }

        if (_transform != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _transform.position) > sfxMinDistance)
            return;

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _sfxIndex) => sfx[_sfxIndex].Stop();

    public void StopSFXWithTime(int _index)
    {
        StartCoroutine(DecreaseVolume(sfx[_index]));
    }
    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.6f);

            if (_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }

    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        playBGM = true;
        bgmIndex = _bgmIndex;

        StopAllBgm();
        bgm[bgmIndex].Play();
    }

    public void StopAllBgm()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    public void DontPlayBGM() => playBGM = false;
    public void CanPlayBGM() => playBGM = true;

    private IEnumerator EnableSFXAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPlaySFX = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    // SFX, BGM 할당
    [SerializeField] private float sfxMinDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    // 씬과 BGM 연동맵 생성
    private Dictionary<int, int> sceneBGMMap = new Dictionary<int, int>();

    // 현재 재생되고 있는 BGM인덱스
    private int bgmIndex;

    // 재생 여부
    public bool playBGM;
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

    // 특정 SFX를 재생 (플레이어와의 거리에 따라)
    public void PlaySFX(int _sfxIndex, Transform _transform)
    {
        if (canPlaySFX == false) return; // SFX 재생이 불가능할 경우 종료

        if (sfx[_sfxIndex].isPlaying)
        {
            StopSFX(_sfxIndex);
        }

        // 플레이어와의 거리가 최소 거리보다 멀다면 SFX 재생하지 않음
        if (_transform != null 
            && Vector2.Distance(PlayerManager.instance.player.transform.position, _transform.position) > sfxMinDistance)
            return; 

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].Play(); // SFX 재생
        }
    }

    // 특정 SFX를 정지
    public void StopSFX(int _sfxIndex) => sfx[_sfxIndex].Stop();

    // SFX 볼륨을 점차 줄여가며 정지
    public void StopSFXWithTime(int _index)
    {
        StartCoroutine(DecreaseVolume(sfx[_index]));
    }

    // 볼륨을 점차 줄이는 코루틴
    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f; // 볼륨 감소
            yield return new WaitForSeconds(.6f);

            if (_audio.volume <= .1f)
            {
                _audio.Stop(); // 볼륨이 충분히 줄어들면 SFX 정지
                _audio.volume = defaultVolume; // 볼륨 초기화
                break;
            }
        }
    }

    // 랜덤한 BGM을 선택하여 재생
    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length); // BGM 인덱스를 랜덤하게 선택
        PlayBGM(bgmIndex); // 선택한 BGM 재생
    }

    // 특정 BGM을 재생
    public void PlayBGM(int _bgmIndex)
    {
        playBGM = true;
        bgmIndex = _bgmIndex;

        StopAllBgm(); // 다른 BGM이 재생 중이라면 모두 정지
        bgm[bgmIndex].Play(); // 지정된 인덱스의 BGM 재생
    }

    // 모든 BGM 정지
    public void StopAllBgm()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    // BGM 재생 중지
    public void DontPlayBGM() => playBGM = false;

    // BGM 재생 가능 설정
    public void CanPlayBGM() => playBGM = true;

    // SFX 재생 가능 설정을 딜레이 후 활성화하는 코루틴
    private IEnumerator EnableSFXAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPlaySFX = true;
    }
}

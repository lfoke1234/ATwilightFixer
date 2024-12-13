using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    // SFX, BGM �Ҵ�
    [SerializeField] private float sfxMinDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    // ���� BGM ������ ����
    private Dictionary<int, int> sceneBGMMap = new Dictionary<int, int>();

    // ���� ����ǰ� �ִ� BGM�ε���
    private int bgmIndex;

    // ��� ����
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

    // Ư�� SFX�� ��� (�÷��̾���� �Ÿ��� ����)
    public void PlaySFX(int _sfxIndex, Transform _transform)
    {
        if (canPlaySFX == false) return; // SFX ����� �Ұ����� ��� ����

        if (sfx[_sfxIndex].isPlaying)
        {
            StopSFX(_sfxIndex);
        }

        // �÷��̾���� �Ÿ��� �ּ� �Ÿ����� �ִٸ� SFX ������� ����
        if (_transform != null 
            && Vector2.Distance(PlayerManager.instance.player.transform.position, _transform.position) > sfxMinDistance)
            return; 

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].Play(); // SFX ���
        }
    }

    // Ư�� SFX�� ����
    public void StopSFX(int _sfxIndex) => sfx[_sfxIndex].Stop();

    // SFX ������ ���� �ٿ����� ����
    public void StopSFXWithTime(int _index)
    {
        StartCoroutine(DecreaseVolume(sfx[_index]));
    }

    // ������ ���� ���̴� �ڷ�ƾ
    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f; // ���� ����
            yield return new WaitForSeconds(.6f);

            if (_audio.volume <= .1f)
            {
                _audio.Stop(); // ������ ����� �پ��� SFX ����
                _audio.volume = defaultVolume; // ���� �ʱ�ȭ
                break;
            }
        }
    }

    // ������ BGM�� �����Ͽ� ���
    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length); // BGM �ε����� �����ϰ� ����
        PlayBGM(bgmIndex); // ������ BGM ���
    }

    // Ư�� BGM�� ���
    public void PlayBGM(int _bgmIndex)
    {
        playBGM = true;
        bgmIndex = _bgmIndex;

        StopAllBgm(); // �ٸ� BGM�� ��� ���̶�� ��� ����
        bgm[bgmIndex].Play(); // ������ �ε����� BGM ���
    }

    // ��� BGM ����
    public void StopAllBgm()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    // BGM ��� ����
    public void DontPlayBGM() => playBGM = false;

    // BGM ��� ���� ����
    public void CanPlayBGM() => playBGM = true;

    // SFX ��� ���� ������ ������ �� Ȱ��ȭ�ϴ� �ڷ�ƾ
    private IEnumerator EnableSFXAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPlaySFX = true;
    }
}

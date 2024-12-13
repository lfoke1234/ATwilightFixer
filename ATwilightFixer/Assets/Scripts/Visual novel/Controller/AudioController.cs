using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource musicSource; 
    public AudioSource soundSource; 

    // 배경 음악과 효과음을 재생
    public void PlayAudio(AudioClip music, AudioClip sound)
    {
        if (sound != null)
        {
            soundSource.clip = sound;
            soundSource.Play();
        }

        if (music != null && musicSource.clip != music)
        {
            StartCoroutine(SwiutchMusic(music));
        }
    }

    // 배경 음악을 부드럽게 전환하는 코루틴
    private IEnumerator SwiutchMusic(AudioClip music)
    {
        // 페이드 아웃합니다.
        if (musicSource.clip != null)
        {
            while (musicSource.volume > 0)
            {
                musicSource.volume -= 0.05f; 
                yield return new WaitForSeconds(0.05f); 
            }
        }
        else
        {
            musicSource.volume = 0;
        }

        musicSource.clip = music;
        musicSource.Play();

        // 페이드 인
        while (musicSource.volume < 0.5)
        {
            musicSource.volume += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}

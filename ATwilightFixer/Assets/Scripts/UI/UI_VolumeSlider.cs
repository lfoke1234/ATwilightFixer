using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parameter;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multuplier;

    // 슬라이더 값에 따라 오디오 믹서의 볼륨을 조절합니다.
    public void SlierValue(float _value) => audioMixer.SetFloat(parameter, Mathf.Log10(_value) * multuplier);

    // 슬라이더의 값을 로드하여 설정합니다.
    public void LoadSlider(float _value)
    {
        if (_value >= 0.001f)
            slider.value = _value;
    }
}

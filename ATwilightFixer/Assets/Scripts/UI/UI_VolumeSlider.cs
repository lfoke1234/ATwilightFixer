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

    // �����̴� ���� ���� ����� �ͼ��� ������ �����մϴ�.
    public void SlierValue(float _value) => audioMixer.SetFloat(parameter, Mathf.Log10(_value) * multuplier);

    // �����̴��� ���� �ε��Ͽ� �����մϴ�.
    public void LoadSlider(float _value)
    {
        if (_value >= 0.001f)
            slider.value = _value;
    }
}

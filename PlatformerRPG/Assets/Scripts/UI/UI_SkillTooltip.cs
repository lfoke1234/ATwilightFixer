using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private Image skillIcon;

    public void SetTooltip(string _text, string _name, Image _icon)
    {
        skillName.text = _name;
        skillText.text = _text;
        skillIcon.sprite = _icon.sprite;
    }
}
